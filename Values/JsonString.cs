public class JsonString : JsonValue
{
    public string Value { get; set; }
    public JsonString(string value) => Value = value;

    public override string ToString() => $"\"{Value}\"";
}