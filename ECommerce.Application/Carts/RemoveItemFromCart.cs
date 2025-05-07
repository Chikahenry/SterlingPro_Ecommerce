using ECommerce.Application.DTOs;
using ECommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Carts
{
    public static class RemoveItemFromCart
    {
        public record Command(Guid UserId, Guid ItemId) : IRequest<CartDto>;

        public class Handler : IRequestHandler<Command, CartDto>
        {
            private readonly ECommerceDbContext _context;

            public Handler(ECommerceDbContext context)
            {
                _context = context;
            }

            public async Task<CartDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

                if (cart == null)
                {
                    throw new Exception("Cart not found");
                }

                var item = cart.Items.FirstOrDefault(i => i.Id == request.ItemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new CartDto(
                    cart.Id,
                    cart.UserId,
                    cart.Total,
                    cart.Items.Select(i => new CartItemDto(
                        i.Id,
                        i.ProductId,
                        i.Product.Name,
                        i.Product.Price,
                        i.Quantity)).ToList());
            }
        }
    }
}
