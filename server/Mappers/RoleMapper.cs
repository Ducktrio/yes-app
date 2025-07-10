using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleContract>();
        CreateMap<CreateRoleContract, Role>();
        CreateMap<UpdateRoleContract, Role>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
