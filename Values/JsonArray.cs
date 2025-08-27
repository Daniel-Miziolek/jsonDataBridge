namespace JsonDataBridge.Values;

public class JsonArray : JsonValue
{
    public List<JsonValue> Items { get; } = new();

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("[ ");
        for (int i = 0; i < Items.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(Items[i]);
        }
        sb.Append(" ]");
        return sb.ToString();
    }
}