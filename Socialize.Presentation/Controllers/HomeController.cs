﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Domain.Entities;
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

        public HomeController(IMapper mapper, ICreateNewUserUseCase createNewUserUseCase, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _mapper = mapper;
            _createNewUserUseCase = createNewUserUseCase;
            _userManager = userManager;
            _emailSender = emailSender;
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
                var code = _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = createdUser.Id, code }, protocol: Request.Scheme);

                string emailTemplate = Mail.GenerateMailTemplate("Please confirm your account in order to start using Socialize", callbackUrl);

                await _emailSender.SendEmailAsync(createdUser.Email, "Confirm your account - Socialize", emailTemplate);
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
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View("ConfirmedEmail");
            }
            else
            {
                return View("Error");
            }
        }

    }
}
