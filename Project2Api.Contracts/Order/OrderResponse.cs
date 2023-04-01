using System;
using System.Collections.Generic;

namespace Project2Api.Contracts.Order
{
    public record OrderResponse(
        Guid Id,
        DateTime dateTime,
        List<string> Items,
        float Price
    );
}
