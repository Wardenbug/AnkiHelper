namespace AnkiHelper.Core.Generation;

public class PromptTemplate
{
    private readonly string _template;

    public PromptTemplate(string filePath) =>
        _template = File.ReadAllText(filePath);

    public string Build(Dictionary<string, string> values)
    {
        var result = _template;
        foreach (var (key, value) in values)
            result = result.Replace($"{{{{{key}}}}}", value);
        return result;
    }
}