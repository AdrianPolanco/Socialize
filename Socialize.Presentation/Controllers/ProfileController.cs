using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.UpdateProfile;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Filters;
using Socialize.Presentation.Models.Profile;


namespace Socialize.Presentation.Controllers
{
    [Authorize]
    [RedirectToLoginIfNotAuthenticated]
    public class ProfileController : Controller
    {
        private readonly IEntityService<User> _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUpdateProfileUseCase _updateProfileUseCase;

        public ProfileController(IEntityService<User> userService, UserManager<ApplicationUser> userManager, IMapper mapper, IUpdateProfileUseCase updateProfileUseCase, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
            _updateProfileUseCase = updateProfileUseCase;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            Guid currentUserId = Guid.Parse(currentUser.Id);
            User user = await _userService.GetByIdAsync(currentUserId, new CancellationToken(), true);
            EditProfileViewModel editProfileViewModel = _mapper.Map<EditProfileViewModel>(user);
            return View(editProfileViewModel);
        }

        public async Task<IActionResult> Update(EditProfileViewModel editProfileViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View("Index", editProfileViewModel);

            Stream? _stream = null;
            string? _fileName = null;

            if(editProfileViewModel.Image is not null)
            {
                IFormFile image = editProfileViewModel.Image;
                (Stream stream, string fileName) = await image.ConvertToStreamAsync(cancellationToken);

                _fileName = fileName;
                _stream = stream; 
            }

            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            editProfileViewModel.PhotoUrl = _fileName;

            User user = _mapper.Map<User>(editProfileViewModel);
            user.PhoneNumber = PhoneNumber.Create(editProfileViewModel.Phone);
            

            bool result = await _updateProfileUseCase.ExecuteAsync(user, _stream, _fileName, cancellationToken);

            if (result) TempData["Message"] = "Your account has been updated succesfully!.";
            else TempData["Message"] = "There has been an error updating your account.";

            if (result) await _signInManager.RefreshSignInAsync(applicationUser);

            return RedirectToAction("Index");
        }
    }
}
