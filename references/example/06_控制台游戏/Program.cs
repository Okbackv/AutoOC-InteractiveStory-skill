using System.Text;

namespace InteractiveStory;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "星落之夜的抉择";

        var jsonPath = FindStoryJson();
        StoryData data;
        try
        {
            data = new StoryData(jsonPath);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"无法加载故事数据: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey(true);
            return;
        }

        ShowTitleScreen(data);

        var state = new GameState();
        var engine = new StoryEngine(data, state);

        RunGameLoop(engine);
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
        {
            if (File.Exists(path))
                return path;
        }

        return "story_nodes.json";
    }

    static void SafeClear()
    {
        try { Console.Clear(); }
        catch (IOException)
        {
            // Running in a non-console environment (piped/redirected) — skip clear
        }
    }

    static void ShowTitleScreen(StoryData data)
    {
        SafeClear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  ╔══════════════════════════════════════════╗
  ║                                          ║
  ║          ✦  星 落 之 夜  ✦              ║
  ║          抉      择                      ║
  ║                                          ║
  ║      — 星落降临，命运的分岔 —             ║
  ║                                          ║
  ╚══════════════════════════════════════════╝
");
        Console.ResetColor();
        Console.WriteLine($"       作者: {data.Author}  |  版本: {data.Version}");
        Console.WriteLine();
        Console.WriteLine("  [输入 save/load/quit 可在任意选择时存档/读档/退出]");
        Console.WriteLine();
        Console.WriteLine("          按任意键开始游戏...");
        Console.ReadKey(true);
    }

    static void RunGameLoop(StoryEngine engine)
    {
        var running = true;
        var showHistory = false;

        while (running)
        {
            var node = engine.CurrentNode;

            SafeClear();

            // Header
            DrawHeader(engine, node);

            // Main text
            DisplayText(node.Text);

            Console.WriteLine();

            // Check for ending
            if (engine.IsEnding)
            {
                DisplayEnding(engine);
                running = false;
                continue;
            }

            // Display choices
            var choices = engine.GetAvailableChoices();
            if (choices.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[错误] 当前节点没有可用选项。");
                Console.ResetColor();
                Console.WriteLine("按任意键退出...");
                Console.ReadKey(true);
                running = false;
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("你的选择：");
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < choices.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  [{i + 1}] ");
                Console.ResetColor();
                Console.WriteLine(choices[i].Text);
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("请输入选项编号 (或 save/load/quit/history): ");
            Console.ResetColor();

            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (input)
            {
                case "save":
                    HandleSave(engine);
                    break;

                case "load":
                    if (HandleLoad(engine))
                        continue;
                    break;

                case "quit":
                    Console.WriteLine("\n确定要退出吗？(y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        running = false;
                    break;

                case "history":
                    showHistory = !showHistory;
                    if (showHistory)
                        ShowHistory(engine);
                    break;

                default:
                    if (int.TryParse(input, out int choiceNum) &&
                        choiceNum >= 1 && choiceNum <= choices.Count)
                    {
                        try
                        {
                            engine.MakeChoice(choiceNum - 1);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\n导航错误: {ex.Message}");
                            Console.ResetColor();
                            Console.WriteLine("按任意键继续...");
                            Console.ReadKey(true);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n无效输入。请输入 1 到 {choices.Count} 之间的数字。");
                        Console.ResetColor();
                        Console.WriteLine("按任意键继续...");
                        Console.ReadKey(true);
                    }
                    break;
            }
        }

        Console.WriteLine("\n\n感谢游玩！按任意键退出...");
        Console.ReadKey(true);
    }

    static void DrawHeader(StoryEngine engine, StoryNode node)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("┌─ ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(engine.Title);
        Console.ResetColor();
        if (node.Title != null)
        {
            Console.Write("  ·  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(node.Title);
            Console.ResetColor();
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(" ──────────────┐");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void DisplayText(string text)
    {
        var paragraphs = text.Split("\n\n");
        foreach (var paragraph in paragraphs)
        {
            var trimmed = paragraph.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            // Word wrap at ~70 chars
            var lines = WrapText(trimmed, 70);
            foreach (var line in lines)
            {
                Console.Write("  ");
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }

    static List<string> WrapText(string text, int maxWidth)
    {
        var lines = new List<string>();
        var currentLine = "";

        foreach (var ch in text)
        {
            if (ch == '\n')
            {
                lines.Add(currentLine);
                currentLine = "";
                continue;
            }

            currentLine += ch;
            if (currentLine.Length >= maxWidth)
            {
                lines.Add(currentLine);
                currentLine = "";
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
            lines.Add(currentLine);

        return lines;
    }

    static void DisplayEnding(StoryEngine engine)
    {
        var endingType = engine.EndingType ?? "unknown";
        var endingTitle = engine.EndingTitle ?? "终幕";

        Console.ForegroundColor = endingType switch
        {
            "good" => ConsoleColor.Green,
            "bad" => ConsoleColor.Red,
            "true" => ConsoleColor.Cyan,
            "secret" => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };

        Console.WriteLine("═══════════════════════════════════════════");
        Console.WriteLine($"  {endingTitle}");
        Console.WriteLine($"  [{GetEndingTypeLabel(endingType)}]");
        Console.WriteLine("═══════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        // Show stats
        var historyCount = engine.State.History.Count;
        var choicesCount = engine.State.ChoicesMade.Count;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  经过 {historyCount} 个场景，做出了 {choicesCount} 次选择。");
        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("  [N] 重新开始  |  [L] 读档  |  [Q] 退出");

        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.L:
                HandleLoad(engine);
                break;
        }
    }

    static string GetEndingTypeLabel(string type) => type switch
    {
        "good" => "好结局",
        "bad" => "坏结局",
        "neutral" => "普通结局",
        "true" => "真结局",
        "secret" => "隐藏结局",
        _ => type
    };

    static void HandleSave(StoryEngine engine)
    {
        Console.WriteLine();
        var saves = SaveManager.ListSaves();
        Console.Write("请输入存档名称 (直接回车使用自动命名): ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(name))
            name = $"auto_{DateTime.Now:yyyyMMdd_HHmmss}";

        SaveManager.Save(engine.State, name);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ 已保存至: {name}");
        Console.ResetColor();
        Console.WriteLine("按任意键继续...");
        Console.ReadKey(true);
    }

    static bool HandleLoad(StoryEngine engine)
    {
        var saves = SaveManager.ListSaves();
        if (saves.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n没有找到存档文件。");
            Console.ResetColor();
            Console.WriteLine("按任意键继续...");
            Console.ReadKey(true);
            return false;
        }

        Console.WriteLine("\n可用的存档:");
        for (int i = 0; i < saves.Count; i++)
            Console.WriteLine($"  [{i + 1}] {saves[i]}");

        Console.Write("\n请选择存档编号 (或输入 0 取消): ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= saves.Count)
        {
            var loaded = SaveManager.Load(saves[idx - 1]);
            if (loaded != null)
            {
                engine.State.CurrentNodeId = loaded.CurrentNodeId;
                engine.State.Flags = loaded.Flags;
                engine.State.History = loaded.History;
                engine.State.ChoicesMade = loaded.ChoicesMade;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✓ 已从 '{saves[idx - 1]}' 读取存档。");
                Console.ResetColor();
                Console.WriteLine("按任意键继续...");
                Console.ReadKey(true);
                return true;
            }
        }

        return false;
    }

    static void ShowHistory(StoryEngine engine)
    {
        SafeClear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("═══ 已走过的路径 ═══");
        Console.ResetColor();
        Console.WriteLine();

        foreach (var nodeId in engine.State.History)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  → ");
            Console.ResetColor();
            Console.WriteLine(nodeId);
        }

        Console.Write("  → ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{engine.CurrentNode.Id} ◁ 当前");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("按任意键返回...");
        Console.ReadKey(true);
    }
}
