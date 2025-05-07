using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
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
    public static class AddItemToCart
    {
        public record Command(Guid UserId, Guid ProductId, int Quantity) : IRequest<CartDto>;

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
                    cart = new Cart { UserId = request.UserId };
                    _context.Carts.Add(cart);
                }

                var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = request.ProductId,
                        Quantity = request.Quantity
                    });
                }

                await _context.SaveChangesAsync(cancellationToken);

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
