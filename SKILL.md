---
name: auto-oc-interactive-story
description: ONLY trigger when the user explicitly says to invoke this skill (e.g., "调用auto_OC_InteractiveStory", "使用互动故事skill", "create interactive story game"). After the skill is loaded, ask the user directly what they want — whether to create OCs, build a world, write a story, or generate games. This skill co-creates multiple Original Characters and world settings, supports single or multi-perspective narratives, designs multi-ending branching stories, and generates playable interactive fiction as a C# console app, C# WinForms app, and/or standalone HTML game — user chooses which formats to produce.
---

# Auto OC Interactive Story

A skill for co-creating Original Characters (OCs) and world settings, designing branching multi-ending stories with optional multi-perspective support, and generating playable interactive fiction applications in up to three formats.

**Reference Example**: A complete worked example lives at `references/example/` — a 15-node, 3-ending fantasy-mystery interactive story called "星落之夜的抉择" with all 7 phases fully populated. When in doubt about output format, structure, or quality expectations, **read the corresponding file in `references/example/` first** and use it as a template.

## Overview

This skill operates in up to seven sequential phases. The user chooses which game formats to produce — phases 5/6/7 are generated only if selected. Each phase builds on the previous one. Always present the user with the current phase's output for approval before proceeding to the next.

```
Phase 1: OC & World-Building  →  Phase 2: Style, Perspective & Game Format  →  Phase 3: Story Design
                                  →  Phase 4: Story Writing  →  Phase 5-7: Games (Console / WinForms / HTML)
```

**Critical — Folder Management**: The AI MUST proactively create all folders at the start of each phase. Do not wait for the user to ask. Each phase's outputs go into numbered, categorized directories. Before writing any file, ensure its parent directory exists.

The user may start at any phase. If they already have OC profiles or a written story, skip to the appropriate phase. Always ask which phase to start from if not clear.

When the user chooses "flexible mode" (recommended default), they can at each step decide to:
- **Provide** their own content
- **Let AI generate** everything
- **Mix** — provide some, let AI fill gaps

When AI generates content, always confirm genre, tone, and rough scope with the user first.

## Output Directory Structure

All outputs go under `interactive_story_output/<project-name>/`. The AI must create the full directory tree before writing files. Use numbered prefixes so folders sort by phase order.

```
interactive_story_output/<project-name>/
├── 01_角色设定/                    # Phase 1 output
│   ├── oc_profiles.md             # OC profiles — narrative bio for each character
│   └── oc_profiles.json           # OC profiles — structured data (schema: references/oc_schema.md)
├── 02_世界观设定/                  # Phase 1 output
│   ├── world_setting.md           # World-building — narrative description
│   └── world_setting.json         # World rules, systems, locations (structured)
├── 03_故事风格与互动设计/           # Phase 2 output
│   ├── style_preferences.json     # Genre, tone, interaction weights, OC participation, perspective, game formats
│   └── design_notes.md            # Design decisions, pacing notes, special mechanics
├── 04_故事结构设计/                # Phase 3 output
│   ├── branching_outline.md       # Visual outline of all nodes and connections
│   └── story_nodes.json           # Machine-readable branching data (schema: references/story_schema.md)
├── 05_故事文档/                    # Phase 4 output
│   └── story.md                   # Complete narrative — every node's full text with choices
├── 06_控制台游戏/                  # Phase 5 output — C# console application (if selected)
│   ├── InteractiveStory.csproj
│   ├── Program.cs
│   ├── GameState.cs
│   ├── StoryData.cs
│   ├── StoryEngine.cs
│   └── SaveManager.cs
├── 07_WinForms游戏/               # Phase 6 output — C# WinForms application (if selected)
│   ├── InteractiveStoryWF.csproj
│   ├── Program.cs
│   ├── MainForm.cs
│   ├── GameState.cs
│   ├── StoryData.cs
│   ├── StoryEngine.cs
│   └── SaveManager.cs
├── 08_HTML游戏/                   # Phase 7 output — browser-based game (if selected)
│   ├── index.html                 # Self-contained HTML game (HTML + CSS + JS in one file)
│   └── story_data.js              # Story nodes as a JavaScript module
└── README.md                      # Project overview, build instructions, credits
```

**Folder creation rule**: At the start of each phase, run `mkdir -p` (or equivalent) for that phase's directory before writing any files. Never write files into a non-existent directory.

