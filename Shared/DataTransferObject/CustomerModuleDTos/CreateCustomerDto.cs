


namespace Shared.DataTransferObject.CustomerModuleDTos
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
