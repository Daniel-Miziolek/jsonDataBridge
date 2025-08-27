namespace JsonDataBridge.Values;

public class JsonObject : JsonValue
{
    public Dictionary<string, JsonValue> Properties { get; } = new();

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("{ ");
        bool first = true;
        foreach (var kvp in Properties)
        {
            if (!first) sb.Append(", ");
            sb.Append($"\"{kvp.Key}\": {kvp.Value}");
            first = false;
        }
        sb.Append(" }");
        return sb.ToString();
    }
}