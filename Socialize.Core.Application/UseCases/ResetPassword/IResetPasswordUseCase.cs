

namespace Socialize.Core.Application.UseCases.ResetPassword
{
    public interface IResetPasswordUseCase
    {
        Task<bool> ResetPassword(string username, string email, string name, string lastname);
    }
}
