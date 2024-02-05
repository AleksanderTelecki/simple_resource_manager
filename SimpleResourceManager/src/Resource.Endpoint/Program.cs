using Resource.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Resource.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Resource.Domain.Options;
using Resource.Domain.Repositories;
using Resource.Endpoint.Abstract;
using Resource.Endpoint.Options;
using Resource.Endpoint.Services;

var builder = WebApplication.CreateBuilder(args);

// Mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<HandlerMarker>());

// DbContext
builder.Services.AddDbContext<ResourceDomainDataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Repositories
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();


// Identity
builder.Services.AddIdentity<ReservationHolder, IdentityRole>()
    .AddEntityFrameworkStores<ResourceDomainDataContext>()
    .AddDefaultTokenProviders();

// JWT Configuration
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    // Adding Jwt Bearer  
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });


// JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<ReservationOptions>(builder.Configuration.GetSection("Reservation"));

// Auth Service
builder.Services.AddTransient<IAuthService, AuthService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