## Phase 1: OC & World-Building

### Step 1.1: Determine Source

Ask the user: "Do you want to provide your own OC/world details, have me generate them, or a mix?"

If the user provides details, collect them interactively. If AI-generated, ask for genre preferences (fantasy, sci-fi, modern, historical, etc.), tone (dark, lighthearted, epic, slice-of-life), and any must-have elements.

### Step 1.2: Build OC Profiles — Support Multiple OCs

**Encourage the user to create multiple OCs.** A rich story needs a cast. Ask: "How many OCs would you like to create? I recommend at least 3 for a compelling story — a protagonist, a supporting character, and an antagonist or foil." Process each OC one at a time.

For each OC, collect or generate the following. See `references/oc_schema.md` for the full JSON structure.

Required fields: name, gender, age, personality, appearance, background, speaking_style
Optional fields: mbti, birthday, height, hobbies, catchphrases, likes, dislikes, abilities, occupation

### Step 1.3: Assign Scene Roles for Each OC

After all OC profiles are created, **assign each OC a scene role** that determines their weight in the story. These roles will be stored in the OC profile's `scene_role` field.

| Role | Label | Description |
|------|-------|-------------|
| `main_cast` | 主角团成员 | Core party. Appears in most nodes. Deeply involved in the main plot. Player may control them. |
| `key_supporting` | 关键配角 | Appears in specific storylines and pivotal scenes. Has their own arc. Not part of the core party but essential. |
| `regular_npc` | 普通NPC | Functional role — shopkeepers, guards, informants. Appears in limited nodes. |
| `background` | 路人 | Background flavor. Mentioned or briefly encountered. No significant plot impact. |

Ask the user to confirm or adjust the role assignments. If there are many OCs, the distribution should roughly be:
- 1-3 in `main_cast`
- 2-5 in `key_supporting`
- The rest in `regular_npc` or `background`

Save roles into each OC's profile in `01_角色设定/oc_profiles.json` (add a `scene_role` field). Also save to `01_角色设定/oc_profiles.md`.

### Step 1.4: Build World Setting

Collect or generate:
- **Setting basics**: time period, location, genre
- **Rules & systems**: magic, technology, politics, economy
- **Key locations**: places the story will visit
- **History & lore**: major events shaping the world

Save to `02_世界观设定/world_setting.md` and `02_世界观设定/world_setting.json`. Show the user and get approval.

## Phase 2: Style, Perspective & Game Format

This phase determines the creative direction, narrative POV, and output formats BEFORE any story design begins.

### Step 2.1: Genre & Tone

Ask the user to choose from these categories. They can select one primary genre and blend in secondary elements.

**Primary genre** (pick one):
- 奇幻 (Fantasy) — magic, mythical creatures, medieval realms
- 科幻 (Sci-Fi) — future tech, space, AI, cybernetics
- 恐怖 (Horror) — psychological fear, supernatural dread, survival horror
- 悬疑 (Mystery) — investigation, clues, twists, whodunit
- 恋爱 (Romance) — relationship-driven, emotional stakes
- 武侠/仙侠 (Wuxia/Xianxia) — martial arts, cultivation, ancient China
- 都市 (Urban/Modern) — contemporary setting, real-world issues
- 末日 (Post-Apocalyptic) — wasteland survival, collapsed civilization
- 历史 (Historical) — real historical period with fictional elements
- 日常 (Slice-of-Life) — warm, gentle, character-driven everyday stories

**Tone** (pick one primary + up to two modifiers):
- Primary: 轻松温馨 / 严肃沉重 / 黑暗压抑 / 热血燃向 / 诙谐幽默 / 文艺诗意
- Modifiers: 治愈、致郁、热血、虐心、搞笑、悬疑、惊悚、浪漫、哲学

**Pacing** (pick one):
- 快节奏 — action every few scenes, rapid plot progression
- 中等 — balanced mix of action and reflection
- 慢节奏 — slow burn, deep character development, atmospheric buildup

### Step 2.2: Interaction Design Weight

This determines what KIND of choices the player faces. Ask the user to distribute 100 points across these five dimensions. If the user doesn't want to do math, ask them to rank their top 3 instead, and assign default weights (40/25/20/10/5).

