using System;
using AutoMapper;
using Yes.Contracts;
using Yes.Models;

namespace Yes.Mappers;

public class CustomerMapper : Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, CustomerContract>();
        CreateMap<CreateCustomerContract, Customer>();
        CreateMap<UpdateCustomerContract, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
