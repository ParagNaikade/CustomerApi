using CustomerApi.Contracts.Exceptions;
using CustomerApi.Contracts.Models;

namespace CustomerApi.Services.CustomerService.Validators
{
    public static class Validator
    {
        public static void ValidateCustomerDetails(Customer customer)
        {
            if (customer == null)
            {
                throw new BadRequestException("Customer cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                throw new BadRequestException("Customer's first name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(customer.LastName))
            {
                throw new BadRequestException("Customer's last name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(customer.BirthDate))
            {
                throw new BadRequestException("Customer's birth date cannot be empty.");
            }
        }

        public static void ValidateCustomerId(int id)
        {
            if(id <= 0)
            {
                throw new BadRequestException("Customer id is invalid. It should be greater than zero.");
            }
        }

        public static void ValidateUpdateCustomerRequest(int id, Customer customer)
        {
            ValidateCustomerId(id);

            ValidateCustomerDetails(customer);
        }
    }
}
