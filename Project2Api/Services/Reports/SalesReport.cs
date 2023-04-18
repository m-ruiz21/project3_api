namespace Project2Api.Services.Reports
{
    public class SalesReport
    {
        public decimal Sales { get; set; }
        public DateTime Date { get; set; }


        public SalesReport(decimal sales, DateTime dateTime)
        {
            Sales = sales;
            Date = dateTime;
        }
    }
}