using Web_Shop_API.IRepositories;
using Web_Shop_API.Repositories;
using Web_Shop_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<IProductRepository>(repository => new ProductRepository(connectionString));
builder.Services.AddScoped<ProductService>();

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
