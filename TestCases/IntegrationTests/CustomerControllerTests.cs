using CustomerApi.Contracts.Models;
using CustomerApi.Contracts.Models.Common;
using CustomerApi;
using FluentAssertions;
using CustomerApi.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CustomerControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/api/customers")]
        public async Task Get_EndpointsReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Theory]
        [InlineData("/api/customers")]
        public async Task AddCustomerAsync_CustomerSavedSuccessfully(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var request = new Customer()
            {
                FirstName = "Jane",
                LastName = "Dow",
                BirthDate = "4/7/1990"
            };

            // Act
            var response = await client.PostAsJsonAsync(url, request);
            var result = await response.Content.ReadAsJsonAsync<ApiSuccessResponse<Customer>>();

            // Assert
            result.Data.Should().NotBeNull();
        }

        [Theory]
        [InlineData("/api/customers")]
        public async Task UpdateCustomerAsync_CustomerUpdatedSuccessfully(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var request = new Customer()
            {
                FirstName = "Jane",
                LastName = "Dow",
                BirthDate = "4/7/1990"
            };

            // First add new customer.
            var response = await client.PostAsJsonAsync(url, request);

            var result = await response.Content.ReadAsJsonAsync<ApiSuccessResponse<Customer>>();

            // update first name
            result.Data.FirstName = "updated name";

            // call update api
            response = await client.PutAsJsonAsync($"{url}/{result.Data.Id}", result.Data);
            result = await response.Content.ReadAsJsonAsync<ApiSuccessResponse<Customer>>();

            // Assert
            result.Data.FirstName.Should().Be("updated name");
        }

        [Theory]
        [InlineData("/api/customers")]
        public async Task DeleteCustomerAsync_CustomerDeletedSuccessfully(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var request = new Customer()
            {
                FirstName = "Jane",
                LastName = "Dow",
                BirthDate = "4/7/1990"
            };

            // First add new customer.
            var response = await client.PostAsJsonAsync(url, request);

            var result = await response.Content.ReadAsJsonAsync<ApiSuccessResponse<Customer>>();

            // call update api
            response = await client.DeleteAsync($"{url}/{result.Data.Id}");

            // Assert
            response.StatusCode.Should().Be(204);
        }
    }
}
