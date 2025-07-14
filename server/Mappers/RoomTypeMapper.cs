using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class RoomTypeMapper : Profile
{
    public RoomTypeMapper()
    {
        CreateMap<RoomType, RoomTypeContract>();
        CreateMap<CreateRoomTypeContract, RoomType>();
        CreateMap<UpdateRoomTypeContract, RoomType>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
