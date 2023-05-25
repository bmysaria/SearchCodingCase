using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using SimonVossSearch.Core;
using SimonVossSearch.Core.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string path = File.ReadAllText("./data.json");
var data = JsonConvert.DeserializeObject<DataFile>(path);

var search = new Search(data, "wc");
search.Execute();

//app.MapGet("/", () => "Hello World!");
app.Run();