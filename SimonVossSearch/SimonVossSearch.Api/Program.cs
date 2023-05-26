using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISearchService, SearchService>();


builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();