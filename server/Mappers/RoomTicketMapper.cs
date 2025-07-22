using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Utilities;

namespace Yes.Mappers;

public class RoomTicketMapper : Profile
{
    public RoomTicketMapper()
    {
        CreateMap<RoomTicket, RoomTicketContract>()
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate.HasValue ? DatetimeUtility.ToUnixTimestampString(src.CheckInDate.Value) : null))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate.HasValue ? DatetimeUtility.ToUnixTimestampString(src.CheckOutDate.Value) : null))
            .ForMember(dest => dest.Created_at, opt => opt.MapFrom(src => DatetimeUtility.ToUnixTimestampString(src.Created_at)));
        CreateMap<CreateRoomTicketContract, RoomTicket>();
        CreateMap<UpdateRoomTicketContract, RoomTicket>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
