using Project2Api.IDbClient;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddSingleton<IDbClient>(sp => new DbClient(sp.GetService<IConfiguration>()));
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    // app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
