using CurrencyDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyDAL.EF;

class ApplicationDbContext : DbContext
{
    public DbSet<CurrencyEntities> Currency { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public ApplicationDbContext() { }

}

