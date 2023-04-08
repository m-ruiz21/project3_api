using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    /// <summary>
    /// Menu Item errors
    /// </summary>
    public static class MenuItem 
    {
        public static Error NotFound => Error.NotFound(
            code: "MenuItem.NotFound", 
            description: "Menu Item not found"
        );

        public static Error UnexpectedError => Error.Unexpected(
            code: "MenuItem.UnexpectedError", 
            description: "Unexpected error occurred when getting menu item"
        );

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "MenuItem.DbError", 
            description: "Error occurred when getting menu item from database"
        );

        public static Error InvalidMenuItem => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "MenuItem.InvalidMenuItem", 
            description: "Given menu item is invalid"
        );
    }
}