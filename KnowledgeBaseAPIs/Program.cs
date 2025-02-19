using FeatBit.Sdk.Server;
using FeatBit.Sdk.Server.DependencyInjection;
using KnowledgeBaseAPIs;

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

builder.Services.AddTransient<IFeatureManager, FeatureManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/fbclienttest", (IFbClient fbClient) => {
    var fbUser = FeatBit.Sdk.Server.Model.FbUser.Builder("123").Build();

    var customReportFlag = fbClient.BoolVariation("custom-report", fbUser, defaultValue: false);
    return customReportFlag switch
    {
        true => "True",
        _ => "False"
    };
});
app.MapGet("/featuremanager", async (IFeatureManager featureManager) => {
    var customReportFlag = await featureManager.IsEnabledAsync("custom-report");
    return customReportFlag switch
    {
        true => "True",
        _ => "False"
    };
});

app.Run();
