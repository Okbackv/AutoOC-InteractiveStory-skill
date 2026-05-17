using System.Text.Json;

namespace InteractiveStory;

public class StoryEngine
{
    private readonly StoryData _data;
    private readonly GameState _state;

    public StoryEngine(StoryData data, GameState state)
    {
        _data = data;
        _state = state;
    }

    public GameState State => _state;

    public StoryNode CurrentNode => _data.GetNode(_state.CurrentNodeId);

    public string Title => _data.Title;

    public bool IsEnding => CurrentNode.IsEnding;

    public string? EndingType => CurrentNode.EndingType;

    public string? EndingTitle => CurrentNode.EndingTitle;

    public List<Choice> GetAvailableChoices()
    {
        if (CurrentNode.Choices == null)
            return new List<Choice>();

        return CurrentNode.Choices
            .Where(c => EvaluateCondition(c.Condition))
            .ToList();
    }

    public void Navigate(string nextNodeId)
    {
        if (!_data.HasNode(nextNodeId))
            throw new InvalidOperationException($"目标节点不存在: {nextNodeId}");

        _state.History.Add(_state.CurrentNodeId);
        _state.CurrentNodeId = nextNodeId;

        ApplyEffects();
    }

    public void MakeChoice(int choiceIndex)
    {
        var choices = GetAvailableChoices();
        if (choiceIndex < 0 || choiceIndex >= choices.Count)
            throw new ArgumentOutOfRangeException(nameof(choiceIndex), "无效的选择序号。");

        var choice = choices[choiceIndex];
        _state.ChoicesMade.Add(new ChoiceRecord
        {
            NodeId = _state.CurrentNodeId,
            ChoiceIndex = choiceIndex
        });

        Navigate(choice.NextNode);
    }

    private bool EvaluateCondition(Dictionary<string, object>? condition)
    {
        if (condition == null || condition.Count == 0)
            return true;

        foreach (var (key, expectedValue) in condition)
        {
            if (!_state.Flags.TryGetValue(key, out var actualValue))
                return false;

            if (expectedValue is JsonElement elem)
            {
                if (elem.ValueKind == JsonValueKind.Object)
                {
                    if (elem.TryGetProperty("min", out var min))
                    {
                        var actualNum = GetNumberValue(actualValue);
                        if (actualNum < min.GetDouble()) return false;
                    }
                    if (elem.TryGetProperty("max", out var max))
                    {
                        var actualNum = GetNumberValue(actualValue);
                        if (actualNum > max.GetDouble()) return false;
                    }
                    if (elem.TryGetProperty("equals", out var eq))
                    {
                        if (!ValuesEqual(actualValue, eq)) return false;
                    }
                    continue;
                }
            }

            if (!ValuesEqual(actualValue, expectedValue))
                return false;
        }

        return true;
    }

    private void ApplyEffects()
    {
        var effects = CurrentNode.Effects;
        if (effects == null) return;

        foreach (var (key, value) in effects)
        {
            if (value is JsonElement elem)
            {
                if (elem.ValueKind == JsonValueKind.Object)
                {
                    if (elem.TryGetProperty("add", out var add))
                    {
                        var current = _state.GetIntFlag(key);
                        _state.SetFlag(key, current + add.GetInt32());
                        continue;
                    }
                    if (elem.TryGetProperty("subtract", out var sub))
                    {
                        var current = _state.GetIntFlag(key);
                        _state.SetFlag(key, current - sub.GetInt32());
                        continue;
                    }
                    if (elem.TryGetProperty("append", out var append))
                    {
                        // Handle append to list — for simplicity, set as flag
                        _state.SetFlag(key, append.GetString() ?? "");
                        continue;
                    }
                }
            }

            if (value is JsonElement boolElem && boolElem.ValueKind == JsonValueKind.True)
                _state.SetFlag(key, true);
            else if (value is JsonElement boolElem2 && boolElem2.ValueKind == JsonValueKind.False)
                _state.SetFlag(key, false);
            else if (value is JsonElement numElem && numElem.ValueKind == JsonValueKind.Number)
                _state.SetFlag(key, numElem.GetInt32());
            else
                _state.SetFlag(key, value);
        }
    }

    private static bool ValuesEqual(object actual, object expected)
    {
        if (expected is JsonElement expElem)
        {
            if (expElem.ValueKind == JsonValueKind.True)
                return GetBoolValue(actual);
            if (expElem.ValueKind == JsonValueKind.False)
                return !GetBoolValue(actual);
            if (expElem.ValueKind == JsonValueKind.Number)
                return Math.Abs(GetNumberValue(actual) - expElem.GetDouble()) < 0.001;
            if (expElem.ValueKind == JsonValueKind.String)
            {
                var actualStr = actual is JsonElement ae ? ae.GetString() : actual?.ToString();
                return string.Equals(actualStr, expElem.GetString(), StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        return Equals(actual, expected);
    }

    private static bool GetBoolValue(object value)
    {
        if (value is JsonElement je && je.ValueKind == JsonValueKind.True)
            return true;
        if (value is bool b)
            return b;
        return false;
    }

    private static double GetNumberValue(object value)
    {
        if (value is JsonElement je && je.ValueKind == JsonValueKind.Number)
            return je.GetDouble();
        if (value is int i) return i;
        if (value is double d) return d;
        if (value is long l) return l;
        return 0;
    }
}
