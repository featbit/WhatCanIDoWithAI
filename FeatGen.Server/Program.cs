using FeatBit.Sdk.Server;
using FeatGen.FeatureFlag;
using FeatGen.Models;
using FeatGen.OpenAI;
using FeatGen.Server.Components;
using FeatGen.Server.Services;
using FeatGen.ReportGenerator;
using Microsoft.EntityFrameworkCore;

// Set the OpenAI.Experimental.EnableOpenTelemetry context switch
AppContext.SetSwitch("OpenAI.Experimental.EnableOpenTelemetry", true);
// Set the OPENAI_EXPERIMENTAL_ENABLE_OPEN_TELEMETRY environment variable
//Environment.SetEnvironmentVariable("OPENAI_EXPERIMENTAL_ENABLE_OPEN_TELEMETRY", "true");
//Environment.SetEnvironmentVariable("OTEL_LOG_LEVEL", "debug");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddDbContext<FeatGenDbContext>(options =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"), op => op.UseVector())
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddTransient<IKeywordVectorSearchService, KeywordVectorSearchService>();

builder.AddServiceDefaults();
builder.AddOpenAIServices();
builder.AddSpecificationGenServices();
builder.AddFeatureFlagServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000");
        //policyBuilder.WithOrigins("*");
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
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

// Change this line to use the specific policy
app.UseCors("AllowSpecificOrigin");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FeatGen.Client._Imports).Assembly);

// Make controllers respect the CORS policy
app.MapControllers().RequireCors("AllowSpecificOrigin");

app.MapGet("/fbclienttest", (IFbClient fbClient) =>
{
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
