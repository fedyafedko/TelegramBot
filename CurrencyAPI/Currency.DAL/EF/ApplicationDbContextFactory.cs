using CurrencyDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CurrencyDAL.EF;
class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    {
        var connectionString = "Server=DESKTOP-A4KSM6M;Database=ConvertCurrency;Trusted_Connection=True;TrustServerCertificate=True";
        var optionsBuilber = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilber.UseSqlServer(connectionString);
        return new ApplicationDbContext(optionsBuilber.Options);
    }
}

