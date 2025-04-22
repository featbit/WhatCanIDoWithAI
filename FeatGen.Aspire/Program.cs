var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FeatGen_Server>("featGen", configure: static project =>
{
    project.ExcludeLaunchProfile = false;
    project.ExcludeKestrelEndpoints = false;
});
       //.WithHttpEndpoint(port: 5178);

builder.Build().Run();