// See https://aka.ms/new-console-template for more information
using System.Dynamic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var json = new
{
    uuid = "uuid 666",
    //distinct_id = "distinct_id 666",
    env_id = "env_id 666",
    evt = "evt 666",
    properties = new
    {
        tag_1 = ""
    },
    timestamp = 123415152
};
string jString = JsonSerializer.Serialize(json);


object? insight;
(new InsightService()).TryParse(jString, out insight);
dynamic obj = insight;
Console.WriteLine(obj);


//string jString = "{\"uuid\":\"4fd82549-d1e8-4803-bd19-ccc237b1b658\",\"distinct_id\":\"093dae72-6b79-4cec-afbf-5319ecf4129a\",\"env_id\":\"99dcd972-5ab0-4d72-bd93-105b39f4e7ce\",\"event\":\"evt 666\",\"properties\":\"{\\u0022tag_1\\u0022:\\u0022tag 1\\u0022}\",\"timestamp\":1740578854000}";

//object? insight;
//insightService.TryParse(jString, out insight);

//await insightService.AddManyAsync(new object[] { insight });

public class InsightService()
{
    public bool TryParse(string json, out object? insight)
    {
        try
        {
            var jsonObject = JsonSerializer.Deserialize<JsonObject>(json);
            if (jsonObject == null)
            {
                insight = null;
            }
            else
            {
                dynamic insightObj = new ExpandoObject();
                insightObj.Uuid = jsonObject["uuid"]?.GetValue<string>();
                insightObj.DistinctId = jsonObject["distinct_id"]?.GetValue<string>();
                insightObj.EnvId = jsonObject["env_id"]?.GetValue<string>();
                insightObj.Event = jsonObject["event"]?.GetValue<string>();
                insightObj.Properties = jsonObject["properties"]?.ToString();
                insightObj.Timestamp = jsonObject["timestamp"]?.GetValue<long>();

                insight = insightObj;
            }
        }
        catch
        {
            insight = null;
        }

        return insight != null;
    }

    public async Task<> AddManyAsync(object[] insights)
    {
        var insertList = new List<object>();
        foreach (var insight in insights)
        {
            dynamic di = insight;
            insertList.Add(new
            {
                Uuid = di.Uuid,
                DistinctId = di.DistinctId,
                EnvId = di.EnvId,
                Event = di.Event,
                Properties = di.Properties,
                Timestamp = di.Timestamp
            });
        };
        foreach(var obj in insertList)
        {
            Console.WriteLine(obj);
        }
    }
}


