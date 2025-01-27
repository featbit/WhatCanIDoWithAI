using ArticleCollector.utils;
using Mscc.GenerativeAI;


await PuppyAgentSBlockBuilder.Build();

var accessToken = "ya29.a0AXeO80RTDjuGrYYXVXMO-Tojbi68MQb6yagDrWip5_EVFG26i8a-2VZVHbaluceOjmF5qbTdRqNVvEhIXqFKF6eRJUxootwOd1nWnBK__yJ6UNkIcL9MC02vb7ipnV6BkSQ8ewjfOhDm2FkIZEeLp0jCvIX6gusePapDFTJaaCgYKASESARMSFQHGX2MixcNmm-4hAiV8blcbKbIG5w0175";
var geminiAgent = new VertexGemini(
    projectId: "project-444506", model: Model.Gemini15Pro002, accessToken);


var result = await geminiAgent.Generate("list all images link from https://docs.getunleash.io/topics/feature-flags/feature-flag-best-practices");
Console.Write(result);