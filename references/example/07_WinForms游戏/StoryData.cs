using System.Text.Json;
using System.Text.Json.Serialization;

namespace InteractiveStory;

public class StoryData
{
    private readonly Dictionary<string, StoryNode> _nodes;

    public string Title { get; }
    public string Author { get; }
    public string Version { get; }

    public StoryData(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        var root = JsonSerializer.Deserialize<StoryRoot>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidDataException("无法解析故事数据文件。");

        Title = root.Title;
        Author = root.Author ?? "未知";
        Version = root.Version ?? "1.0";

        _nodes = new Dictionary<string, StoryNode>();
        foreach (var node in root.Nodes)
        {
            _nodes[node.Id] = node;
        }

        if (!_nodes.ContainsKey("start"))
            throw new InvalidDataException("故事数据缺少 'start' 节点。");
    }

    public StoryNode GetNode(string id)
    {
        if (_nodes.TryGetValue(id, out var node))
            return node;
        throw new KeyNotFoundException($"找不到节点: {id}");
    }

    public bool HasNode(string id) => _nodes.ContainsKey(id);
}

public class StoryRoot
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = "未命名故事";

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("nodes")]
    public List<StoryNode> Nodes { get; set; } = new();
}

public class StoryNode
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("choices")]
    public List<Choice>? Choices { get; set; }

    [JsonPropertyName("isEnding")]
    public bool IsEnding { get; set; }

    [JsonPropertyName("endingType")]
    public string? EndingType { get; set; }

    [JsonPropertyName("endingTitle")]
    public string? EndingTitle { get; set; }

    [JsonPropertyName("perspective")]
    public string? Perspective { get; set; }

    [JsonPropertyName("effects")]
    public Dictionary<string, object>? Effects { get; set; }
}

public class Choice
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    [JsonPropertyName("nextNode")]
    public string NextNode { get; set; } = "";

    [JsonPropertyName("condition")]
    public Dictionary<string, object>? Condition { get; set; }

    [JsonPropertyName("tooltip")]
    public string? Tooltip { get; set; }
}
