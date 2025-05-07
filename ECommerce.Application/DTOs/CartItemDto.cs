using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public record CartItemDto(
                Guid Id,
                Guid ProductId,
                string ProductName,
                decimal ProductPrice,
                int Quantity);
}
