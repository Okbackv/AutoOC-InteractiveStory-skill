# Auto OC Interactive Story

> Full-pipeline interactive story generation skill: from character creation to playable games

## What is this?

A **skill** for Claude Code that guides you and the AI through co-creating:

1. **Original Characters (OCs)** — multiple OCs with scene roles (main cast / supporting / NPC / background)
2. **World-Building** — magic systems, politics, geography, history — fully customizable
3. **Branching Stories** — multi-ending linear narratives with condition/effect systems, single or multi-POV
4. **Playable Games** — one-click HTML browser game (recommended), C# console app, C# WinForms app

## Quick Start

In Claude Code:

```
调用 auto_OC_InteractiveStory skill
```

Then follow the AI's guidance through 7 phases (you can start from any phase).

## Skill Structure

```
auto-oc-interactive-story/
├── SKILL.md                    # Main skill file (complete 7-phase guide)
├── README.md                   # This file (Chinese)
├── README-en_US.md             # English version (you are here)
├── references/
│   ├── oc_schema.md            # OC profile JSON schema
│   ├── story_schema.md         # Story nodes JSON schema
│   └── example/                # ⭐ Complete reference example
│       ├── 01_角色设定/         # 3 OC profiles
│       ├── 02_世界观设定/       # World setting
│       ├── 03_故事风格与互动设计/# Style preferences & OC participation
│       ├── 04_故事结构设计/     # 15-node / 3-ending story graph
│       ├── 05_故事文档/         # Full narrative (~9,000 characters)
│       ├── 06_控制台游戏/       # C# console app
│       ├── 07_WinForms游戏/     # C# WinForms desktop app
│       ├── 08_HTML游戏/         # Browser game (recommended)
│       └── README.md
```

## The 7 Phases

| Phase | What | Output |
|-------|------|--------|
| Phase 1 | OC creation + world-building | OC profiles (md/json), world setting (md/json) |
| Phase 2 | Style, perspective & game format selection | Style preferences JSON, design notes |
| Phase 3 | Branching story structure design | Branching outline, story nodes JSON |
| Phase 4 | Full story writing | Complete narrative with all branches |
| Phase 5 | Console game (optional) | C# .NET console project |
| Phase 6 | WinForms game (optional) | C# .NET WinForms project |
| Phase 7 | HTML game (⭐ strongly recommended) | Self-contained HTML + JS browser game |

## Key Features

- **Flexible sourcing** — OCs and worlds can be user-provided, AI-generated, or mixed
- **Multi-POV support** — single perspective or multi-character switching narratives
- **OC participation design** — assign scene roles and appearance weights to every OC
- **Interaction weight system** — customize Puzzle / Combat / Social / Exploration / Moral Dilemma distribution
- **Story length + choice density** — short/medium/long scale × dense/moderate/sparse choices
- **Condition + effects system** — track player choices, unlock conditional paths and endings
- **Three game formats** — HTML (recommended), Console, WinForms
- **Save/load** — supported in all formats

## Reference Example

`references/example/` contains a complete worked example — **"星落之夜的抉择" (The Choice on the Night of Falling Stars)**:

- Fantasy-mystery genre, 15 scene nodes, 3 endings
- 3 OCs, single-POV narrative
- All three game formats implemented and ready to compile/run

## Requirements

- Claude Code (any version)
- .NET 9.0 SDK (only needed for Console/WinForms generation)
- Modern browser (HTML game requires zero installation)
