using System.Diagnostics;

const string WorkingDirectory = @"D:\PROJECTS\PRACTICAL PROJECT\IRISA\server\Migrators\Irisa.Identity.Migrator";

Console.WriteLine("Command Executor");
Console.WriteLine("---------------");
Console.WriteLine($"Working Directory: {WorkingDirectory}");
Console.WriteLine("Type 'exit' to quit the application");
Console.WriteLine();

while (true)
{
    try
    {
        Console.Write("Enter command > ");
        string? command = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(command))
        {
            Console.WriteLine("Please enter a valid command.");
            continue;
        }

        if (command.ToLower() == "exit")
        {
            break;
        }

        ExecuteCommand(command);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
static void ExecuteCommand(string command)
{
    var processInfo = new ProcessStartInfo
    {
        FileName = "cmd.exe",
        Arguments = $"/c {command}",
        WorkingDirectory = WorkingDirectory,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = new Process
    {
        StartInfo = processInfo
    };

    process.OutputDataReceived += (sender, args) =>
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Console.WriteLine(args.Data);
        }
    };

    process.ErrorDataReceived += (sender, args) =>
    {
        if (!string.IsNullOrEmpty(args.Data))
        {
            Console.WriteLine($"Error: {args.Data}");
        }
    };

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        Console.WriteLine($"Command exited with code: {process.ExitCode}");
    }
}