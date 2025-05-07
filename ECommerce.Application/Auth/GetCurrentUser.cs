using ECommerce.Application.DTOs;
using ECommerce.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Auth
{
    public static class GetCurrentUser
    {
        public record Query(ClaimsPrincipal User) : IRequest<UserDto>;

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly ECommerceDbContext _context;

            public Handler(ECommerceDbContext context)
            {
                _context = context;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    throw new Exception("User not authenticated");
                }

                var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return new UserDto(user.Id, user.Username);
            }
        }
    }
}
