var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FeatGen_Server>("featGen");

builder.Build().Run();