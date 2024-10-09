using MySql.Data.MySqlClient;

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
}
