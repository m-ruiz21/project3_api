using System;
using System.Collections.Generic;

namespace Project2Api.Contracts.Order
{
    public record OrderResponse(
        Guid Id,
        List<string> Items,
        int Price
    );
}
