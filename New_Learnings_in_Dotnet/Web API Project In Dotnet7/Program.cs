using Microsoft.EntityFrameworkCore;
using WebAPIProject_In_Dotnet_7.Data;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // adding dbcontext file and attaching connectionstring with it.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Injecting dependencies directly into Services
builder.Services.AddScoped<ICharacterService, CharacterService>();

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