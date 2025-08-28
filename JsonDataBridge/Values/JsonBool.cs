namespace JsonDataBridge.Values;

public class JsonBool : JsonValue
{
    public bool Value { get; set; }
    public JsonBool(bool value) => Value = value;

    public override string ToString() => Value ? "true" : "false";
}