using System.Threading.Tasks;
using CustomerApi.Contracts.Interfaces;
using CustomerApi.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Host.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Find customers by first or last name. 
        /// Customer names starting with search term will be returned.
        /// </summary>
        /// <param name="searchTerm">
        /// If no search term is specified, then no result will be returned.
        /// Use this to find customers whose names start with search term.
        /// </param>
        /// <returns>
        /// Matching customers
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/customers
        ///     
        /// </remarks>
        /// <response code="200">Returns the matching customers</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<IActionResult> GetCustomersByNameAsync([FromQuery]string searchTerm)
        {
            return Ok(await _customerService.GetCustomersByNameAsync(searchTerm));
        }

        /// <summary>
        /// Add the new customer.
        /// </summary>
        /// <param name="customer">
        /// Customer to be added.
        /// </param>
        /// <returns>
        /// Newly added customer with unique id
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/customers
        ///     {
        ///        "id": 0,
        ///        "firstName": "jane",
        ///        "lastName": "dow",
        ///        "birthDate": "4/6/1990"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">added customer with unique id</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<IActionResult> AddCustomerAsync([FromBody] Customer customer)
        {
            return Ok(await _customerService.AddCustomerAsync(customer));
        }

        /// <summary>
        /// Update customer by id.
        /// </summary>
        /// <param name="id">
        /// Unique id of customer to be updated
        /// </param>
        /// <param name="customer">
        /// Customer to be updated with modified data
        /// </param>
        /// <returns>
        /// Updated customer
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/customers/1
        ///     {
        ///        "id": 1,
        ///        "firstName": "john",
        ///        "lastName": "diggle",
        ///        "birthDate": "4/6/1991"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">customer is updated with modified data</response>
        /// <response code="404">customer with given id is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] Customer customer)
        {
            return Ok(await _customerService.UpdateCustomerAsync(id, customer));
        }

        /// <summary>
        /// Delete customer by id.
        /// </summary>
        /// <param name="id">
        /// Unique id of customer to be deleted
        /// </param>
        /// <returns>
        /// Empty result
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/customers/1
        ///
        /// </remarks>
        /// <response code="204">customer is deleted successfully</response>
        /// <response code="404">customer with given id is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return new NoContentResult();
        }
    }
}
