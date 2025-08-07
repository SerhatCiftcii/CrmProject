using CrmProject.Domain.Entities;


namespace CrmProject.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user, IList<string> roles);
    }
}
