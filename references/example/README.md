# 星落之夜的抉择

> 一个奇幻悬疑题材的互动文字冒险游戏示例项目 — 三种游戏格式全支持

## 项目结构

```
example_星落之夜/
├── 01_角色设定/                    # 3位OC的完整档案（含场景角色分配）
│   ├── oc_profiles.md
│   └── oc_profiles.json
├── 02_世界观设定/                  # 艾瑟兰大陆魔法学院世界观
│   ├── world_setting.md
│   └── world_setting.json
├── 03_故事风格与互动设计/           # 风格偏好、视角设计、OC参与度、游戏格式选择
│   ├── style_preferences.json
│   └── design_notes.md
├── 04_故事结构设计/                # 15节点3结局的故事分支
│   ├── branching_outline.md
│   └── story_nodes.json
├── 05_故事文档/                    # 完整可读故事文本
│   └── story.md
├── 06_控制台游戏/                  # ⭐ C# 控制台互动游戏
│   ├── 启动游戏.bat               # 一键启动（双击即可）
│   ├── InteractiveStory.csproj
│   ├── Program.cs / GameState.cs / StoryData.cs / StoryEngine.cs / SaveManager.cs
├── 07_WinForms游戏/               # ⭐ C# WinForms 桌面窗口游戏
│   ├── InteractiveStoryWF.csproj
│   ├── Program.cs / MainForm.cs
│   └── GameState.cs / StoryData.cs / StoryEngine.cs / SaveManager.cs
├── 08_HTML游戏/                   # ⭐ 浏览器游戏（无需安装，双击即玩）
│   ├── index.html                 # 完整游戏（HTML+CSS+JS）
│   └── story_data.js              # 故事数据
└── README.md
```

## 故事概要

帝国历847年，星落降临。魔法学院学徒**林星河**——世上仅存的星象魔法师——必须在燃烧的村庄、禁忌的真相和帝国的追兵之间做出抉择。每一个决定都在星空之下留下痕迹。

**15个场景节点 · 3个结局（Good / Neutral / True）**

## OC 角色一览

| OC | 场景角色 | 出场率 | 说明 |
|----|---------|--------|------|
| 林星河 | 主角团成员 | 100% | 玩家扮演的主角，星象魔法学徒 |
| 艾琳 | 关键配角 | 40% | 冰系魔法大师，学院院长，导师兼养母 |
| 卡修斯 | 关键配角 | 30% | 战斗法师学徒，劲敌兼挚友 |

**叙事视角**: 单一视角（林星河），第二人称（"你"）

## 互动权重

解谜 35% · 社交 25% · 战斗 15% · 探索 15% · 抉择 10%

## 运行方式

### HTML 游戏（推荐 — 零门槛）
```
双击 08_HTML游戏/index.html
```
在浏览器中直接游玩，支持存档（localStorage）、键盘快捷键（1-9选择、S存档、L读档）。可分享给任何人。

### 控制台游戏（轻量终端版）
```
双击 06_控制台游戏/启动游戏.bat
```
自动检测 .NET SDK、编译、启动。游戏指令：数字选择选项 · `save` 存档 · `load` 读档 · `quit` 退出 · `history` 查看路径

### WinForms 游戏（桌面窗口版）
```bash
cd 07_WinForms游戏
dotnet run
```
图形化窗口界面：标题画面 → 按钮选择 → 深色/浅色主题切换 → 菜单存档/读档。支持 Ctrl+S/Ctrl+L 快捷键。

存档文件保存在 `Documents\InteractiveStory\saves\`
