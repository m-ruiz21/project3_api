using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    /// <summary>
    /// Cutlery errors
    /// </summary>
    public static class Cutlery 
    {
        public static Error NotFound => Error.NotFound(
            code: "Cutlery.NotFound", 
            description: "Cutlery Item not found"
        );

        public static Error UnexpectedError => Error.Unexpected(
            code: "Cutlery.UnexpectedError", 
            description: "Unexpected error occurred when getting cutlery item"
        );

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "Cutlery.DbError", 
            description: "Error occurred when getting cutlery item from database"
        );

        public static Error InvalidCutlery => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "Cutlery.InvalidMenuItem", 
            description: "Given cutlery item is invalid"
        );
    }
}