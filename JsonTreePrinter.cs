using JsonDataBridge.Values;
using Spectre.Console;

namespace JsonDataBridge;

public static class JsonTreePrinter
{
    public static void Print(JsonValue value)
    {
        var tree = new Tree("[yellow]JSON[/]");
        BuildTree(value, tree);
        AnsiConsole.Write(tree);
    }

    private static void BuildTree(JsonValue value, Tree tree)
    {
        var root = tree.AddNode("[yellow]root[/]");
        BuildTree(value, root);
    }

    private static void BuildTree(JsonValue value, TreeNode node)
    {
        switch (value)
        {
            case JsonObject obj:
                foreach (var kvp in obj.Properties)
                {
                    var keyText = Escape(kvp.Key);
                    try
                    {
                        var keyNode = node.AddNode($"[blue]{keyText}[/]");
                        BuildTree(kvp.Value, keyNode);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding key: '{keyText}'");
                        Console.WriteLine($"Exception: {ex.Message}");
                        throw;
                    }
                }
                break;

            case JsonArray arr:
                int index = 0;
                foreach (var item in arr.Items)
                {
                    var indexText = Escape($"[{index}]");
                    var itemText = $"[green]{indexText}[/]";
                    try
                    {
                        var itemNode = node.AddNode(itemText);
                        BuildTree(item, itemNode);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding array element: '{itemText}'");
                        Console.WriteLine($"Exception: {ex.Message}");
                        throw;
                    }
                    index++;
                }
                break;

            case JsonString str:
                var strText = $"[white]\"{Escape(str.Value)}\"[/]";
                try
                {
                    node.AddNode(strText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding string: '{strText}'");
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw;
                }
                break;

            case JsonNumber num:
                var numText = $"[cyan]{num.Value}[/]";
                try
                {
                    node.AddNode(numText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding number: '{numText}'");
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw;
                }
                break;

            case JsonBool b:
                var boolText = b.Value ? "[green]true[/]" : "[red]false[/]";
                try
                {
                    node.AddNode(boolText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding boolean: '{boolText}'");
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw;
                }
                break;

            case JsonNull:
                var nullText = "[grey]null[/]";
                try
                {
                    node.AddNode(nullText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding null: '{nullText}'");
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw;
                }
                break;
        }
    }

    private static string Escape(string text)
    {
        return text
            .Replace("[", "[[")
            .Replace("]", "]]");
    }
}
