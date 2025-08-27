using JsonDataBridge.Values;

namespace JsonDataBridge;

public static class JsonParser
{
    public static JsonValue Parse(string json)
    {
        var reader = new JsonReader(json);
        reader.SkipWhitespace();
        return ParseValue(reader);
    }

    private static JsonValue ParseValue(JsonReader reader)
    {
        reader.SkipWhitespace();
        char c = reader.Peek();

        return c switch
        {
            '{' => ParseObject(reader),
            '[' => ParseArray(reader),
            '"' => ParseString(reader),
            't' or 'f' => ParseBool(reader),
            'n' => ParseNull(reader),
            _ => ParseNumber(reader)
        };
    }

    private static JsonObject ParseObject(JsonReader reader)
    {
        var obj = new JsonObject();
        reader.Next();
        reader.SkipWhitespace();

        while (reader.Peek() != '}')
        {
            reader.SkipWhitespace();
            var key = ((JsonString)ParseString(reader)).Value;

            reader.SkipWhitespace();
            if (reader.Next() != ':') throw new Exception("Expected ':'");

            var value = ParseValue(reader);
            obj.Properties[key] = value;

            reader.SkipWhitespace();
            if (reader.Peek() == ',')
            {
                reader.Next(); // skip ,
                reader.SkipWhitespace();
            }
            else
            {
                break;
            }
        }

        if (reader.Next() != '}') throw new Exception("Expected '}'");
        return obj;
    }

    private static JsonArray ParseArray(JsonReader reader)
    {
        var arr = new JsonArray();
        reader.Next();
        reader.SkipWhitespace();

        while (reader.Peek() != ']')
        {
            var value = ParseValue(reader);
            arr.Items.Add(value);

            reader.SkipWhitespace();
            if (reader.Peek() == ',')
            {
                reader.Next();
                reader.SkipWhitespace();
            }
            else
            {
                break;
            }
        }

        if (reader.Next() != ']') throw new Exception("Expected ']'");
        return arr;
    }

    private static JsonString ParseString(JsonReader reader)
    {
        var sb = new System.Text.StringBuilder();
        if (reader.Next() != '"') throw new Exception("Expected '\"'");

        while (true)
        {
            char c = reader.Next();

            if (c == '"') break;
            if (c == '\0') throw new Exception("Expected '\"'");
            if (c == '\\')
            {
                c = reader.Next();
                if (c == '\0')
                {
                    throw new Exception("Unterminated escape sequence in string");
                }
            }
            sb.Append(c);
        }

        return new JsonString(sb.ToString());
    }

    private static JsonBool ParseBool(JsonReader reader)
    {
        if (reader.Match("true"))
        {
            return new JsonBool(true);
        }
        else if (reader.Match("false"))
        {
            return new JsonBool(false);
        }
        else
        {
            throw new Exception("Invalid boolean value");
        }
    }

    private static JsonNull ParseNull(JsonReader reader)
    {
        if (reader.Match("null"))
        {
            return new JsonNull();
        }
        else
        {
            throw new Exception("Invalid null value");
        }
    }

    private static JsonNumber ParseNumber(JsonReader reader)
    {
        var sb = new System.Text.StringBuilder();

        char c = reader.Peek();
        if (c == '-')
        {
            sb.Append(reader.Next());
            c = reader.Peek();
        }

        while (char.IsDigit(c) || c == '.' || c == 'e' || c == 'E' || c == '+' || c == '-')
        {
            sb.Append(reader.Next());
            c = reader.Peek();
        }

        if (double.TryParse(sb.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result))
        {
            return new JsonNumber(result);
        }
        else
        {
            throw new Exception($"Invalid number: {sb}");
        }
    }
}