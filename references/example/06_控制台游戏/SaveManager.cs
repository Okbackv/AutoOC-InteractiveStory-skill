using System.Text.Json;

namespace InteractiveStory;

public static class SaveManager
{
    private static readonly string SaveDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "InteractiveStory", "saves");

    public static void Save(GameState state, string slotName)
    {
        Directory.CreateDirectory(SaveDir);
        var path = GetSavePath(slotName);

        var json = JsonSerializer.Serialize(state, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        File.WriteAllText(path, json);
    }

    public static GameState? Load(string slotName)
    {
        var path = GetSavePath(slotName);
        if (!File.Exists(path)) return null;

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GameState>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static List<string> ListSaves()
    {
        if (!Directory.Exists(SaveDir))
            return new List<string>();

        return Directory.GetFiles(SaveDir, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .OrderBy(f => f)
            .ToList()!;
    }

    public static bool DeleteSave(string slotName)
    {
        var path = GetSavePath(slotName);
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }

    private static string GetSavePath(string slotName)
    {
        return Path.Combine(SaveDir, $"{slotName}.json");
    }
}
