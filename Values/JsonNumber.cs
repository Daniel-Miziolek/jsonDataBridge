public class JsonNumber : JsonValue
{
    public double Value { get; set; }
    public JsonNumber(double value) => Value = value;

    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}