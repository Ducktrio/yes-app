using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserContract>();
        CreateMap<CreateUserContract, User>()
            .ForMember(p => p.Password, option => option.MapFrom(source => BCrypt.Net.BCrypt.HashPassword(source.Password)));
        CreateMap<UpdateUserContract, User>()
            .ForMember(p => p.Password, option => option.MapFrom((src, dest) =>
                !string.IsNullOrEmpty(src.Password) ? BCrypt.Net.BCrypt.HashPassword(src.Password) : dest.Password))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
