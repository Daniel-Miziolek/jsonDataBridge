using JsonDataBridge;
using JsonDataBridge.Values;

namespace Tests;

public class ParserTest
{
    [Fact]
    public void Parse_String()
    {
        var json = "\"hello\"";
        var result = JsonParser.Parse(json);

        Assert.IsType<JsonString>(result);
        Assert.Equal("hello", ((JsonString)result).Value);
    }

    [Fact]
    public void Parse_Number()
    {
        var json = "123.45";
        var result = JsonParser.Parse(json);

        Assert.IsType<JsonNumber>(result);
        Assert.Equal(123.45, ((JsonNumber)result).Value);
    }

    [Fact]
    public void Parse_BooleanTrue()
    {
        var json = "true";
        var result = JsonParser.Parse(json);

        Assert.IsType<JsonBool>(result);
        Assert.True(((JsonBool)result).Value);
    }

    [Fact]
    public void Parse_Null()
    {
        var json = "null";
        var result = JsonParser.Parse(json);

        Assert.IsType<JsonNull>(result);
    }

    [Fact]
    public void Parse_Array()
    {
        var json = "[1, \"abc\", false]";
        var result = JsonParser.Parse(json);

        var array = Assert.IsType<JsonArray>(result);
        Assert.Equal(3, array.Items.Count);
        Assert.IsType<JsonNumber>(array.Items[0]);
        Assert.IsType<JsonString>(array.Items[1]);
        Assert.IsType<JsonBool>(array.Items[2]);
    }

    [Fact]
    public void Parse_Object()
    {
        var json = "{ \"name\": \"Daniel\", \"age\": 16 }";
        var result = JsonParser.Parse(json);

        var obj = Assert.IsType<JsonObject>(result);
        Assert.True(obj.Properties.ContainsKey("name"));
        Assert.True(obj.Properties.ContainsKey("age"));
        Assert.Equal("Daniel", ((JsonString)obj.Properties["name"]).Value);
        Assert.Equal(16, ((JsonNumber)obj.Properties["age"]).Value);
    }

    [Fact]
    public void Parse_InvalidJson()
    {
        var invalidJson = "{ \"unclosed\": [1, 2, 3 ";
        Assert.Throws<Exception>(() => JsonParser.Parse(invalidJson));
    }

    [Fact]
    public void Parse_EmptyObject()
    {
        var json = "{}";
        var result = JsonParser.Parse(json);

        var obj = Assert.IsType<JsonObject>(result);
        Assert.Empty(obj.Properties);
    }

    [Fact]
    public void Parse_EmptyArray()
    {
        var json = "[]";
        var result = JsonParser.Parse(json);

        var array = Assert.IsType<JsonArray>(result);
        Assert.Empty(array.Items);
    }
}