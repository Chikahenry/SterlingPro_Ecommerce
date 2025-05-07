using ECommerce.Application.DTOs;
using ECommerce.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products
{
    public static class GetProducts
    {
        public record Query() : IRequest<List<ProductDto>>;

        public class Handler : IRequestHandler<Query, List<ProductDto>>
        {
            private readonly ECommerceDbContext _context;

            public Handler(ECommerceDbContext context)
            {
                _context = context;
            }

            public async Task<List<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Products
                    .Select(p => new ProductDto(
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Price,
                        p.StockQuantity,
                        p.ImageUrl))
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
