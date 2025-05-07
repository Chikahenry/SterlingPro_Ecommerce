using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public record CartDto(
                Guid Id,
                Guid UserId,
                decimal Total,
                List<CartItemDto> Items);
}
