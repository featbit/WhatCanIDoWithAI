using KnowledgeBase.BlazorWebApp.Client.Pages;
using KnowledgeBase.BlazorWebApp.Components;
using FeatBit.Sdk.Server.DependencyInjection; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add FeatBit .NET Server SDK
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
    .AddAdditionalAssemblies(typeof(KnowledgeBase.BlazorWebApp.Client._Imports).Assembly);

app.Run();
