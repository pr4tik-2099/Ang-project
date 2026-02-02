using Ang_app.Data;
using Ang_app.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
//using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var jwt = builder.Configuration.GetSection("JWTSettings");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<Ang_app.Data.DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")),ServiceLifetime.Transient);
builder.Services.AddIdentity<AppUser,IdentityRole>()
.AddEntityFrameworkStores<Ang_app.Data.DbContext>()
.AddDefaultTokenProviders();
    
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => 
{
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = jwt["Audience"],
        ValidIssuer = jwt["Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.GetSection("Key").Value!))
    };
}
);


builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    //options.AllowCredentials();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
