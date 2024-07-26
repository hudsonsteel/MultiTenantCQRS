using Microsoft.AspNetCore.Mvc;
using MultiTenantCQRS.Read.Domain.Dtos;
using MultiTenantCQRS.Read.Domain.Interfaces.Repositories;
using MultiTenantCQRS.Read.SqlServer.Services;
using Newtonsoft.Json;

namespace MultiTenantCQRS.ReadApi.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController(ServiceFactoryRepository serviceFactoryRepository) : ControllerBase
    {
        private readonly ServiceFactoryRepository _serviceFactoryRepository = serviceFactoryRepository;

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>The order corresponding to the ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (id == default)
            {
                return BadRequest("Invalid order request");
            }

            var _repository = _serviceFactoryRepository.GetInstanceRepository<ICustomerOrdersRepository>();

            var order = await _repository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var customerOrders = JsonConvert.DeserializeObject<CustomerOrders>(order.Json);

            return Ok(customerOrders);
        }
    }
}
