using KnowledgeBase.Server.Components;
using FeatBit.Sdk.Server.DependencyInjection;
using FeatBit.Sdk.Server;
using KnowledgeBase.Models;
using Microsoft.EntityFrameworkCore;
using KnowledgeBase.Server.Services;
using KnowledgeBase.Server.FeatureFlag;
using KnowledgeBase.OpenAI;
using KnowledgeBase.SpecGenerator;

// Set the OpenAI.Experimental.EnableOpenTelemetry context switch
AppContext.SetSwitch("OpenAI.Experimental.EnableOpenTelemetry", true);

// Set the OPENAI_EXPERIMENTAL_ENABLE_OPEN_TELEMETRY environment variable
Environment.SetEnvironmentVariable("OPENAI_EXPERIMENTAL_ENABLE_OPEN_TELEMETRY", "true");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddDbContext<KnowledgeBaseDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"), op =>op.UseVector())
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddTransient<IKeywordVectorSearchService, KeywordVectorSearchService>();

builder.AddServiceDefaults();
builder.AddOpenAIServices();
builder.AddSpecificationGenServices();

// Add FeatBit .NET Server SDK
builder.Services.AddFeatBit(options =>
{
    var featBitSection = builder.Configuration.GetSection("FeatBit");
    options.EnvSecret = featBitSection["EnvSecret"];
    options.StreamingUri = new Uri(featBitSection["StreamingUri"] ?? "");
    options.EventUri = new Uri(featBitSection["EventUri"] ?? "");
    options.StartWaitTime = TimeSpan.FromSeconds(3);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(KnowledgeBase.Client._Imports).Assembly);

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

app.MapHub<FeatureFlagHub>("/featureflaghub");

app.Run();
