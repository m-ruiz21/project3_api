using Project2Api.DbTools;
using Project2Api.Services.Orders;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
    builder.Services.AddSingleton<IDbClient, DbClient>();
    builder.Services.AddSingleton<IOrdersService, OrdersService>(); 
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
