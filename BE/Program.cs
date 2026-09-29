using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Inventory.Api.Data;
using Inventory.Api.Repositories;
using Inventory.Api.Repositories.Interfaces;
using Inventory.Api.Services;
using Inventory.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found."
    );

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();
builder.Services.AddScoped<ITokenService, TokenService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is not configured."
    );

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)
                ),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (args.Length == 3 && args[0] == "--reset-local-password")
{
    using var resetScope = app.Services.CreateScope();
    var resetDb = resetScope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    var resetHasher = resetScope.ServiceProvider
        .GetRequiredService<IPasswordHasher<User>>();
    var resetEmail = args[1].Trim();
    var resetUser = await resetDb.Users
        .SingleOrDefaultAsync(user => user.Email == resetEmail);

    if (resetUser == null)
    {
        Console.WriteLine($"User not found: {resetEmail}");
        return;
    }

    resetUser.PasswordHash = resetHasher.HashPassword(resetUser, args[2]);
    await resetDb.SaveChangesAsync();

    var verificationResult = resetHasher.VerifyHashedPassword(
        resetUser,
        resetUser.PasswordHash,
        args[2]
    );

    Console.WriteLine(
        verificationResult == PasswordVerificationResult.Failed
            ? "Password reset verification failed."
            : $"Password reset successfully for {resetEmail}."
    );
    return;
}

// TEST MYSQL CONNECTION
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    var canConnect = await db.Database.CanConnectAsync();

    Console.WriteLine(
        canConnect
            ? "MYSQL CONNECTED SUCCESSFULLY"
            : "MYSQL CONNECTION FAILED"
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
