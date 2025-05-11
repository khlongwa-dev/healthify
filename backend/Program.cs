using System.Text;
using backend.Data;
using backend.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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