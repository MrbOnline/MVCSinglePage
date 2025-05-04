
using FinalProject.BackEnd.ApplicationServices.Contracts;
using FinalProject.BackEnd.ApplicationServices.Services;
using FinalProject.BackEnd.Models;
using FinalProject.BackEnd.Models.Services.Contracts;
using FinalProject.BackEnd.Models.Services.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Database Context
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(connectionString));

// Dependency Injection
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();


// Swagger Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
