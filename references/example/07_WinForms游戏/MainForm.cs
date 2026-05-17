using System.Text;

namespace InteractiveStory;

public class MainForm : Form
{
    private readonly StoryEngine _engine;
    private readonly RichTextBox _textBox;
    private readonly FlowLayoutPanel _choicesPanel;
    private readonly Label _titleLabel;
    private readonly Label _povLabel;
    private readonly StatusStrip _statusStrip;
    private readonly ToolStripStatusLabel _statusLabel;
    private readonly Panel _titleScreen;
    private readonly Panel _endingScreen;

    private bool _darkTheme = true;

    public MainForm(StoryEngine engine)
    {
        _engine = engine;
        Text = engine.Title;
        Size = new Size(860, 680);
        MinimumSize = new Size(600, 400);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Microsoft YaHei", 10);

        // ── Menu Bar ──
        var menu = new MenuStrip();
        var fileMenu = new ToolStripMenuItem("游戏");
        fileMenu.DropDownItems.Add("新游戏", null, (s, e) => RestartGame());
        fileMenu.DropDownItems.Add("存档", null, (s, e) => HandleSave());
        fileMenu.DropDownItems.Add("读档", null, (s, e) => HandleLoad());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("退出", null, (s, e) => Close());
        menu.Items.Add(fileMenu);

        var viewMenu = new ToolStripMenuItem("视图");
        var themeItem = new ToolStripMenuItem("切换主题", null, (s, e) => ToggleTheme());
        viewMenu.DropDownItems.Add(themeItem);
        menu.Items.Add(viewMenu);

        var helpMenu = new ToolStripMenuItem("帮助");
        helpMenu.DropDownItems.Add("操作说明", null, (s, e) => ShowHelp());
        helpMenu.DropDownItems.Add("查看路径", null, (s, e) => ShowHistory());
        menu.Items.Add(helpMenu);

        Controls.Add(menu);

        // ── Title / POV Label ──
        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(16, 8, 16, 4)
        };

        _titleLabel = new Label
        {
            Text = engine.Title,
            Font = new Font("Microsoft YaHei", 13, FontStyle.Bold),
            ForeColor = Color.SteelBlue,
            AutoSize = true,
            Location = new Point(0, 0)
        };

        _povLabel = new Label
        {
            Font = new Font("Microsoft YaHei", 9, FontStyle.Regular),
            ForeColor = Color.DarkSeaGreen,
            AutoSize = true,
            Location = new Point(0, 24),
            Visible = false
        };

        headerPanel.Controls.Add(_titleLabel);
        headerPanel.Controls.Add(_povLabel);
        Controls.Add(headerPanel);

