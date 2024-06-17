using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.UseCases.CreateNewUser;
using Socialize.Core.Domain.Entities;
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

        public HomeController(IMapper mapper, ICreateNewUserUseCase createNewUserUseCase)
        {
            _mapper = mapper;
            _createNewUserUseCase = createNewUserUseCase;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(RegisterUserViewModel registerUserViewModel, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return View(registerUserViewModel);

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
