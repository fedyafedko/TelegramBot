using CurrencyDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL.EF;

public class ApplicationDbContext : DbContext
{
    public DbSet<CurrencyEntities> Currencies { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //public ApplicationDbContext()
    //{
    //    Database.EnsureCreated();
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(
    //        "Server=DESKTOP-A4KSM6M;Database=ConvertCurrency;Trusted_Connection=True;TrustServerCertificate=True");
    //}

}

