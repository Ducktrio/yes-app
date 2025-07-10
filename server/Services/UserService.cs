using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Repositories;

namespace Yes.Services;

public interface IUserService
{
    Task<UserContract?> GetLoggedInUser(string token);
    Task<LoginResponseContract?> Login(LoginContract loginContract);
}

public class UserService(IAuthenticationService authenticationService, IUserRepository userRepository, IMapper mapper) : IUserService
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserContract?> GetLoggedInUser(string token)
    {
        var userId = _authenticationService.GetId(token);
        if (userId == null) return null;
        var user = (await _userRepository.Get(userId, null, null)).FirstOrDefault();
        if (user == null) return null;
        return _mapper.Map<UserContract>(user);
    }

    public async Task<LoginResponseContract?> Login(LoginContract loginContract)
    {
        var user = (await _userRepository.Get(null, null, loginContract.Username)).FirstOrDefault();
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginContract.Password, user.Password)) return null;
        return new LoginResponseContract
        {
            Token = _authenticationService.GenerateToken(user),
            User = _mapper.Map<UserContract>(user)
        };
    }
}
