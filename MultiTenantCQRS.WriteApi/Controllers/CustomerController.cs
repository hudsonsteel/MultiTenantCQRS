using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantCQRS.Write.Application.Commands.Customer;
using MultiTenantCQRS.WriteApi.Transport.Request.Customer;
using MultiTenantCQRS.WriteApi.Validations.Customer;

namespace MultiTenantCQRS.WriteApi.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerCreateRequest customer)
        {
            var validator = new CreateCustomerCommandValidator().Validate(customer);

            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors.Distinct().Select(e => e.ErrorMessage));
            }

            var command = new CreateCustomerCommand
            {
                Name = customer.Name,
                Email = customer.Email,
                Orders = customer.Orders.Select(o => new CreateCustomerCommand.Order
                {
                    Date = o.Date,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId
                }).ToList()
            };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateRequest customer)
        {
            var validator = new CustomerUpdateRequestValidator().Validate(customer);

            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors.Distinct().Select(e => e.ErrorMessage));
            }

            var command = new UpdateCustomerCommand
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Orders = customer.Orders.Select(o => new UpdateCustomerCommand.Order
                {
                    Id = o.Id,
                    Date = o.Date,
                    TotalAmount = o.TotalAmount,
                }).ToList()
            };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("remove/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }

            var command = new DeleteCustomerCommand(id);

            await _mediator.Send(command);

            return Ok();
        }
    }

}
