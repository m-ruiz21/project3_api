using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    public static class Inventory
    {
        public static Error NotFound => Error.NotFound(
            code: "Inventory.NotFound", 
            description: "Inventory not found"
        );

        public static Error UnexpectedError => Error.Unexpected(
            code: "Inventory.UnexpectedError", 
            description: "Unexpected error occurred when getting inventory"
        );

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "Inventory.DbError", 
            description: "Error occurred when getting inventory from database"
        );
        public static Error InvalidInventoryItem => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "Inventory.InvalidInventoryItem", 
            description: "Given inventory item is invalid"
        );
    }
}