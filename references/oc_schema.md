# OC Profile JSON Schema

Used in `docs/oc_profiles.json`. Defines structured data for each Original Character.

## Top-Level Structure

```json
{
  "project_name": "string",
  "ocs": [ ... ]
}
```

## OC Object

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `name` | string | yes | Character name |
| `gender` | string | yes | Male / Female / Other |
| `age` | number | yes | Age in years |
| `mbti` | string | no | MBTI type, e.g. "INTJ", "ENFP" |
| `birthday` | string | no | Birthday, any format |
| `height` | string | no | Height, e.g. "175cm" |
| `appearance` | string | yes | Physical description |
| `personality` | string | yes | Personality traits and characteristics |
| `speaking_style` | string | yes | How they talk — tone, pace, vocabulary, catchphrases |
| `background` | string | yes | Backstory and history |
| `hobbies` | string[] | no | List of hobbies |
| `catchphrases` | string[] | no | Signature phrases |
| `likes` | string[] | no | Things they like |
| `dislikes` | string[] | no | Things they dislike |
| `abilities` | string[] | no | Special skills, powers, talents |
| `occupation` | string | no | Job or role |
| `relationships` | object[] | no | Connections to other OCs |
| `role_in_story` | string | no | Protagonist / Antagonist / Supporting / etc. |
| `scene_role` | string | yes | One of: `"main_cast"`, `"key_supporting"`, `"regular_npc"`, `"background"` — determines how often this OC appears |
| `playable` | boolean | no | Whether the player can control this OC (relevant for multi-perspective mode). Default: true for main_cast, false for others |

### Relationship Object

```json
{
  "target": "string — name of the other OC",
  "type": "string — e.g. 'friend', 'rival', 'mentor', 'family', 'romantic'",
  "description": "string — nature of the relationship"
}
```

## Complete Example

```json
{
  "project_name": "星落传奇",
  "ocs": [
    {
      "name": "林星河",
      "gender": "男",
      "age": 19,
      "mbti": "INFJ",
      "birthday": "11月7日",
      "height": "178cm",
      "appearance": "黑发黑瞳，眉目清秀，常穿灰色法师袍，左手有一道星形伤疤",
      "personality": "内敛温柔，意志坚定，在关键时刻异常果断。不擅长拒绝别人，但对自己的信念绝不妥协。内心深处渴望被认可。",
      "speaking_style": "语气温和，语速偏慢，思考时会短暂停顿。不常说笑，但偶尔会冷不丁冒出一句很冷的话。习惯性说'嗯……'开头。",
      "background": "孤儿，被魔法学院院长艾琳收养。从小展现星象魔法的天赋，但因为一次失败的预言导致同村受灾，一直心怀愧疚。",
      "hobbies": ["观星", "阅读古籍", "吹笛"],
      "catchphrases": ["嗯……让我想想", "星星会告诉我们答案"],
      "likes": ["星空", "安静的地方", "甜食", "古籍的气味"],
      "dislikes": ["无谓的争斗", "嘈杂的人群", "谎言"],
      "abilities": ["星象预言（不稳定）", "基础星魔法", "古籍解读"],
      "occupation": "魔法学院学徒",
      "relationships": [
        {"target": "艾琳", "type": "mentor", "description": "养母兼导师，敬重但偶有理念冲突"},
        {"target": "卡修斯", "type": "rival", "description": "同为学徒，实力相当，互相看不顺眼又互相欣赏"}
      ],
      "role_in_story": "protagonist",
      "scene_role": "main_cast",
      "playable": true
    },
    {
      "name": "艾琳",
      "gender": "女",
      "age": 42,
      "mbti": "INTJ",
      "birthday": "3月21日",
      "height": "165cm",
      "appearance": "银白长发，冰蓝色眼眸，面容冷峻，身材纤细但气场强大，身穿深蓝星空法袍",
      "personality": "理智至上，计划周密，不轻易表露情感。对学术追求近乎偏执，但内心深处对学生有着隐藏的温情。",
      "speaking_style": "语调平稳冷静，措辞精准，从不废话。表达不满时不会提高音量，而是用更冷的语气和更复杂的词汇。",
      "background": "帝国最年轻的魔法学院院长，冰系魔法大师。曾有一位得意门生因魔法实验事故丧生，成为她心中的隐痛。",
      "hobbies": ["魔法研究", "下棋", "栽培冰晶花"],
      "catchphrases": ["逻辑胜过热情", "再来一次"],
      "likes": ["秩序", "知识", "冰晶花茶", "沉默的学生"],
      "dislikes": ["混乱", "感情用事", "政治斗争", "无意义的社交"],
      "abilities": ["冰系魔法大师", "远古魔法解密", "战术指挥"],
      "occupation": "魔法学院院长",
      "relationships": [
        {"target": "林星河", "type": "mentor", "description": "养子兼学生，在他身上看到了已故学生的影子"}
      ],
      "role_in_story": "supporting",
      "scene_role": "key_supporting",
      "playable": false
    }
  ]
}
```
