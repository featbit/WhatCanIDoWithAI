// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PostgreSqler.Models;
using System.Text.Json;

var predefinedGuids = new List<string>{
    "8f7c89ac-4904-45d5-8a4f-fd5e6177c252",
    "51c886ba-e3b9-4f76-8379-bc8721699fd0",
    "cd78f0cd-232f-4002-a250-d261adf4682b",
    "7e85425c-7296-4f02-8e14-2f3b4dbada0a",
    "1205cf17-ac64-4de0-88d2-4cb6258d4ebb",
    "5dff4f3e-e1b8-4da9-b4c8-8ee4b59633f8",
    "640ca1e0-fdb5-4b25-9a2c-b56bf4c212eb",
    "ee53946a-618e-4c05-b92f-dbee2031553d",
    "1df8b041-6c5f-45f8-aed1-91bb26ee5fc3",
    "0372217f-a9c3-40a4-bf76-4f958f2683fa",
    "27d2a1d7-56ef-4de2-8a85-87492a4d1fe0",
    "04fc3320-3926-41d3-a8dd-1cb942206477",
    "8e1d9ae1-f5a2-4ac3-b83d-34fc998d256f",
    "f5dee4a4-ce8b-46f3-84ca-04d4051dfa41",
    "5eece2d8-84b4-4c77-8fb7-54b21a39928b",
    "4313b3eb-399d-4bbf-bf80-a95c2b42c900",
    "65f877c2-b5bf-4b06-a9d3-04bcb7df2bae",
    "c8c8c65f-16d4-4e18-9337-5a212287e850",
    "81b9a64b-c6c4-4ba5-85de-279be5289a24",
    "328098b8-43cd-40c9-aeb8-29056d0f6161",
    "744408dd-ca1b-4554-ac64-7143d5871b6b",
    "3bb8836c-90a2-4fc0-ad61-8ac7256d6079",
    "16d39ed6-ba89-4c97-aac0-3187bdffb0f2",
    "6c6a7102-6f98-4ff2-a483-69cf437cb732",
    "0e73713a-950e-4bf1-a3d6-ca688c1365c7",
    "63717102-94ea-4964-b696-59bf30c5eff8",
    "c88c5139-9f9f-4c82-9c5b-df004ace60b4",
    "583c045e-3de2-441f-a25d-baf7c9b702f6",
    "cde78397-e82d-40d5-9691-5bc08f85d069",
    "6fddc130-27da-4e70-b3ee-d430bce8a458"
};

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
    DateTime start = DateTime.UtcNow;
    for (int i = 0; i < predefinedGuids.Count; i++)
    {
        Console.WriteLine($"Loop: {i}");
        for (int j = 0; j < 1500; j++)
        {
            ctx.Events.Add(new Event
            {
                Id = new Guid(),
                EnvId = Guid.Parse(predefinedGuids[i]),
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
        //Task.Delay(1000).Wait();
    }
    DateTime end = DateTime.UtcNow;
    var diff = end - start;
    var events = await ctx.Events.CountAsync();
    Console.WriteLine($"events: {events}");
    Console.WriteLine($"diff: {diff.Seconds}");
}
else
{
    Console.WriteLine("Failed to connect to database");
}