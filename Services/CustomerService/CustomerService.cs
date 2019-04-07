using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Contracts.Exceptions;
using CustomerApi.Contracts.Interfaces;
using CustomerApi.Contracts.Models;
using CustomerApi.Services.CustomerService.Validators;
using CustomerApi.DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _customerDbContext;

        public CustomerService(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext;
        }

        public async Task<List<Customer>> GetCustomersByNameAsync(string searchTerm)
        {
            var customers = new List<Customer>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                customers = await _customerDbContext.Customers.Where(cust => 
                            cust.FirstName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            cust.LastName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToListAsync();
            }

            return customers;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            Validator.ValidateCustomerDetails(customer);

            var response = await _customerDbContext.AddAsync(customer);

            await _customerDbContext.SaveChangesAsync();

            return response.Entity;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            Validator.ValidateCustomerId(id);

            var customer = _customerDbContext.Customers.FirstOrDefault(cust => cust.Id == id);
            if (customer == null)
            {
                throw new NoResultFoundException($"Customer with id {id} not found.");
            }
                
            _customerDbContext.Customers.Remove(customer);
            await _customerDbContext.SaveChangesAsync();
        }

        public async Task<Customer> UpdateCustomerAsync(int id, Customer customer)
        {
            Validator.ValidateUpdateCustomerRequest(id, customer);

            var searchResult = _customerDbContext.Customers.FirstOrDefault(cust => cust.Id == id);
            if (searchResult == null)
            {
                throw new NoResultFoundException($"Customer with id {id} not found.");
            }

            searchResult.FirstName = customer.FirstName;
            searchResult.LastName = customer.LastName;
            searchResult.BirthDate = customer.BirthDate;

            _customerDbContext.Customers.Update(searchResult);
            await _customerDbContext.SaveChangesAsync();

            return searchResult;
        }

        public void Dispose()
        {
            _customerDbContext.Dispose();
        }
    }
}
