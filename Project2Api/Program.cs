using Project2Api.Services.Orders;
using Project2Api.Services.MenuItems;
using System.Data;
using Npgsql;
using Project2Api.Repositories;
using Project2Api.Services.Inventory;
using Project2Api.Services.CutleryItems;
using Project2Api.Services.Reports;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

    // connect dapper DbConnection services 
    builder.Services.AddTransient<IDbConnection>(
        (sp) => 
            new NpgsqlConnection(builder.Configuration.GetValue<string>("PostgreSQL:ConnectionString"))
        );
    
    // repositories
    builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>(); 
    builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
    builder.Services.AddScoped<ICutleryRepository, CutleryRepository>();
    builder.Services.AddScoped<IOrderedMenuItemRepository, OrderedMenuItemRepository>();

    // services
    builder.Services.AddScoped<IOrdersService, OrdersService>();
    builder.Services.AddScoped<IMenuItemService, MenuItemService>();   
    builder.Services.AddScoped<IInventoryService, InventoryService>();  
    builder.Services.AddScoped<ICutleryService, CutleryService>();
    builder.Services.AddScoped<IReportsService, ReportsService>();

    // controllers 
    builder.Services.AddControllers();

    // cors (for local testing use only)
    // builder.Services.AddCors(options =>
    // {
    //     options.AddPolicy("MyCorsPolicy", builder =>
    //     {
    //         builder.AllowAnyOrigin()
    //             .AllowAnyMethod()
    //             .AllowAnyHeader();
    //     });
    // });
}

var app = builder.Build();
{
    // for testing use only:
    // app.UseCors("MyCorsPolicy");

    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
