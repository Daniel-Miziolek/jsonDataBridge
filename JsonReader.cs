namespace JsonDataBridge;

public class JsonReader
{
    private readonly string _text;
    private int _pos;

    public JsonReader(string text) => _text = text;

    public char Peek() => _pos < _text.Length ? _text[_pos] : '\0';
    public char Next() => _pos < _text.Length ? _text[_pos++] : '\0';
    public void SkipWhitespace()
    {
        while (char.IsWhiteSpace(Peek())) _pos++;
    }

    public bool Match(string expected)
    {
        for (int i = 0; i < expected.Length; i++)
        {
            if (_pos + i >= _text.Length || _text[_pos + i] != expected[i])
                return false;
        }

        _pos += expected.Length;
        return true;
    }
}