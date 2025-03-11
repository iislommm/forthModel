using Instagram.Dal.Entities;
using Instagram.Dal.EntityConfigurationss;
using Microsoft.EntityFrameworkCore;

namespace Instagram.Dal;

public class MainContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }


    public MainContext(DbContextOptions<MainContext> options)
        : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseSqlServer("Data Source=localhost\\SQLDEV;User ID=sa;Password=akobirakoone;Initial Catalog=IdentityHub;TrustServerCertificate=True;");
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }

}