using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Infrastructure.Shared.Services.Interfaces;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Helpers;
using Socialize.Presentation.Models;
using Socialize.Presentation.Models.Users;
using System.Diagnostics;

namespace Socialize.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICreateNewUserUseCase _createNewUserUseCase;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<User> _userRepository; 

        public HomeController(IMapper mapper, ICreateNewUserUseCase createNewUserUseCase, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IRepository<User> repository)
        {
            _mapper = mapper;
            _createNewUserUseCase = createNewUserUseCase;
            _userManager = userManager;
            _emailSender = emailSender;
            _userRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(RegisterUserViewModel registerUserViewModel, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return View("Index", registerUserViewModel);

            ApplicationUser UserExists = await _userManager.FindByNameAsync(registerUserViewModel.Username);
            if (UserExists is not null) { 
                ModelState.AddModelError("Username", "Username already exists");
                return View("Index", registerUserViewModel);
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

            return RedirectToAction("Index");
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

    }
}
