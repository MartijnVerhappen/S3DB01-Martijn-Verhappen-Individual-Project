using DAL;
using System.Configuration;
using Core.IRepositories;
using Web_Shop_API.Repositories;
using Microsoft.EntityFrameworkCore;
using Core.IServices;
using Logic.Services;

var AllowTauriOrigin = "AllowTauriOrigin";

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowTauriOrigin,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost", "http://127.0.0.1", "http://localhost:1420")
                                .AllowAnyHeader()
                                .AllowAnyMethod();  // Allow specific HTTP methods like GET, POST, etc.
                      });
});

builder.Services.AddDbContext<DBContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Register repositories and services for dependency injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AllowTauriOrigin);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
