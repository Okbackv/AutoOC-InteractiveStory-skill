# Story Nodes JSON Schema

The `story_nodes.json` file is the single source of truth for the interactive story. Both the story documentation and the C# application parse it.

## Top-Level Structure

```json
{
  "title": "string — the story title",
  "author": "string — author name (optional)",
  "version": "string — semantic version (optional, e.g. '1.0')",
  "perspective_mode": "string — 'single' or 'multi'",
  "protagonist_oc": "string — OC name (single perspective) or null (multi)",
  "nodes": [ ... ]
}
```

## Node Object

Each node represents one scene / passage in the story.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | string | yes | Unique identifier, e.g. `"start"`, `"forest_crossroads"`, `"ending_good"` |
| `title` | string | no | Short scene name for display in status bar, e.g. `"The Dark Forest"` |
| `text` | string | yes | Narrative text. Use `\n\n` for paragraph breaks. |
| `choices` | array | only if not ending | Array of Choice objects |
| `isEnding` | boolean | no | Mark as `true` for ending nodes |
| `endingType` | string | only if ending | One of: `"good"`, `"bad"`, `"neutral"`, `"true"`, `"secret"` |
| `endingTitle` | string | no | Display title for the ending, e.g. `"The Hero's Sacrifice"` |
| `condition` | object | no | Condition that must be met for this node to be reachable |
| `effects` | object | no | Effects applied to GameState when entering this node |
| `onEnter` | string | no | Optional script/action executed on entering (future use) |
| `perspective` | string | only if multi-POV | The OC name whose point of view this node is from. Required for all non-ending nodes when `perspective_mode` is `"multi"`. |

### Choice Object

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `text` | string | yes | Text shown to player for this choice |
| `nextNode` | string | yes | `id` of the node this choice leads to |
| `condition` | object | no | Condition required for this choice to appear |
| `tooltip` | string | no | Extra hint shown on hover (WinForms only) |

## Condition System

Conditions use simple key-value checks against `GameState.Flags`.

```json
{
  "condition": {
    "has_sword": true,
    "reputation": { "min": 5 },
    "chapter": { "equals": 2 }
  }
}
```

Operators: direct value equality, `min`, `max`, `equals`, `not`, `oneOf`

Multiple keys = ALL must match (AND logic).

## Effects System

Effects modify `GameState.Flags` when entering a node.

```json
{
  "effects": {
    "has_sword": true,
    "reputation": { "add": 1 },
    "hp": { "subtract": 10 },
    "companions": { "append": "Luna" },
    "visited_forest": true
  }
}
```

Operators: direct set, `add`, `subtract`, `append`, `remove`, `toggle`

## Complete Example

```json
{
  "title": "星落之夜的抉择",
  "author": "AI & User",
  "version": "1.0",
  "perspective_mode": "single",
  "protagonist_oc": "林星河",
  "nodes": [
    {
      "id": "start",
      "title": "星落之夜",
      "text": "流星划过天际的那个夜晚，你站在学院的高塔上，看着远方燃烧的村庄。\n\n寒风呼啸着穿过塔楼的石窗。你的导师艾琳站在你身后，她的声音低沉而急迫：\"你必须做出选择。\"\n\n楼下传来追兵的脚步声。",
      "choices": [
        {"text": "\"我去村子救人！\" — 冲向楼梯", "nextNode": "village_rescue"},
        {"text": "\"我们先找出真相。\" — 跟随导师前往图书馆", "nextNode": "library_search"},
        {"text": "\"谁也别想走。\" — 转身面对追兵", "nextNode": "confront_guards"}
      ]
    },
    {
      "id": "village_rescue",
      "title": "燃烧的村庄",
      "text": "你冲进火海，浓烟呛得你几乎睁不开眼。在倒塌的房屋之间，你听到了微弱的哭声。\n\n一个孩子被困在燃烧的房屋里，而一旁的粮仓也在坍塌边缘。你只有时间救一个。",
      "choices": [
        {"text": "救出孩子", "nextNode": "rescue_child"},
        {"text": "抢救粮仓 — 全村人的过冬粮食", "nextNode": "rescue_grain"},
        {"text": "呼喊导师使用冰霜魔法，尝试两全", "nextNode": "ice_magic_rescue",
         "condition": {"learned_ice_magic": true}}
      ],
      "effects": {"visited_village": true}
    },
    {
      "id": "rescue_child",
      "text": "你抱着孩子冲出火海，身后房屋轰然倒塌。粮仓在烈火中化为灰烬。\n\n孩子的母亲跪在你面前，泪流满面。村民们的目光中既有感激，也有对寒冬的恐惧。",
      "choices": [
        {"text": "继续旅程", "nextNode": "road_ahead"}
      ],
      "effects": {
        "saved_child": true,
        "reputation": {"add": 3},
        "grain_saved": false
      }
    },
    {
      "id": "ending_hero",
      "text": "你做到了。不是用魔法，不是用计谋，而是用坚定不移的心。\n\n当你最终站在学院废墟前，朝阳恰好升起。那些跟随你一路走来的人们，站在你身后。\n\n艾琳轻轻拍了拍你的肩膀：\"你比你想象的要强大得多。\"\n\n一个新的时代开始了。而这一次，你不再是旁观者。",
      "isEnding": true,
      "endingType": "good",
      "endingTitle": "英雄的黎明"
    },
    {
      "id": "ending_sacrifice",
      "text": "你倒在了最后一战中。\n\n但你的牺牲换来了村庄的安全，换来了真相的大白。在你的墓碑上，只刻着一句话：\n\n\"这里长眠着一个选择了正确的路的人。\"\n\n风吹过原野，人们永远不会忘记你的名字。",
      "isEnding": true,
      "endingType": "neutral",
      "endingTitle": "最后的牺牲"
    }
  ]
}
```

## Validation Rules

1. `"start"` node must exist
2. Every `nextNode` must reference a valid node `id`
3. Every path from `"start"` must eventually reach an `isEnding: true` node
4. No unreachable nodes (warn but allow)
5. Node `id` values must be unique
