


namespace Service.MappingProfiles
{
    internal class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();       
        }
    }
}
