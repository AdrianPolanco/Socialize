using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Helper;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Application.UseCases.ResetPassword;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Middlewares;
using Socialize.Presentation.Models;
using Socialize.Presentation.Models.Users;
using System.Diagnostics;

namespace Socialize.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICreateUserUseCase _createNewUserUseCase;
        private readonly IResetPasswordUseCase _resetPassword;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly IEmailSender _emailSender;
        private readonly IRepository<User> _userRepository; 

        public HomeController(IMapper mapper, 
            ICreateUserUseCase createNewUserUseCase,
            IResetPasswordUseCase resetPassword,
            UserManager<ApplicationUser> userManager, 
            IEmailSender emailSender, 
            IRepository<User> repository,
            SignInManager<ApplicationUser> signInManager)
        {
            _mapper = mapper;
            _createNewUserUseCase = createNewUserUseCase;
            _userManager = userManager;
            _emailSender = emailSender;
            _userRepository = repository;
            _signInManager = signInManager;
            _resetPassword = resetPassword;
        }

        [RedirectToPostsFilter]
        public IActionResult Index()
        {
            return View();
        }

        [RedirectToPostsFilter]
        public IActionResult SignUp()
        {
            return View();
        }

        [RedirectToPostsFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if(!ModelState.IsValid) return View("Index", loginUserViewModel);

            ApplicationUser user = await _userManager.FindByNameAsync(loginUserViewModel.Username);

            if(user is null)
            {
                ModelState.AddModelError("Username", "Username not found");
                return View("Index", loginUserViewModel);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("Username", "Email not confirmed, please, verify your account using your email to be able to sign in.");
                return View("Index", loginUserViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginUserViewModel.Password, true, true);

            if(result.Succeeded) return RedirectToAction("Index", "Posts");

            ModelState.AddModelError("Password", "Invalid password");
            return View("Index", loginUserViewModel);
  
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [RedirectToPostsFilter]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUserViewModel registerUserViewModel, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return View("SignUp", registerUserViewModel);

            ApplicationUser UserExists = await _userManager.FindByNameAsync(registerUserViewModel.Username);
            if (UserExists is not null) { 
                ModelState.AddModelError("Username", "Username already exists");
                return View("SignUp", registerUserViewModel);
            }

            ApplicationUser userEmailExists = await _userManager.FindByEmailAsync(registerUserViewModel.Email);
            if (userEmailExists is not null)
            {
                ModelState.AddModelError("Email", "Your email has already been registered.");
                return View("SignUp", registerUserViewModel);
            }

            (Stream stream, string fileName) = await registerUserViewModel.Image.ConvertToStreamAsync(cancellationToken);

            User user = _mapper.Map<User>(registerUserViewModel);
            user.PhotoUrl = fileName;

            bool result = await _createNewUserUseCase.ExecuteAsync(user, stream, fileName, cancellationToken);

            if(result)
            {
                ApplicationUser createdUser = await _userManager.FindByNameAsync(user.Username);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
                var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = createdUser.Id, code }, protocol: Request.Scheme);

                string emailTemplate = Mail.GenerateMailTemplate($"{createdUser.Name} {createdUser.Lastname}","Please confirm your account in order to start using Socialize", callbackUrl);

                await _emailSender.SendEmailAsync(createdUser.Email, "Confirm your account - Socialize", emailTemplate);
                TempData["SuccessMessage"] = "Registration successful! Please check your email to confirm your account.";
            }

            return RedirectToAction("SignUp");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return RedirectToAction("Index", "Home");
            

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");
            

            var result = await _userManager.ConfirmEmailAsync(user, code);
            var userEntity = await _userRepository.GetByIdAsync(Guid.Parse(userId), new CancellationToken());
            userEntity.IsActived = true;
            await _userRepository.UpdateAsync(userEntity, new CancellationToken());
            if (result.Succeeded) return View("ConfirmedEmail");
            else return View("Error");
            
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendPassword(ResetUserPasswordViewModel resetUserPasswordViewModel)
        {
            if(!ModelState.IsValid) return View("ResetPassword", resetUserPasswordViewModel);   

            ApplicationUser user = await _userManager.FindByNameAsync(resetUserPasswordViewModel.Username);

            if(user is null)
            {
                ModelState.AddModelError("Username", "Username not found");
                return View("ResetPassword", resetUserPasswordViewModel);
            }

            var result = await _resetPassword.ResetPassword(user.UserName, user.Email, user.Name, user.Lastname);

            if(result)  TempData["SuccessMessage"] = "Password reset successful! Please check your email to get your new password.";
            else TempData["ErrorMessage"] = "An error occurred while trying to reset your password. Please try again later.";  

            return RedirectToAction("ResetPassword");
        }

    }
}
