using OnlineBazar.Domain.Entities;

namespace OnlineBazar.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}
