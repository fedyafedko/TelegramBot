using CurrencyDAL.EF;
using CurrencyAPI.Currency.DAL.Repositories.Interfaces;
using CurrencyAPI.DAL.Repositories;
using CurrencyAPI.CurrencyBLL.Interfaces;
using CurrencyAPI.CurrencyBLL.Server;
using Currency.BLL.AutoMapper;
;

var builder = WebApplication.CreateBuilder(args);

// Add builder to the container.

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddControllers();

//DB context
builder.Services.AddDbContext<ApplicationDbContext>();
//Repositories
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//Services
builder.Services.AddScoped<ICurrencyService, CurrencyServer>();
//builder.Services.AddScoped<IAuthServer, AuthServer>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
