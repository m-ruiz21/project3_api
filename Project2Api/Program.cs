using Project2Api.DbTools;
using Project2Api.Services.Orders;
using Project2Api.Services.MenuItems;
using System.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
    builder.Services.AddSingleton<IDbClient, DbClient>();
    builder.Services.AddSingleton<IOrdersService, OrdersService>();
    builder.Services.AddSingleton<IMenuItemService, MenuItemService>();     
    builder.Services.AddControllers();
    builder.Services.AddTransient<IDbConnection>(
        (sp) => 
            new NpgsqlConnection(builder.Configuration.GetValue<string>("PostgreSQL:ConnectionString"))
        );
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