| Dimension | Description | High weight means... |
|-----------|-------------|---------------------|
| 解谜 (Puzzle) | Logic, clues, environmental puzzles, codes | Choices involve figuring things out — what to investigate, how to connect clues |
| 战斗 (Combat) | Physical conflict, strategy, life-or-death | Choices involve fight-or-flight, tactics, resource management in battle |
| 社交 (Social) | Dialogue, relationships, persuasion, politics | Choices involve what to say, who to trust, how to navigate relationships |
| 探索 (Exploration) | Discovery, world detail, hidden secrets | Choices involve where to go, what to examine, branching paths in the environment |
| 抉择 (Moral Dilemma) | Hard ethical choices, sacrifice, consequence | Choices involve trade-offs with no clear right answer, values tested |

**How weights affect story design**: If 解谜 is 40%, roughly 40% of all choice nodes should involve puzzle-solving. If 社交 is 30%, about 30% of choices should be dialogue/relationship-driven. The weights directly shape the kind of branching the story uses.

#### Choice Density (选项密集度)

Also ask the user how frequently they want choices to appear. This is separate from interaction weights — it's about pacing, not content type.

| Level | Label | Description |
|-------|-------|-------------|
| `dense` | 密集 | A choice every 1-3 paragraphs (~200-500字 between choices). Fast-paced, highly interactive. Best for short-form stories and players who want constant agency. |
| `moderate` | 适中 | A choice every 4-7 paragraphs (~500-1500字 between choices). Balanced reading and interaction. Best for most stories. (Default) |
| `sparse` | 稀疏 | A choice every 8+ paragraphs (~1500-3000字 between choices). Long narrative passages, fewer but more consequential decisions. Best for epic/literary stories. |

This directly maps to how many words each non-ending node should contain. The AI must respect this when writing node text in Phase 4.

### Step 2.3: Narrative Perspective & OC Participation

This is the most critical structural decision for the story. Ask the user:

#### A. Default Story Perspective

Which narrative voice does the story use?
- **第二人称** ("你") — "You walk into the room..." — most immersive for interactive fiction (default)
- **第一人称** ("我") — "I walk into the room..." — more intimate, better for deep character study

#### B. Single or Multi-Perspective?

Ask: "Should the entire story be told from ONE character's point of view, or should the player switch between multiple characters at different points in the story?"

**Single Perspective** (default for simplicity):
- Ask: "Which OC is the protagonist / player character?" — select one OC from `main_cast`
- This OC is the lens through which every scene is experienced
- All other OCs are NPCs the protagonist interacts with
- Simpler story design, easier to write, easier to code

**Multi-Perspective** (more ambitious):
- Ask: "Which OCs can the player control? And roughly what percentage of the story is from each OC's perspective?"
- Example: "林星河 50%, 艾琳 30%, 卡修斯 20%"
- The story will have designated POV-switch nodes where the player's perspective changes
- Each playable OC's `scene_role` must be `main_cast` or `key_supporting`
- Multi-perspective nodes need a `perspective` field in story_nodes.json indicating which OC is the current POV
- If multi-perspective, ask: "Should POV switches happen at fixed story points (simpler), or should the player sometimes CHOOSE which character's perspective to follow (more complex)?"

#### C. OC Participation Weights (for ALL OCs)

Regardless of single or multi-perspective, every OC has a participation weight that determines how often they appear. These should already be roughly set by the `scene_role` from Step 1.3. Now refine them:

- For each `main_cast` OC: what % of nodes do they appear in? (typically 50-100%)
- For each `key_supporting` OC: what % of nodes? (typically 20-50%)
- For `regular_npc` and `background`: specific nodes where they appear

Save this as `oc_participation` in style_preferences.json.

### Step 2.4: Game Format Selection

**Ask the user which game format(s) to generate.** They can pick one, two, or all three. **Strongly recommend HTML as the default choice** — it requires zero installation, works on any platform, and can be shared by simply sending a file.

| Format | Directory | Recommendation | Requirements |
|--------|-----------|----------------|--------------|
| **HTML Game** ⭐ | `08_HTML游戏/` | **强烈推荐 (Strongly Recommended)** — 零门槛，双击即玩，可分享，跨平台。适合所有人。 | 任何现代浏览器 |
| Console App | `06_控制台游戏/` | 不推荐 (Not Recommended) — 需要安装 .NET SDK，仅限 Windows 终端用户。仅当用户明确需要终端体验时才生成。 | .NET SDK |
| WinForms App | `07_WinForms游戏/` | 不推荐 (Not Recommended) — 需要 .NET SDK + Windows，界面不如 HTML 美观灵活。仅当用户明确需要桌面窗口应用时才生成。 | .NET SDK + Windows |

