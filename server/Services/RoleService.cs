using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Repositories;

namespace Yes.Services;

public interface IRoleService
{
    Task<List<RoleContract>> Get(string? id = null);
}

public class RoleService(IRoleRepository roleRepository, IMapper mapper) : IRoleService
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<List<RoleContract>> Get(string? id = null)
    {
        return _mapper.Map<List<RoleContract>>(await _roleRepository.Get(id));
    }
}
