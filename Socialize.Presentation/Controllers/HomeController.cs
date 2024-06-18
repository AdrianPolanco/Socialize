using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Domain.Entities;
using Socialize.Infrastructure.Identity.Models;
using Socialize.Presentation.Extensions;
using Socialize.Presentation.Models;
using Socialize.Presentation.Models.Users;
using System.Diagnostics;

namespace Socialize.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICreateNewUserUseCase _createNewUserUseCase;
        private readonly IMapper _mapper;
        private readonly IUserManagerAdapter _userManagerAdapter;

        public HomeController(IMapper mapper, ICreateNewUserUseCase createNewUserUseCase, IUserManagerAdapter userManagerAdapter)
        {
            _mapper = mapper;
            _createNewUserUseCase = createNewUserUseCase;
            _userManagerAdapter = userManagerAdapter;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(RegisterUserViewModel registerUserViewModel, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return View("Index", registerUserViewModel);

            bool UserExists = await _userManagerAdapter.UserExists(registerUserViewModel.Username, cancellationToken);
            if (UserExists) { 
                ModelState.AddModelError("Username", "Username already exists");
                return View("Index", registerUserViewModel);
            }

            (Stream stream, string fileName) = await registerUserViewModel.Image.ConvertToStreamAsync(cancellationToken);

            User user = _mapper.Map<User>(registerUserViewModel);
            user.PhotoUrl = fileName;

            await _createNewUserUseCase.ExecuteAsync(user, stream, fileName, cancellationToken);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