**Default recommendation**: If the user is unsure, generate **HTML only**. It covers 95% of use cases. Console and WinForms should only be generated when the user explicitly asks for them or has a clear technical reason.

Always tell the user: *"我推荐生成 HTML 版本——双击 index.html 就能在浏览器里玩，不需要安装任何东西，而且可以发给朋友直接玩。控制台和窗体版本需要安装 .NET SDK 才能运行，通常没必要，除非你有特别的需求。"*

### Step 2.5: Additional Preferences

Ask the user:
- **Death/failure**: Can the player reach a "dead end" bad ending mid-story, or do all paths play through to a proper ending?
- **Romance**: Should romantic subplots be included? If yes, which OCs are romanceable?
- **Special mechanics**: Any unique gameplay ideas? (e.g., inventory system, time limit, sanity meter, relationship points)
- **Pronouns**: For second-person POV, what pronoun does the narrative use? (你=default, 您=formal)

### Step 2.6: Save Style Preferences

Save all decisions from Phase 2 to `03_故事风格与互动设计/style_preferences.json`:

```json
{
  "genre": {
    "primary": "奇幻",
    "secondary": ["悬疑"]
  },
  "tone": {
    "primary": "严肃沉重",
    "modifiers": ["悬疑", "哲学"]
  },
  "pacing": "中等",
  "choice_density": "moderate",
  "interaction_weights": {
    "解谜": 35,
    "战斗": 15,
    "社交": 25,
    "探索": 15,
    "抉择": 10
  },
  "narrative_perspective": "第二人称",
  "perspective_mode": "single",
  "protagonist_oc": "林星河",
  "multi_perspective_config": null,
  "oc_participation": {
    "林星河": {"scene_role": "main_cast", "appearance_pct": 100, "playable": true, "pov_weight": 100},
    "艾琳": {"scene_role": "key_supporting", "appearance_pct": 40, "playable": false, "pov_weight": 0},
    "卡修斯": {"scene_role": "key_supporting", "appearance_pct": 30, "playable": false, "pov_weight": 0}
  },
  "game_formats": ["console", "html"],
  "story_scope": {
    "scale": "中篇",
    "target_words": 15000,
    "target_nodes": 20,
    "target_endings": 3,
    "ending_types": ["good", "neutral", "true"]
  },
  "death_possible": false,
  "romance_enabled": false,
  "special_mechanics": []
}
```

For multi-perspective, `perspective_mode` would be `"multi"` and `multi_perspective_config` would contain:

```json
{
  "switch_type": "fixed",
  "playable_ocs": [
    {"name": "林星河", "pov_weight": 50},
    {"name": "艾琳", "pov_weight": 30},
    {"name": "卡修斯", "pov_weight": 20}
  ]
}
```

Also save a human-readable summary to `03_故事风格与互动设计/design_notes.md` explaining the rationale behind all choices. Show both files to the user for approval before proceeding.

## Phase 3: Story Design

### Step 3.1: Scope Confirmation

Ask the user about story scale. Present these reference tiers to help them decide:

| Scale | Total Word Count | Nodes | Endings | Best For |
|-------|-----------------|-------|---------|----------|
| **短篇** | 3,000-8,000字 | 8-15 | 2-3 | Quick play (10-20 min), simple branching |
| **中篇** | 8,000-25,000字 | 15-30 | 3-4 | Standard experience (20-60 min), meaningful depth |
| **长篇** | 25,000-60,000字 | 30-60 | 4-5 | Epic journey (1-3 hours), high replayability |
| **自定义** | User specifies | User specifies | User specifies | Full creative control |

Ask the user:
- Which scale tier? (短篇/中篇/长篇/自定义)
- If custom: target word count, number of nodes, number of endings
- What ending types? (e.g., good/bad/neutral/true/secret)
- Should ending types be evenly distributed, or weighted toward certain types?

