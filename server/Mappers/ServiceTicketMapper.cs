using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class ServiceTicketMapper : Profile
{
    public ServiceTicketMapper()
    {
        CreateMap<ServiceTicket, ServiceTicketContract>();
        CreateMap<CreateServiceTicketContract, ServiceTicket>();
        CreateMap<UpdateServiceTicketContract, ServiceTicket>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
