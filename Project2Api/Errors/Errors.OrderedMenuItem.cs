using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    /// <summary>
    /// Menu Item errors
    /// </summary>
    public static class OrderedMenuItem 
    {
        public static Error NotFound => Error.NotFound(
            code: "OrderedMenuItem.NotFound", 
            description: "Ordered Menu Item not found"
        );

        public static Error UnexpectedError => Error.Unexpected(
            code: "OrderedMenuItem.UnexpectedError", 
            description: "Unexpected error occurred when getting ordered menu item"
        );

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "OrderedMenuItem.DbError", 
            description: "Error occurred when getting ordered menu item from database"
        );

        public static Error InvalidMenuItem => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "OrderedMenuItem.InvalidOrderedMenuItem", 
            description: "Given ordered menu item is invalid"
        );
    }
}