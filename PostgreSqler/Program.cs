// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PostgreSqler.Models;


await using var ctx = new FeatBitContext(new DbContextOptionsBuilder<FeatBitContext>().Options);

await ctx.Database.EnsureCreatedAsync();
var connected = await ctx.Database.CanConnectAsync();

if (connected)
{
    Console.WriteLine("Connected to database");
    // await ctx.Database.MigrateAsync();

    ctx.TestsTable.Add(new Tests { Testf = "test" });
    await ctx.SaveChangesAsync();

    await ctx.TestsTable.LoadAsync();
    var events = ctx.TestsTable.Local.ToObservableCollection();
    Console.WriteLine(events.Count);
}
else
{
    Console.WriteLine("Failed to connect to database");
}