        // ── Main Text Area ──
        _textBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            Font = new Font("Microsoft YaHei", 11.5f),
            BackColor = Color.FromArgb(30, 30, 50),
            ForeColor = Color.FromArgb(224, 224, 224),
            ScrollBars = RichTextBoxScrollBars.Vertical,
            Padding = new Padding(24, 12, 24, 12),
            Visible = false
        };
        Controls.Add(_textBox);

        // ── Choices Panel ──
        _choicesPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(16, 8, 16, 12),
            WrapContents = false,
            Visible = false
        };
        Controls.Add(_choicesPanel);

        // ── Title Screen ──
        _titleScreen = CreateTitleScreen();
        Controls.Add(_titleScreen);
        _titleScreen.BringToFront();

        // ── Ending Screen ──
        _endingScreen = new Panel
        {
            Dock = DockStyle.Fill,
            Visible = false,
            BackColor = Color.FromArgb(30, 30, 50)
        };
        Controls.Add(_endingScreen);

        // ── Status Strip ──
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel("");
        _statusStrip.Items.Add(_statusLabel);
        Controls.Add(_statusStrip);

        Load += (s, e) => UpdateTheme();
        Resize += (s, e) => _titleScreen.Invalidate();
    }

    // ═══════════════════════════════════════════
    //  Title Screen
    // ═══════════════════════════════════════════

    private Panel CreateTitleScreen()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(30, 30, 50)
        };

        var titleLabel = new Label
        {
            Text = _engine.Title,
            Font = new Font("Microsoft YaHei", 22, FontStyle.Bold),
            ForeColor = Color.SteelBlue,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(600, 60),
            Location = new Point(0, 160)
        };
        panel.Controls.Add(titleLabel);

        var subtitle = new Label
        {
            Text = "星落降临，命运的分岔",
            Font = new Font("Microsoft YaHei", 11),
            ForeColor = Color.Gray,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(600, 30),
            Location = new Point(0, 220)
        };
        panel.Controls.Add(subtitle);

        var meta = new Label
        {
            Text = $"作者: {_engine.Title}  |  版本: 1.0",
            Font = new Font("Microsoft YaHei", 9),
            ForeColor = Color.DimGray,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(600, 25),
            Location = new Point(0, 250)
        };
        panel.Controls.Add(meta);

        var startBtn = new Button
        {
            Text = "开始游戏",
            Font = new Font("Microsoft YaHei", 12),
            Size = new Size(180, 48),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(0, 310)
        };
        startBtn.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 90);
        startBtn.Click += (s, e) => StartGame();
        panel.Controls.Add(startBtn);

        var loadBtn = new Button
        {
            Text = "读取存档",
            Font = new Font("Microsoft YaHei", 12),
            Size = new Size(180, 48),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(0, 370)
        };
        loadBtn.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 90);
        loadBtn.Click += (s, e) => HandleLoad();
        panel.Controls.Add(loadBtn);

        // Center the buttons and labels horizontally
        panel.Resize += (s, e) =>
        {
            var cx = panel.Width / 2;
            titleLabel.Location = new Point(cx - 300, 160);
            subtitle.Location = new Point(cx - 300, 220);
            meta.Location = new Point(cx - 300, 250);
            startBtn.Location = new Point(cx - 90, 310);
            loadBtn.Location = new Point(cx - 90, 370);
        };

        panel.Resize += (s, e) =>
        {
            var cx = panel.Width / 2;
            titleLabel.Left = cx - 300;
            subtitle.Left = cx - 300;
            meta.Left = cx - 300;
            startBtn.Left = cx - 90;
            loadBtn.Left = cx - 90;
        };

        return panel;
    }

    // ═══════════════════════════════════════════
    //  Game Flow
    // ═══════════════════════════════════════════

    private void StartGame()
    {
        _engine.State.CurrentNodeId = "start";
        _engine.State.Flags.Clear();
        _engine.State.History.Clear();
        _engine.State.ChoicesMade.Clear();
        _titleScreen.Visible = false;
        _endingScreen.Visible = false;
        RenderNode();
    }

    private void RestartGame()
    {
        if (_engine.State.History.Count > 0)
        {
            var result = MessageBox.Show("确定要重新开始吗？当前进度将丢失。", "重新开始",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;
        }
        StartGame();
    }

    private void RenderNode()
    {
        var node = _engine.CurrentNode;

        _titleScreen.Visible = false;
        _endingScreen.Visible = false;
        _textBox.Visible = true;
        _choicesPanel.Visible = true;

        // Update header
        _titleLabel.Text = node.Title != null
            ? $"{_engine.Title} · {node.Title}"
            : _engine.Title;

        // POV indicator
        if (!string.IsNullOrEmpty(node.Perspective))
        {
            _povLabel.Text = $"当前视角: {node.Perspective}";
            _povLabel.Visible = true;
        }
        else
        {
            _povLabel.Visible = false;
        }

        _statusLabel.Text = $"位置: {node.Id}";

        // Ending?
        if (node.IsEnding)
        {
            _textBox.Visible = false;
            _choicesPanel.Visible = false;
            ShowEnding(node);
            return;
        }

        // Render text
        _textBox.Clear();
        SetRichText(_textBox, node.Text);

        // Render choices
        _choicesPanel.Controls.Clear();
        var choices = _engine.GetAvailableChoices();

        if (choices.Count == 0)
        {
            var errLabel = new Label
            {
                Text = "[错误] 当前节点没有可用选项。",
                ForeColor = Color.Red,
                AutoSize = true
            };
            _choicesPanel.Controls.Add(errLabel);
            return;
        }

        for (int i = 0; i < choices.Count; i++)
        {
            var choice = choices[i];
            var btn = new Button
            {
                Text = $"[{i + 1}]  {choice.Text}",
                AutoSize = true,
                MinimumSize = new Size(560, 42),
                MaximumSize = new Size(780, 80),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei", 10.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 14, 0),
                Tag = i
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(50, 50, 80);
            btn.Click += ChoiceButton_Click;
            _choicesPanel.Controls.Add(btn);
        }
    }

    private void ChoiceButton_Click(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.Tag is int index)
        {
            try
            {
                _engine.MakeChoice(index);
                RenderNode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ShowEnding(StoryNode node)
    {
        _endingScreen.Controls.Clear();
        _endingScreen.Visible = true;

        var endingType = node.EndingType ?? "neutral";
        var typeLabels = new Dictionary<string, string>
        {
            ["good"] = "好结局",
            ["bad"] = "坏结局",
            ["neutral"] = "普通结局",
            ["true"] = "真结局",
            ["secret"] = "隐藏结局"
        };
        var typeColors = new Dictionary<string, Color>
        {
            ["good"] = Color.ForestGreen,
            ["bad"] = Color.Firebrick,
            ["neutral"] = Color.Silver,
            ["true"] = Color.SteelBlue,
            ["secret"] = Color.Goldenrod
        };

        var color = typeColors.GetValueOrDefault(endingType, Color.White);

        var titleLabel = new Label
        {
            Text = node.EndingTitle ?? "终幕",
            Font = new Font("Microsoft YaHei", 20, FontStyle.Bold),
            ForeColor = color,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(600, 50),
            Location = new Point(0, 80)
        };
        _endingScreen.Controls.Add(titleLabel);

        var typeLabel = new Label
        {
            Text = typeLabels.GetValueOrDefault(endingType, endingType),
            Font = new Font("Microsoft YaHei", 11),
            ForeColor = color,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(200, 28),
            Location = new Point(0, 134)
        };
        _endingScreen.Controls.Add(typeLabel);

        var stats = new Label
        {
            Text = $"经过 {_engine.State.History.Count} 个场景，做出了 {_engine.State.ChoicesMade.Count} 次选择",
            Font = new Font("Microsoft YaHei", 9),
            ForeColor = Color.Gray,
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.None,
            Size = new Size(600, 20),
            Location = new Point(0, 170)
        };
        _endingScreen.Controls.Add(stats);

        var replayBtn = new Button
        {
            Text = "再玩一次",
            Font = new Font("Microsoft YaHei", 11),
            Size = new Size(140, 42),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(0, 230)
        };
        replayBtn.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 90);
        replayBtn.Click += (s, e) => StartGame();
        _endingScreen.Controls.Add(replayBtn);

        var loadBtn = new Button
        {
            Text = "读取存档",
            Font = new Font("Microsoft YaHei", 11),
            Size = new Size(140, 42),
            FlatStyle = FlatStyle.Flat,
            Location = new Point(0, 280)
        };
        loadBtn.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 90);
        loadBtn.Click += (s, e) => HandleLoad();
        _endingScreen.Controls.Add(loadBtn);

        // Center everything
        _endingScreen.Resize += (s, e) =>
        {
            var cx = _endingScreen.Width / 2;
            titleLabel.Left = cx - 300;
            typeLabel.Left = cx - 100;
            stats.Left = cx - 300;
            replayBtn.Left = cx - 150;
            loadBtn.Left = cx + 10;
        };
    }

    // ═══════════════════════════════════════════
    //  Save / Load
    // ═══════════════════════════════════════════

    private void HandleSave()
    {
        if (!_textBox.Visible) return;

        using var dialog = new SaveFileDialog
        {
            Title = "保存游戏",
            Filter = "存档文件 (*.json)|*.json",
            DefaultExt = "json",
            FileName = $"save_{DateTime.Now:yyyyMMdd_HHmmss}.json",
            InitialDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "InteractiveStory", "saves")
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dialog.FileName)!);
            var json = System.Text.Json.JsonSerializer.Serialize(_engine.State,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dialog.FileName, json, Encoding.UTF8);
            _statusLabel.Text = $"已保存: {Path.GetFileName(dialog.FileName)}";
        }
    }

    private void HandleLoad()
    {
        using var dialog = new OpenFileDialog
        {
            Title = "读取存档",
            Filter = "存档文件 (*.json)|*.json",
            InitialDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "InteractiveStory", "saves")
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var json = File.ReadAllText(dialog.FileName, Encoding.UTF8);
                var loaded = System.Text.Json.JsonSerializer.Deserialize<GameState>(json,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (loaded != null)
                {
                    _engine.State.CurrentNodeId = loaded.CurrentNodeId;
                    _engine.State.Flags = loaded.Flags;
                    _engine.State.History = loaded.History;
                    _engine.State.ChoicesMade = loaded.ChoicesMade;
                    _titleScreen.Visible = false;
                    _endingScreen.Visible = false;
                    RenderNode();
                    _statusLabel.Text = $"已读取存档: {Path.GetFileName(dialog.FileName)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取存档失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ═══════════════════════════════════════════
    //  Helpers
    // ═══════════════════════════════════════════

    private void SetRichText(RichTextBox box, string text)
    {
        box.Clear();
        var paragraphs = text.Split("\n\n");
        foreach (var para in paragraphs)
        {
            var trimmed = para.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;
            box.SelectionIndent = 24;
            box.SelectionRightIndent = 24;
            box.AppendText(trimmed);
            box.AppendText("\n\n");
        }
        box.Select(0, 0);
    }

    private void ToggleTheme()
    {
        _darkTheme = !_darkTheme;
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        if (_darkTheme)
        {
            BackColor = Color.FromArgb(30, 30, 50);
            _textBox.BackColor = Color.FromArgb(30, 30, 50);
            _textBox.ForeColor = Color.FromArgb(224, 224, 224);
            _titleScreen.BackColor = Color.FromArgb(30, 30, 50);
            _endingScreen.BackColor = Color.FromArgb(30, 30, 50);
            _choicesPanel.BackColor = Color.FromArgb(30, 30, 50);

            foreach (Button btn in _choicesPanel.Controls.OfType<Button>())
            {
                btn.BackColor = Color.FromArgb(40, 40, 65);
                btn.ForeColor = Color.FromArgb(224, 224, 224);
                btn.FlatAppearance.BorderColor = Color.FromArgb(50, 50, 80);
            }
        }
        else
        {
            BackColor = Color.WhiteSmoke;
            _textBox.BackColor = Color.White;
            _textBox.ForeColor = Color.FromArgb(30, 30, 30);
            _titleScreen.BackColor = Color.WhiteSmoke;
            _endingScreen.BackColor = Color.WhiteSmoke;
            _choicesPanel.BackColor = Color.WhiteSmoke;

            foreach (Button btn in _choicesPanel.Controls.OfType<Button>())
            {
                btn.BackColor = Color.FromArgb(230, 230, 240);
                btn.ForeColor = Color.FromArgb(30, 30, 30);
                btn.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 200);
            }
        }
    }

    private void ShowHelp()
    {
        MessageBox.Show(
            "互动故事游戏 — 操作说明\n\n" +
            "· 点击选项按钮进行选择\n" +
            "· 游戏 → 存档 / 读档 来保存进度\n" +
            "· 视图 → 切换主题 在深色/浅色之间切换\n" +
            "· 帮助 → 查看路径 回顾你的旅程\n\n" +
            "键盘快捷键:\n" +
            "· 数字键 1-9: 快速选择\n" +
            "· Ctrl+S: 存档\n" +
            "· Ctrl+L: 读档",
            "操作说明",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ShowHistory()
    {
        var sb = new StringBuilder();
        sb.AppendLine("═══ 已走过的路径 ═══\n");
        foreach (var nodeId in _engine.State.History)
        {
            sb.AppendLine($"  → {nodeId}");
        }
        sb.AppendLine($"  → {_engine.CurrentNode.Id} ◁ 当前");

        MessageBox.Show(sb.ToString(), "路径历史",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
