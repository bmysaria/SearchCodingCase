using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISearchService, SearchService>();


builder.Services.AddControllers();
builder.Services.AddCors();

var app = builder.Build();

app.MapControllers();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
app.Run();