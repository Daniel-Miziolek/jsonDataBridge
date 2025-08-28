using JsonDataBridge.Values;
using Spectre.Console;

namespace JsonDataBridge;

class Program
{
    static void Main()
    {
        string path = ChooseDrive();
        Stack<string> pathHistory = new Stack<string>();

        while (true)
        {
            try
            {
                var entries = Directory.GetFileSystemEntries(path).ToList();

                entries.Insert(0, "Go back");
                entries.Insert(1, "Exit");

                var folderOrFile = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select a file to create the data bridge")
                        .MoreChoicesText("[grey](Move up and down to reveal more files)[/]")
                        .AddChoices(entries)
                );

                if (Directory.Exists(folderOrFile))
                {
                    pathHistory.Push(path);
                    path = folderOrFile;
                }
                else if (File.Exists(folderOrFile))
                {
                    Console.WriteLine($"Choosen file: {folderOrFile}");
                    if (IsJsonFile(folderOrFile))
                    {
                        var txtInFile = File.ReadAllText(folderOrFile);
                        JsonValue value = JsonParser.Parse(txtInFile);
                        JsonTreePrinter.Print(value);
                    }
                    else
                    {
                        Console.WriteLine("Wrong file. Allowed extension: .json");
                    }
                    break;
                }
                else if (folderOrFile == "Go back")
                {
                    if (pathHistory.Count > 0)
                    {
                        path = pathHistory.Pop();
                    }
                    else
                    {
                        return;
                    }
                }
                else if (folderOrFile == "Exit")
                {
                    pathHistory.Clear();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                break;
            }
        }
    }

    static string ChooseDrive()
    {
        var drives = DriveInfo.GetDrives()
            .Where(d => d.IsReady)
            .Select(d => d.Name)
            .ToList();

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a drive to start browsing:")
                .PageSize(10)
                .AddChoices(drives)
        );
    }

    static bool IsJsonFile(string path)
    {
        return string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase);
    }
}