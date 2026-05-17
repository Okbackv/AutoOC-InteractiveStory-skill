# Auto OC Interactive Story

> 从角色创建到可玩游戏的全流程互动故事生成技能

[Switch to English](https://github.com/Okbackv/AutoOC-InteractiveStory-skill/blob/main/README.en-US.md)

## 这是什么？

这是一个为 Claude Code 设计的 **skill**，它能引导你和 AI 共同完成：

1. **创建原创角色（OC）** — 支持多个角色，分配主角团/关键配角/路人等场景角色
2. **构建世界观** — 魔法体系、政治格局、地理历史，全可自定义
3. **设计分支故事** — 多结局线性叙事，条件/效果系统，支持单一或多视角
4. **生成可玩游戏** — 一键生成 HTML 网页游戏（推荐）、C# 控制台应用、C# WinForms 窗口应用

## 快速开始

在 Claude Code 中调用：

```
调用 auto_OC_InteractiveStory skill
```

然后按照 AI 引导，依次完成 7 个阶段（你可以从任意阶段开始）。

## Skill 结构

```
auto-oc-interactive-story/
├── SKILL.md                    # 主技能文件（7阶段完整指南）
├── README.md                   # 本文件
├── README-en_US.md             # English version
├── references/
│   ├── oc_schema.md            # OC角色档案 JSON 结构定义
│   ├── story_schema.md         # 故事节点 JSON 结构定义
│   └── example/                # ⭐ 完整参考示例
│       ├── 01_角色设定/         # 3个OC的完整档案
│       ├── 02_世界观设定/       # 魔法学院世界观
│       ├── 03_故事风格与互动设计/# 风格偏好、视角、OC参与度
│       ├── 04_故事结构设计/     # 15节点/3结局的分支图
│       ├── 05_故事文档/         # 完整可读故事（约9000字）
│       ├── 06_控制台游戏/       # C# 控制台应用
│       ├── 07_WinForms游戏/     # C# WinForms 桌面应用
│       ├── 08_HTML游戏/         # 浏览器游戏（推荐）
│       └── README.md
```

## 7 个阶段

| 阶段 | 内容 | 产出 |
|------|------|------|
| Phase 1 | OC 角色创建 + 世界观构建 | 角色档案（md/json）、世界观设定（md/json） |
| Phase 2 | 风格偏好、视角设计、游戏格式选择 | 风格偏好 JSON、设计笔记 |
| Phase 3 | 分支故事结构设计 | 分支大纲、故事节点 JSON |
| Phase 4 | 完整故事撰写 | 含所有分支的完整故事文档 |
| Phase 5 | 控制台游戏生成（可选） | C# .NET 控制台项目 |
| Phase 6 | WinForms 游戏生成（可选） | C# .NET WinForms 项目 |
| Phase 7 | HTML 游戏生成（⭐ 强烈推荐） | 自包含 HTML + JS 浏览器游戏 |

## 特色功能

- **灵活来源** — 角色和世界观可以由你提供、AI 生成、或两者混合
- **多视角支持** — 可选单一视角或多角色切换视角叙事
- **OC 参与度设计** — 每个角色分配场景角色和出场权重
- **互动权重系统** — 解谜/战斗/社交/探索/抉择 五维度定制
- **故事长度 + 选项密集度** — 短/中/长篇 + 密集/适中/稀疏
- **条件 + 效果系统** — 追踪玩家选择，影响后续可选路径和结局
- **三种游戏格式** — HTML（推荐）、Console、WinForms
- **存档/读档** — 所有格式均支持

## 参考示例

`references/example/` 包含一个完整的参考项目——**《星落之夜的抉择》**：

- 奇幻悬疑题材，15个场景节点，3个结局
- 3个OC（林星河/艾琳/卡修斯），单一视角叙事
- 三种游戏格式均已实现，可直接编译运行

## 要求

- Claude Code（任何版本）
- .NET 9.0 SDK（仅生成 Console/WinForms 时需要）
- 现代浏览器（HTML 游戏无需任何安装）
