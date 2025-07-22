using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;
using Yes.Utilities;

namespace Yes.Mappers;

public class ServiceTicketMapper : Profile
{
    public ServiceTicketMapper()
    {
        CreateMap<ServiceTicket, ServiceTicketContract>()
            .ForMember(dest => dest.Created_at, opt => opt.MapFrom(src => DatetimeUtility.ToUnixTimestampString(src.Created_at)));
        CreateMap<CreateServiceTicketContract, ServiceTicket>();
        CreateMap<UpdateServiceTicketContract, ServiceTicket>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
