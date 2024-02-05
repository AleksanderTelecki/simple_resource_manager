using Resource.Messages.Models;

namespace Resource.Endpoint.Abstract;

public interface IAuthService
{
    Task<(int, string)> Registeration(RegistrationModel model, string role);
    Task<(int, string)> Login(LoginModel model);
}