using CurrencyDAL.EF;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAPI.Extentions;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate();

        // Add Seed Data...
    }
}