**Important**: The node count and word count must be consistent with the choice density from Phase 2. A `dense` density × 30 nodes ≈ ~6,000-15,000字 total, while a `sparse` density × 30 nodes ≈ ~45,000-90,000字. If the user's choices contradict (e.g., "短篇" + "稀疏" + "40 nodes"), flag the inconsistency and help them adjust.

### Step 3.2: Design the Branching Structure

Design the story as a directed graph of nodes. Each node contains narrative text and a set of choices. See `references/story_schema.md` for the complete JSON schema.

**Critical — Apply Phase 2 decisions here:**
- The distribution of choice types (解谜/战斗/社交/探索/抉择) across all nodes must roughly match the interaction weights
- Node narrative must reflect the assigned `perspective` OC's voice, knowledge, and personality
- OC appearances must match the `oc_participation` weights: if an OC has 40% appearance_pct, they should appear in ~40% of nodes
- For multi-perspective stories, nodes must have a `perspective` field indicating which OC is the current POV character

Key rules for story design:
- Every node has a unique `id` string (e.g., "start", "forest_path", "ending_good")
- Non-ending nodes have `choices` — an array of `{text, nextNode}` objects
- Ending nodes have `isEnding: true` and `endingType` (good, bad, neutral, true, secret)
- The story starts from a node with id `"start"`
- Every path must eventually reach an ending node (no infinite loops)
- Nodes can have optional `condition` and `effects` for tracking game state (flags, inventory)
- For multi-perspective: nodes can have a `perspective` field (the OC name) — games use this to display "Playing as [OC Name]" and adapt UI

First, present the branching structure as a visual outline (`04_故事结构设计/branching_outline.md`) showing all nodes, their connections, which interaction dimension each choice belongs to, and which OC's POV each node is from (if multi-perspective). Then save the machine-readable data to `04_故事结构设计/story_nodes.json`. Get user approval on the outline before proceeding.

## Phase 4: Story Writing

Write the full narrative for every node in `04_故事结构设计/story_nodes.json`. Follow these guidelines:

- Each node's text should be substantial — typically 100-500 words depending on the user's preference
- Choice texts should clearly indicate the consequence ("Confront the guard" vs "Try sneaking past")
- Maintain consistent character voice based on OC profiles — the narrative tone should match the POV character
- For multi-perspective: each POV character's nodes should feel distinctly different in voice, knowledge, and focus
- Match the tone and pacing set in Phase 2
- World rules established in Phase 1 must be respected
- Use the narrative perspective chosen in Phase 2 (第二人称 or 第一人称)

Save the complete story to `05_故事文档/story.md`. This file contains every node's text in order, with clear node ID headers, the POV character (if multi-perspective), followed by the choices at that node. It serves as both documentation and a readable "script" of the entire story.

## Phase 5: Console Application (if selected)

If the user selected `"console"` in their game_formats, generate a complete C# console application.

### Technical Requirements

