using CurrencyDAL.EF;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;
using CurrencyAPI.DAL.Repositories;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.CurrencyBLL.Server;
using Currency.BLL.AutoMapper;
using Microsoft.EntityFrameworkCore;
using CurrencyAPI.Extentions;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add builder to the container.

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddControllers();

//DB context


builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositories
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
//Services
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // You can now use 'context' here
DbInitializer.Initialize(context);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
