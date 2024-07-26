using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantCQRS.Write.Application.Commands.Order;
using MultiTenantCQRS.WriteApi.Transport.Request.Order;
using MultiTenantCQRS.WriteApi.Validations.Order;

namespace MultiTenantCQRS.WriteApi.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var validator = new OrderCreateRequestValidator().Validate(request);

            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors.Distinct().Select(e => e.ErrorMessage));
            }

            var orderCreatedEvent = new CreateOrderCommand
            {
                CustomerId = request.CustomerId,
                TotalAmount = request.TotalAmount,
                OrderDate = DateTime.UtcNow
            };

            var response = await _mediator.Send(orderCreatedEvent);

            return Ok(response);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateOrderRequest order)
        {
            var validator = new UpdateOrderRequestValidator().Validate(order);

            if (!validator.IsValid)
            {
                return BadRequest(validator.Errors.Distinct().Select(e => e.ErrorMessage));
            }

            var command = new UpdateOrderCommand
            {
                Id = order.Id,
                Date = order.Date,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId
            };

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("remove/customer/{customerId}/order/{orderId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder(int customerId, int orderId)
        {
            if (customerId <= 0 || orderId <= 0)
            {
                return BadRequest("Invalid customer ID or order ID.");
            }

            var command = new DeleteOrderCommand(customerId, orderId);

            await _mediator.Send(command);

            return Accepted();
        }
    }
}
