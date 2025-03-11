using Instagram.Dal;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Instagram.Server.Configurations;

public static class DataBaseConfiguration
{
    public static void ConfigureDataBase (this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DataBaseConnection");
        builder.Services.AddDbContext<MainContext>(options => options.UseSqlServer(connectionString));
    } 
}
