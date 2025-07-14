using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class ServiceMapper : Profile
{
    public ServiceMapper()
    {
        CreateMap<Service, ServiceContract>();
        CreateMap<CreateServiceContract, Service>();
        CreateMap<UpdateServiceContract, Service>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
