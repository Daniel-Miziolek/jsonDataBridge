using Spectre.Console;

namespace JsonDataBridge
{
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
                            .Title("Choose file to read")
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
                        if (IsContainGoodExension(folderOrFile))
                        {
                            var txtInFile = File.ReadAllText(folderOrFile);
                            JsonValue value = JsonParser.Parse(txtInFile);
                            JsonTreePrinter.Print(value);
                        }
                        else
                        {
                            Console.WriteLine("Wrong file. Allowed extensions {JSON}");
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

        static bool IsContainGoodExension(string path)
        {
            string[] allowedExtensions = [".json"];
            return allowedExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }
    }
}