using System.Text;
using Auth.Core.Interfaces.DomainServices;
using Auth.Core.Interfaces.Integration;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Producers;
using Auth.Web.Interfaces;
using Auth.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Integration.Configuration;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

//CORS
const string policyName = "AllowOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

//DBContext
builder.Services.AddDbContext<AuthContext>(options => { options.UseSqlServer(Config.ConnectionStrings.ShwUsers); });

//API Controllers
builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Build repositories
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

//Build kafka producer
builder.Services.AddScoped<ISyncProducer, KafkaProducer>();

//Build services
builder.Services.AddScoped<IAuthService, AuthService>();

//Build view model services
builder.Services.AddScoped<IViewModelService, ViewModelService>();

//JWT Key
var key = Encoding.UTF8.GetBytes(Config.Authorization.JwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Config.Authorization.Policies.RequireShippingCompanyAdminRole,
        policy => policy.RequireRole(Config.Authorization.Roles.ShippingCompanyAdmin));
    options.AddPolicy(Config.Authorization.Policies.RequireKemiDbUserRole,
        policy => policy.RequireRole(Config.Authorization.Roles.KemiDbUser));
    options.AddPolicy(Config.Authorization.Policies.RequireSuperAdminRole,
        policy => policy.RequireRole(Config.Authorization.Roles.SuperAdmin));
});

//Logging

//Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .Filter.ByExcluding(logEvent =>
        logEvent.Level == LogEventLevel.Warning &&
        logEvent.MessageTemplate.Text.Contains("XML"))
    .Filter.ByExcluding(logEvent =>
        logEvent.Level == LogEventLevel.Warning &&
        logEvent.MessageTemplate.Text.Contains("https"))
    .Filter.ByExcluding(logEvent =>
        logEvent.Level == LogEventLevel.Warning &&
        logEvent.MessageTemplate.Text.Contains("Storing"))
    .ReadFrom.Configuration(ctx.Configuration));

//Configure startup logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

//Startup logging
try
{
    Log.Information("AuthService starting up");
}
catch (Exception ex)
{
    Log.Fatal(ex, "AuthService failed to start up");
}
finally
{
    Log.CloseAndFlush();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(policyName);
app.UseAuthentication();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.MapFallbackToFile("index.html");

app.Run();