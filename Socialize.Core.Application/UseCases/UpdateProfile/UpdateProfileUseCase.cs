
using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Services.Base;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;

namespace Socialize.Core.Application.UseCases.UpdateProfile
{
    public class UpdateProfileUseCase : IUpdateProfileUseCase
    {
        private readonly IUserManagerAdapter _userManagerAdapter;
        private readonly IEntityService<User> _userService;
        private readonly IFileService<User> _fileService;

        public UpdateProfileUseCase(IUserManagerAdapter userManagerAdapter, IEntityService<User> userService, IFileService<User> fileService)
        {
            _userManagerAdapter = userManagerAdapter;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<bool> ExecuteAsync(User user, Stream stream, string fileName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(user.Name)
                || string.IsNullOrEmpty(user.Username)
                || string.IsNullOrEmpty(user.Email.Value)
                || string.IsNullOrEmpty(user.PhoneNumber.Value)) return false;

            User fetchedUser = await _userService.GetByIdAsync(user.Id, cancellationToken, false);

            if(fetchedUser is null) return false;

            if(stream is not null && !string.IsNullOrEmpty(fileName))
            {
                string imageUrl = await _fileService.UploadImageAsync(stream, fileName, user.Username, cancellationToken);

                user.PhotoUrl = imageUrl;
            }
            
            if(user.PhotoUrl is null) user.PhotoUrl =  fetchedUser.PhotoUrl;
            await _userManagerAdapter.UpdateUser(user);

            if(!string.IsNullOrEmpty(user.Password)) await _userManagerAdapter.HashPassword(fetchedUser.Id, user.Password);

            fetchedUser.Name = user.Name;
            fetchedUser.Lastname = user.Lastname;
            fetchedUser.Username = user.Username;
            fetchedUser.PhoneNumber = user.PhoneNumber;
            fetchedUser.Email = user.Email;
            if (stream is not null && !string.IsNullOrEmpty(fileName)) fetchedUser.PhotoUrl = user.PhotoUrl;

            await _userService.UpdateAsync(fetchedUser, cancellationToken);

            return true;
        }
    }
}
