using System;
using System.Collections.Generic;

namespace Project2Api.Contracts.Order
{
    public record OrderRequest(
        List<string> Items,
        int Price
    );
}
