using Project2Api.DbTools;
using Project2Api.Services.Orders;
using Project2Api.Services.MenuItems;
using System.Data;
using Npgsql;
using Project2Api.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

    // connect dapper DbConnection services 
    builder.Services.AddTransient<IDbConnection>(
        (sp) => 
            new NpgsqlConnection(builder.Configuration.GetValue<string>("PostgreSQL:ConnectionString"))
        );
    
    // repositories
    builder.Services.AddSingleton<IDbClient, DbClient>();
    builder.Services.AddSingleton<IMenuItemRepository, MenuItemRepository>(); 
    builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();

    // services
    builder.Services.AddSingleton<IOrdersService, OrdersService>();
    builder.Services.AddSingleton<IMenuItemService, MenuItemService>();     
    
    // controllers 
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
