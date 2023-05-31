using CurrencyDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL.EF;

public class ApplicationDbContext : DbContext
{
    public DbSet<CurrencyEntities> Currencies { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}

