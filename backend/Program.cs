using System.Text;
using backend.Data;
using backend.Services;
using backend.Models;
using backend.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Validate and retrieve JWT configuration values
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
                ?? throw new InvalidOperationException("Jwt:Issuer is missing in configuration.");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
                  ?? throw new InvalidOperationException("Jwt:Audience is missing in configuration.");
var jwtKey = builder.Configuration["Jwt:Key"] 
             ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddScoped<JwtService>();

// Configure SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=doctors.db"));


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Bind CloudinarySettings from appsettings.json
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);

// Add Cloudinary instance
builder.Services.AddSingleton(sp =>
{
    var config = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
    var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new CloudinaryDotNet.Cloudinary(account);
});

var app = builder.Build();

// admin creation support
if (args.Contains("createsuperuser"))
{
    CreateAdminUser(app.Services);
    return;
}

// Middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();

// helper methods
static void CreateAdminUser(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    Console.Write("Enter admin email: ");
    var email = Console.ReadLine()?.Trim();

    Console.Write("Enter password: ");
    var password = ReadPassword();

    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Email and password must not be empty.");
        return;
    }

    if (context.Users.Any(u => u.Email == email))
    {
        Console.WriteLine("An admin with that email already exists.");
        return;
    }

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

    var admin = new Admin
    {
        Email = email,
        Password = hashedPassword
    };

    context.Admins.Add(admin);
    context.SaveChanges();

    Console.WriteLine("Admin user created successfully.");
}

static string ReadPassword()
{
    var password = string.Empty;
    ConsoleKey key;
    do
    {
        var keyInfo = Console.ReadKey(intercept: true);
        key = keyInfo.Key;

        if (key == ConsoleKey.Backspace && password.Length > 0)
        {
            password = password[0..^1];
            Console.Write("\b \b");
        }
        else if (!char.IsControl(keyInfo.KeyChar))
        {
            password += keyInfo.KeyChar;
            Console.Write("*");
        }
    } while (key != ConsoleKey.Enter);

    Console.WriteLine();
    return password;
}

//dotnet run -- createsuperuser