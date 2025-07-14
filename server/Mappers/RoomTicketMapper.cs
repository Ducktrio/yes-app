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
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate.HasValue ? DatetimeUtility.ToUnixTimestampString(src.CheckOutDate.Value) : null));
        CreateMap<CreateRoomTicketContract, RoomTicket>()
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate != null ? DatetimeUtility.FromUnixTimestampString(src.CheckInDate) : (DateTime?)null))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate != null ? DatetimeUtility.FromUnixTimestampString(src.CheckOutDate) : (DateTime?)null));
        CreateMap<UpdateRoomTicketContract, RoomTicket>()
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate != null ? DatetimeUtility.FromUnixTimestampString(src.CheckInDate) : (DateTime?)null))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate != null ? DatetimeUtility.FromUnixTimestampString(src.CheckOutDate) : (DateTime?)null))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
