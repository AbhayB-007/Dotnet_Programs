using System.Text.Json.Serialization;
using Serilog;
using TaskFlow.API.Extensions;
using TaskFlow.API.Middleware;
using TaskFlow.Application;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure;
using TaskFlow.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog (structured logging) ----------
builder.Host.UseSerilog((ctx, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console());

// ---------- MVC / Controllers ----------
builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ---------- Layered dependency injection ----------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ---------- CORS: allow the React SPA (configured origin) to call this API ----------
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// ---------- Swagger / OpenAPI with JWT support ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- Health checks (useful for Azure App Service "health check path") ----------
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

// ---------- Middleware pipeline ----------
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// ---------- Apply EF Core migrations + seed roles/admin user on startup ----------
await DbSeeder.SeedAsync(app.Services);

app.Run();
