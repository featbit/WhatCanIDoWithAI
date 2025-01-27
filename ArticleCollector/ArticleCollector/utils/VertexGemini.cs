using Mscc.GenerativeAI;

/// <summary>
/// https://github.com/mscraftsman/generative-ai
/// </summary>
public class VertexGemini
{
    private readonly string _projectId;
    private readonly string _model;
    private readonly string _region;
    private readonly string _accessToken;

    public VertexGemini(string projectId, string model, string accessToken)
    {
        _projectId = projectId;
        _region = "";
        _model = model;
        _accessToken = accessToken;
    }

    public async Task<string?> Generate(string prompt)
    {
        var vertex = new VertexAI(_projectId);
        var model = vertex.GenerativeModel(_model);
        model.UseGrounding = true;
        model.AccessToken = _accessToken;

        var response = await model.GenerateContent(prompt);
        return response.Text;
    }
}