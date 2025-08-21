using SmartTaskPro.Models;



namespace SmartTaskPro.Services.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user, IList<string> roles);
}
