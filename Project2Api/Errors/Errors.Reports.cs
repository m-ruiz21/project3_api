using ErrorOr;

namespace Project2Api.ServiceErrors;

public static partial class Errors
{
    /// <summary>
    /// Cutlery errors
    /// </summary>
    public static class Reports 
    {
        public static Error UnexpectedError => Error.Unexpected(
            code: "Reports.UnexpectedError", 
            description: "Unexpected error occurred when getting reports"
        );

        public static Error DbError => Error.Custom(
            type: (int)CustomErrorType.Database, 
            code: "Reports.DbError", 
            description: "Error occurred when getting report"
        );

        public static Error InvalidCutlery => Error.Custom(
            type: (int)CustomErrorType.InvalidParams,
            code: "Reports.InvalidReportRequest", 
            description: "Given report request is invalid"
        );
    }
}