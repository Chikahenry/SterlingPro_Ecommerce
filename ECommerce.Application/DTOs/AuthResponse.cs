using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public record AuthResponse(
                Guid UserId,
                string Username,
                string Token);
}
