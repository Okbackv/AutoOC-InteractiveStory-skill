namespace InteractiveStory;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var jsonPath = FindStoryJson();
        StoryData data;
        try
        {
            data = new StoryData(jsonPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"无法加载故事数据: {ex.Message}", "启动失败",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var state = new GameState();
        var engine = new StoryEngine(data, state);

        Application.Run(new MainForm(engine));
    }

    static string FindStoryJson()
    {
        string[] candidates =
        {
            "story_nodes.json",
            Path.Combine("..", "04_故事结构设计", "story_nodes.json"),
            Path.Combine("..", "..", "..", "..", "04_故事结构设计", "story_nodes.json"),
        };

        foreach (var path in candidates)
            if (File.Exists(path)) return path;

        return "story_nodes.json";
    }
}
