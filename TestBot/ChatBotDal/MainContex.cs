using ChatBotDal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

namespace ChatBotDal;

public class MainContex : DbContext
{ 
    public DbSet<Users> Users { get; set; }
    //public MainContex(DbContextOptions<MainContex> options)
    //    : base(options);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         
    }
}    