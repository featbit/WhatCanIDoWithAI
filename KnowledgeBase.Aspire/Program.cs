var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.KnowledgeBase_Server>("knowledgeBase");

builder.Build().Run();