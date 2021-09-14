using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("X-TraceId");
    logging.ResponseHeaders.Add("X-Pagination");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Minimal API in .NET 6", Version = "v1" });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API in .NET 6 v1"));
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.MapGet("/hello", () => new { Hello = "World" });

app.MapGet("/todos", () => Enumerable.Range(1, 3).Select(index => new Todo
{
    Id = index,
    IsComplete = true,
    Title = $"Item-{index}"
}));

app.MapPost("/todos", async (Todo todo) => await Task.FromResult(todo));

app.Run();
