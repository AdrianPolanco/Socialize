using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.UseCases.CreateNewUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserManagerAdapter _userManagerAdapter;
        private readonly IEntityService<User> _userService; 
        private readonly IFileService<User> _fileService;

        public CreateUserUseCase(IUserManagerAdapter userManagerAdapter, IEntityService<User> userService, IFileService<User> fileService)
        {
            _userManagerAdapter = userManagerAdapter;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<bool> ExecuteAsync(User user, Stream stream, string fileName, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(user.Name)
                || string.IsNullOrEmpty(user.Username)
                || string.IsNullOrEmpty(user.Password)
                || string.IsNullOrEmpty(user.Email.Value)
                || string.IsNullOrEmpty(user.PhoneNumber.Value)
                || stream == null
                || string.IsNullOrEmpty(fileName)) return false;

            if (await _userManagerAdapter.UserExists(user.Username, cancellationToken)) return false;
            
            string imageUrl = await _fileService.UploadImageAsync(stream, fileName, user.Username, cancellationToken);

            user.PhotoUrl = imageUrl;

            await _userService.AddAsync(user, cancellationToken);

            return true;
        }
    }
}
