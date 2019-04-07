using CustomerApi.Contracts.Models;
using CustomerApi.DataAccessLayer;
using CustomerApi.Services.CustomerService;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.UnitTests
{
    public class CustomerServiceTests
    {
        [Fact]
        public void GetCustomersByNameAsync_SearchTermEmpty_ReturnsEmptyCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                using (var service = new CustomerService(context))
                {
                    var customers = service.GetCustomersByNameAsync(string.Empty).Result;

                    customers.Should().BeNullOrEmpty();
                }
            }
        }

        [Fact]
        public void GetCustomersByNameAsync_ValidSearchTerm_ReturnsMatchingCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                // Seed some data
                context.Customers.AddRange(
                    new Customer()
                    {
                        FirstName = "jane",
                        LastName = "dow"
                    },
                    new Customer()
                    {
                        FirstName = "john",
                        LastName = "dow"
                    });

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test 
            using (var context = new CustomerDbContext(options))
            {
                using (var service = new Services.CustomerService.CustomerService(context))
                {
                    // Act
                    var customers = service.GetCustomersByNameAsync("JA").Result;

                    // Assert
                    var expectedCustomer = new Customer()
                    {
                        FirstName = "jane",
                        LastName = "dow"
                    };

                    customers.Count.Should().Be(1);

                    customers.First().Should().BeEquivalentTo(expectedCustomer, config => config.Excluding(obj => obj.Id));
                }
            }
        }

        [Fact]
        public void GetCustomersByNameAsync_InvalidSearchTerm_ReturnsEmptyCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                // Seed some data
                context.Customers.Add(
                    new Customer()
                    {
                        FirstName = "jane",
                        LastName = "dow"
                    });

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test 
            using (var context = new CustomerDbContext(options))
            {
                using (var service = new Services.CustomerService.CustomerService(context))
                {
                    // Act
                    var customers = service.GetCustomersByNameAsync("invalid").Result;

                    // Assert
                    customers.Should().BeNullOrEmpty();
                }
            }
        }

        [Fact]
        public void AddCustomerAsync_ValidCustomer_CustomerAddedSuccessfully()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                using (var service = new Services.CustomerService.CustomerService(context))
                {
                    // First check that there are no customers.
                    var customers = service.GetCustomersByNameAsync("JA").Result;

                    customers.Count.Should().Be(0);

                    // Add single customer
                    var customer = service.AddCustomerAsync(new Customer()
                                        {
                                            FirstName = "jane",
                                            LastName = "dow",
                                            BirthDate = "01-01-1990"
                                        }).Result;

                    customers = service.GetCustomersByNameAsync("JA").Result;

                    // Now check if customer is added or not.
                    customers.Count.Should().Be(1);
                }
            }
        }

        [Fact]
        public async Task DeleteCustomerAsync_ValidCustomer_CustomerDeletedSuccessfully()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                using (var service = new Services.CustomerService.CustomerService(context))
                {
                    // Add single customer
                    var customer = service.AddCustomerAsync(new Customer()
                    {
                        FirstName = "jane",
                        LastName = "dow",
                        BirthDate = "01-01-1990"
                    }).Result;

                    // Check that customer is added or not.
                    var customers = service.GetCustomersByNameAsync("JA").Result;

                    customers.Count.Should().Be(1);

                    // Delete single customer
                    await service.DeleteCustomerAsync(customer.Id);

                    customers = service.GetCustomersByNameAsync("JA").Result;

                    // Now check if customer is deleted or not.
                    customers.Count.Should().Be(0);
                }
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync_ValidCustomer_CustomerUpdatedSuccessfully()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CustomerDbContext(options))
            {
                using (var service = new Services.CustomerService.CustomerService(context))
                {
                    // Add single customer
                    var customer = service.AddCustomerAsync(new Customer()
                    {
                        FirstName = "jane",
                        LastName = "dow",
                        BirthDate = "01-01-1990"
                    }).Result;

                    // Check that customer is added or not.
                    var customers = service.GetCustomersByNameAsync("JA").Result;
                    customers.Count.Should().Be(1);

                    // update customers first name
                    customer.FirstName = "updated name";

                    await service.UpdateCustomerAsync(customer.Id, customer);

                    customers = service.GetCustomersByNameAsync("dow").Result;

                    // Now check if customer first name is updated or not.
                    customers.First().FirstName.Should().Be("updated name");
                }
            }
        }
    }
}
