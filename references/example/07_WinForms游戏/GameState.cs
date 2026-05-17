using System.Text.Json;
using System.Text.Json.Serialization;

namespace InteractiveStory;

public class GameState
{
    [JsonPropertyName("currentNodeId")]
    public string CurrentNodeId { get; set; } = "start";

    [JsonPropertyName("flags")]
    public Dictionary<string, object> Flags { get; set; } = new();

    [JsonPropertyName("history")]
    public List<string> History { get; set; } = new();

    [JsonPropertyName("choicesMade")]
    public List<ChoiceRecord> ChoicesMade { get; set; } = new();

    public T? GetFlag<T>(string key) where T : class
    {
        if (Flags.TryGetValue(key, out var value))
            return value as T;
        return null;
    }

    public bool GetBoolFlag(string key)
    {
        if (Flags.TryGetValue(key, out var value))
        {
            if (value is JsonElement elem && elem.ValueKind == JsonValueKind.True)
                return true;
            if (value is bool b)
                return b;
        }
        return false;
    }

    public int GetIntFlag(string key, int defaultValue = 0)
    {
        if (Flags.TryGetValue(key, out var value))
        {
            if (value is JsonElement elem && elem.ValueKind == JsonValueKind.Number)
                return elem.GetInt32();
            if (value is int i)
                return i;
        }
        return defaultValue;
    }

    public void SetFlag(string key, object value)
    {
        Flags[key] = value;
    }
}

public class ChoiceRecord
{
    [JsonPropertyName("nodeId")]
    public string NodeId { get; set; } = "";

    [JsonPropertyName("choiceIndex")]
    public int ChoiceIndex { get; set; }
}
