
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Contracts.Models;

namespace CustomerApi.Contracts.Interfaces
{
    public interface ICustomerService : IDisposable
    {
        Task<List<Customer>> GetCustomersByNameAsync(string searchTerm);

        Task<Customer> AddCustomerAsync(Customer customer);

        Task<Customer> UpdateCustomerAsync(int id, Customer customer);

        Task DeleteCustomerAsync(int id);
    }
}