- **Target**: .NET 9.0 (check `dotnet --version` first; use what's available)
- **Project type**: Console Application
- **Dependencies**: System.Text.Json (built-in)

### File-by-File Specification

**`InteractiveStory.csproj`**: Standard .NET console project file targeting the chosen framework. Include an `<ItemGroup>` that copies `story_nodes.json` to the output directory.

**`Program.cs`**: Entry point. Shows title screen (with story title, author, version), initializes `StoryEngine`, runs the main game loop. On each turn: display current node text, present numbered choices, accept user input, navigate to next node. Handle "save", "load", and "quit" commands at any prompt. If multi-perspective, display "当前视角: [OC Name]" in the header bar.

**`GameState.cs`**: Holds runtime state:
- `CurrentNodeId` — current position in the story
- `Flags` — `Dictionary<string, object>` for tracking conditions (e.g., `{"has_sword": true, "reputation": 5}`)
- `History` — `List<string>` of visited node IDs for backtrack display
- `ChoicesMade` — `List<{nodeId, choiceIndex}>` for save file

**`StoryData.cs`**: Loads and provides access to `story_nodes.json`. Contains classes:
- `StoryRoot` — wraps `List<StoryNode>`
- `StoryNode` — `Id`, `Text`, `Choices`, `IsEnding`, `EndingType`, `Perspective` (optional, for multi-POV)
- `Choice` — `Text`, `NextNode`

The JSON file should be loaded from `../04_故事结构设计/story_nodes.json` (relative path from the project directory).

**`StoryEngine.cs`**: Core game logic:
- `DisplayNode(StoryNode)` — writes text to console with paging
- `GetChoices(StoryNode)` — returns valid choices (evaluates conditions against GameState)
- `Navigate(string nextNodeId)` — moves to target node, applies effects to GameState
- `CheckEnding()` — returns true if current node is an ending
- Handle save/load commands at any input prompt

**`SaveManager.cs`**: JSON file save/load:
- `Save(GameState, string path)` — serialize GameState to JSON
- `GameState Load(string path)` — deserialize GameState from JSON
- Save files go to `%USERPROFILE%\Documents\InteractiveStory\saves\`
- Auto-detect available save files, show list when loading

### Console UI Guidelines

- Use `Console.Clear()` between scenes for readability
- Use `Console.ForegroundColor` for emphasis (choices in cyan, warnings in yellow, endings in green/red)
- Paged text output: show text in chunks, wait for keypress before continuing
- Choice input: numbered list, accept number input with validation
- Show a header bar with: current scene name, POV character (if multi-perspective), flags summary (collapsible)
- At endings: display "THE END — [Ending Type]" and offer to restart or load

### Save/Load Integration

- Player can type "save" at any choice prompt to save
- Player can type "load" at any choice prompt to load a previous save
- Auto-save at each choice point (optional, ask user preference)
- Save files named: `<project-name>_slot<N>.json`

## Phase 6: WinForms Application (if selected)

If the user selected `"winforms"` in their game_formats, generate a Windows Forms version with a graphical UI. Reuse the same backend logic (GameState, StoryData, StoryEngine, SaveManager) from the console app.

### Technical Requirements

- **Target**: .NET 9.0 Windows Forms
- **Project type**: Windows Forms Application
- **Additional dependency**: none (pure WinForms)

### Architecture

**`MainForm.cs`**: The main window (800x600 default, resizable).

Layout (top to bottom):
1. **Menu bar** — File menu: New Game, Save, Load, Exit
2. **Title label** — Story title, bold, larger font
3. **POV indicator** (if multi-perspective) — "当前视角: [OC Name]"
4. **Status strip** (bottom) — Current node name, flags count
5. **Main text area** (center, fills space) — `RichTextBox`, read-only, with scrollbar
6. **Choices panel** (bottom, docked) — `FlowLayoutPanel` with dynamically generated `Button` controls

Behavior:
- On startup: title screen + "New Game" / "Load Game" buttons
- During gameplay: display node text, generate choice buttons, show POV character
- On ending: show ending text, "Play Again" / "Return to Title" buttons
- Save/Load: use `SaveFileDialog` / `OpenFileDialog` with .json filter
- Text formatting: bold for character names, italics for thoughts

### WinForms UI Guidelines

- Clean, readable layout with good spacing
- `RichTextBox` with `Font = new Font("Microsoft YaHei", 11)` for CJK text support
- Choice buttons: `AutoSize = true`, `MinimumSize = (400, 40)`, clear text
- Background: dark theme or light theme — ask user preference
- Respond to `Form.Resize` — text area and choice panel should scale properly

## Phase 7: HTML Game (if selected)

If the user selected `"html"` in their game_formats, generate a self-contained browser-based game.

### Why HTML?
- **Zero install** — double-click `index.html` and it works in any modern browser
- **Shareable** — email the file, host it on any web server, or put it on a USB drive
- **Cross-platform** — works on Windows, Mac, Linux, even mobile browsers

### Technical Requirements

- **Single file**: `index.html` containing all HTML, CSS, and JavaScript (no external dependencies)
- **Data file**: `story_data.js` — the story nodes exported as a JavaScript module
- **Browser compatibility**: Chrome, Firefox, Edge, Safari (any modern browser)

### File Specification

**`story_data.js`**: Export the story nodes as a JavaScript constant:

```javascript
const STORY_DATA = {
  "title": "星落之夜的抉择",
  "author": "AI & User",
  "version": "1.0",
  "perspective_mode": "single",
  "nodes": [ /* ... same structure as story_nodes.json ... */ ]
};
```

**`index.html`**: A complete, self-contained interactive fiction player. Must include:

1. **HTML structure**:
   - Title bar (story name)
   - POV indicator (if multi-perspective: "当前视角: [OC Name]")
   - Main text area (large, scrollable)
   - Choices container (buttons)
   - Save/Load toolbar (bottom)
   - Ending screen overlay

2. **CSS styling**:
   - Dark theme by default (easy on eyes for long reading)
   - Responsive layout (works on desktop and mobile)
   - Smooth text fade-in animation
   - Choice buttons with hover effects
   - Ending screen with colored border (green=good, red=bad, cyan=true, gold=secret)

3. **JavaScript game engine**:
   - Parse `STORY_DATA` on load
   - Main game loop: show node text → show choices → navigate on click
   - Condition evaluation (same logic as C# StoryEngine)
   - Effects application (same logic as C# StoryEngine)
   - Save/Load using `localStorage` (browser-native, no files needed):
     - Save: `localStorage.setItem('save_slot_N', JSON.stringify(gameState))`
     - Load: `JSON.parse(localStorage.getItem('save_slot_N'))`
     - List saves by scanning localStorage keys
   - History: track visited nodes, show path on demand
   - Keyboard shortcuts: 1-9 for choices, S for save, L for load

4. **UI polish**:
   - Title screen with story name and "开始游戏" / "读取存档" buttons
   - Text appears with a subtle fade-in
   - Choice buttons slide in from below
   - Ending overlay with ending title and type label
   - "再玩一次" button on ending screen

### HTML Game UI Guidelines

- Default dark theme: `background: #1a1a2e`, text: `#e0e0e0`, choices: `#16213e` with `#0f3460` hover
- Font: `'Microsoft YaHei', 'Noto Sans SC', sans-serif` for CJK; `16px` base size
- Main text area: max-width `800px`, centered, line-height `1.8`
- Choice buttons: full width within container, `12px` padding, rounded corners
- Mobile responsive: single-column layout, touch-friendly button sizes (min `44px` height)
- Save/Load: small toolbar at bottom, unobtrusive icons

## Story Nodes JSON Schema

The `story_nodes.json` is the single source of truth driving both the story document and all game formats. Its structure is defined in `references/story_schema.md`. All game formats parse their respective version of this data at runtime.

Additional fields for this skill:
- `perspective` (string, optional): For multi-perspective stories, the OC name whose POV this node is from. Games use this to display "Playing as [OC Name]".

At a minimum, the JSON looks like:

```json
{
  "title": "Story Title",
  "perspective_mode": "single",
  "nodes": [
    {
      "id": "start",
      "text": "The story begins here...",
      "perspective": "林星河",
      "choices": [
        {"text": "Do this", "nextNode": "node_a"},
        {"text": "Do that", "nextNode": "node_b"}
      ]
    },
    {
      "id": "ending_good",
      "text": "You succeeded!",
      "isEnding": true,
      "endingType": "good"
    }
  ]
}
```

## Interaction Principles

- **Phase gating** — always get user approval before moving to the next phase. Never skip ahead.
- **Show don't assume** — after each generation step, show a summary and ask for confirmation.
- **User is the author** — AI is a co-creator. The user has final say on all creative decisions.
- **Handle corrections gracefully** — if the user wants to change something in an earlier phase, go back and revise, then cascade changes forward.
- **Code quality** — generated C# must compile. Include `.csproj` files. Use `dotnet build` to verify before presenting to user.
- **HTML must be self-contained** — no external CDN links, no npm dependencies. Everything in one file + one data file.
- **CJK support** — all text output and UI controls must handle Chinese characters. Save files use UTF-8.
- **Respect game format choices** — only generate the formats the user selected. Don't waste time building what isn't wanted.

## Quick Start (for the AI)

When the skill triggers:
1. Ask: "What's your project name, and do you have any existing OC/world-building content?"
2. Determine starting phase based on user's answer
3. **Before writing anything, browse `references/example/`** to see the expected quality, structure, and format for each phase's output. Use the example files as templates.
4. Before writing any file, create the full directory tree under `interactive_story_output/<project-name>/`
5. In Phase 1, guide the user to create multiple OCs and assign scene roles
6. In Phase 2, clarify single vs multi-perspective. **Default to HTML only for game format** unless the user explicitly asks for Console or WinForms.
7. Follow the phase sequence, getting approval at each step
8. After all selected games are complete: "Your project is ready. Here's how to play: [list per-format instructions]."
