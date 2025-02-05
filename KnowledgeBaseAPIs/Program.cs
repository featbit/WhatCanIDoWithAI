using FeatBit.Sdk.Server.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// add FeatBit service
builder.Services.AddFeatBit(options =>
{
    options.EnvSecret = "dnja-RQ0JEWY7DO7TwGc9gq6T7F-uqb0K6zBegwsPUHw";
    options.StreamingUri = new Uri("wss://app-eval.featbit.co");
    options.EventUri = new Uri("https://app-eval.featbit.co");
    options.StartWaitTime = TimeSpan.FromSeconds(3);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
