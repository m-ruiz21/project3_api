using ErrorOr;

namespace Project2Api.ServiceErrors;

public static class Errors
{
    public static class Orders
    {
        public static Error NotFound => Error.NotFound(
            code: "Order.NotFound", 
            description: "Order not found"
        );

        public static Error UnexpectedError => Error.Unexpected(
            code: "Order.UnexpectedError", 
            description: "Unexpected error occurred when getting order"
        );
    }
}