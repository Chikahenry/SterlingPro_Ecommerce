using ECommerce.Application.Carts;
using ECommerce.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart([FromQuery] Guid userId)
        {
            var cart = await _mediator.Send(new GetCart.Query(userId));
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartDto>> AddItem(AddItemToCart.Command command)
        {
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<CartDto>> UpdateItem(Guid id, UpdateCartItem.Command command)
        {
            if (id != command.ItemId)
            {
                return BadRequest();
            }

            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult<CartDto>> RemoveItem(Guid id, [FromQuery] Guid userId)
        {
            var cart = await _mediator.Send(new RemoveItemFromCart.Command(userId, id));
            return Ok(cart);
        }
    }
}
