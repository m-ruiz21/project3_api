using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    /// <summary>
    /// Order errors
    /// </summary>
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

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "Order.DbError", 
            description: "Error occurred when getting order from database"
        );

        public static Error InvalidOrder => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "Order.InvalidOrder", 
            description: "Given order is invalid"
        );
    }
}