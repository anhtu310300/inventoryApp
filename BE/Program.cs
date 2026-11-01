using FluentValidation;
using Inventory.Api.Data;
using Inventory.Api.Domain;
using Inventory.Api.Mappings;
using Inventory.Api.Repositories;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services;
using Inventory.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "The database connection string was not found.");

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<
    IEmployeeRepository,
    EmployeeRepository>();

builder.Services.AddScoped<
    IEmployeeService,
    EmployeeService>();

builder.Services.AddScoped<
    IAuthRepository,
    AuthRepository>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    ICategoryRepository,
    CategoryRepository>();

builder.Services.AddScoped<
    ICategoryService,
    CategoryService>();

builder.Services.AddScoped<
    ISupplierRepository,
    SupplierRepository>();

builder.Services.AddScoped<
    ISupplierService,
    SupplierService>();

builder.Services.AddScoped<
    IWarehouseRepository,
    WarehouseRepository>();

builder.Services.AddScoped<
    IWarehouseService,
    WarehouseService>();

builder.Services.AddScoped<
    ICustomerRepository,
    CustomerRepository>();

builder.Services.AddScoped<
    ICustomerService,
    CustomerService>();

builder.Services.AddScoped<
    IPasswordHasher<Employee>,
    PasswordHasher<Employee>>();

builder.Services.AddAutoMapper(
    configuration => { },
    typeof(EmployeeProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
