


using Microsoft.AspNetCore.Identity;
using Socialize.Core.Application.Adapters;
using Socialize.Core.Application.Helper;
using Socialize.Core.Application.Services.Interfaces;
using Socialize.Core.Domain.Entities;
using Socialize.Core.Domain.ValueObjects;

namespace Socialize.Core.Application.UseCases.ResetPassword
{
    public class ResetPasswordUseCase : IResetPasswordUseCase
    {
        private readonly IUserManagerAdapter _userManagerAdapter;
        private readonly IEmailSender _emailSender;
        public ResetPasswordUseCase(IUserManagerAdapter userManagerAdapter, IEmailSender emailSender)
        {
            _userManagerAdapter = userManagerAdapter;
            _emailSender = emailSender;
        }
        public async Task<bool> ResetPassword(string username, string email, string name, string lastname)
        {

            var password = PasswordGenerator.GenerateRandomPassword();
            var result = await _userManagerAdapter.ResetPassword(username, password);

            if (result)
            {
                string emailTemplate = Mail.GenerateResetPasswordMailTemplate($"{name} {lastname}", password);

                await _emailSender.SendEmailAsync(email, "Password reset - Socialize", emailTemplate);

                return true;
            }

            return false;
        }
    }
}
