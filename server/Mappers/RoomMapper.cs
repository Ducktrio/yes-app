using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class RoomMapper : Profile
{
    public RoomMapper()
    {
        CreateMap<Room, RoomContract>();
        CreateMap<CreateRoomContract, Room>();
        CreateMap<UpdateRoomContract, Room>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
