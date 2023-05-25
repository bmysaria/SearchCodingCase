using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string path = File.ReadAllText("./data.json");
var data = JsonConvert.DeserializeObject<DataFile>(path);

//Search.Execute(data, "Head");
var search = new Search(data, "head");
search.Execute();

app.MapGet("/", () => "Hello World!");
app.Run();