// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PostgreSqler.Models;
using System.Text.Json;


await using var ctx = new FeatBitContext(new DbContextOptionsBuilder<FeatBitContext>().Options);

await ctx.Database.EnsureCreatedAsync();
var connected = await ctx.Database.CanConnectAsync();

if (connected)
{
    Console.WriteLine("Connected to database");
    // await ctx.Database.MigrateAsync();

    //ctx.Tests.Add(new Test { Testf = "test" });
    //await ctx.SaveChangesAsync();

    //var tests = await ctx.Tests.CountAsync();
    //Console.WriteLine($"tests: {tests}");

    var envId = new Guid();
    for(int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Loop: {i}");
        for (int j = 0; j < 1000; j++)
        {
            ctx.Events.Add(new Event
            {
                Id = new Guid(),
                EnvId = envId,
                FlagKey = "test-feature-flag-key",
                Data = JsonSerializer.Serialize(new EventData()
                {
                    AccountId = "test-account-id",
                    EnvId = "test-env-id",
                    FeatureFlagKey = "test-feature-flag-key",
                    ProjectId = "test-project-id",
                    Route = "test-route",
                    SendToExperiment = true,
                    Tag0 = "test-tag0",
                    Tag1 = "test-tag1",
                    Tag2 = "test-tag2",
                    Tag3 = "test-tag3",
                    UserKeyId = "test-user-key-id-" + j,
                    UserName = "test-user-name-" + j,
                    VariationId = "test-variation-id"
                }),
                EventType = "test",
                Timestamp = DateTime.UtcNow
            });
        }
        await ctx.SaveChangesAsync();
        Task.Delay(1000).Wait();
    }

    var events = await ctx.Events.CountAsync();
    Console.WriteLine($"events: {events}");
}
else
{
    Console.WriteLine("Failed to connect to database");
}