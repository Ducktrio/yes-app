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
    Task<List<UserContract>> Get(string? id = null, string? role_id = null, string? username = null);
    Task<UserContract?> Create(CreateUserContract createUser);
    Task<UserContract?> Update(string id, UpdateUserContract updateUser);
    Task<UserContract?> Delete(string id);
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

    public async Task<List<UserContract>> Get(string? id = null, string? role_id = null, string? username = null)
    {
        return _mapper.Map<List<UserContract>>(await _userRepository.Get(id, role_id, username));
    }

    public async Task<UserContract?> Create(CreateUserContract createUser)
    {
        var user = await _userRepository.Create(_mapper.Map<User>(createUser));
        return _mapper.Map<UserContract>(user);
    }

    public async Task<UserContract?> Update(string id, UpdateUserContract updateUser)
    {
        var existingUser = (await _userRepository.Get(id, null, null)).FirstOrDefault();
        if (existingUser == null) return null;
        _mapper.Map(updateUser, existingUser);
        var updatedUser = await _userRepository.Update(existingUser);
        return _mapper.Map<UserContract>(updatedUser);
    }

    public async Task<UserContract?> Delete(string id)
    {
        var deletedUser = await _userRepository.Delete(id);
        return _mapper.Map<UserContract>(deletedUser);
    }
}
