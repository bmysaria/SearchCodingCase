using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Persistence;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;
using SimonVossSearch.Core.Parser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseNpgsql("User ID=postgres;Password=postgrespw;Host=localhost;Port=5432;Database=mydb;"));
builder.Services.AddScoped<IDataFileParser, DataFileParser>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>(); 


builder.Services.AddControllers();
builder.Services.AddCors();

var app = builder.Build();

app.MapControllers();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SearchDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

SeedDatabase();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}