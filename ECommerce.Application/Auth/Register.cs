using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Auth
{
    public static class Register
    {
        public record Command(string Username, string Password) : IRequest<AuthResponse>;

        public class Handler : IRequestHandler<Command, AuthResponse>
        {
            private readonly ECommerceDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ITokenService _tokenService;

            public Handler(
                ECommerceDbContext context,
                IPasswordHasher passwordHasher,
                ITokenService tokenService)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _tokenService = tokenService;
            }

            public async Task<AuthResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
                {
                    throw new Exception("Username already exists");
                }

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = _passwordHasher.HashPassword(request.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                var token = _tokenService.GenerateToken(user);

                return new AuthResponse(
                    user.Id,
                    user.Username,
                    token);
            }
        }
    }
}
