using System;
using System.IO;
using System.IO.Compression;

const string objectsDirectory = ".git/objects";

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

string command = args[0];

switch(command)
{
    case "init":
    {
        Directory.CreateDirectory(".git");
        Directory.CreateDirectory(objectsDirectory);
        Directory.CreateDirectory(".git/refs");
        File.WriteAllText(".git/HEAD", "ref: refs/heads/main\n");
        Console.WriteLine("Initialized git directory");
        break;
    }
    case "cat-file":
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("cat-file requires 2 arguments");
        }

        var flag = args.Skip(1).ToString() ?? "";
        var hash = args.Skip(2).ToString() ?? "";

        if (!flag.Equals("-p"))
        {
            throw new ArgumentException("cat-file only supports the '-p' flag");
        }
       
        DecompressFile(Path.Combine(objectsDirectory, hash));
        break;
    }
    default :
        throw new ArgumentException($"Unknown command {command}");
}

void DecompressFile(string compressedFile)
{
    using (FileStream compressedStream = File.OpenRead(compressedFile))
    using (ZLibStream decompressionStream = new ZLibStream(compressedStream, CompressionMode.Decompress))
    using (StreamReader reader = new StreamReader(decompressionStream))
    {
        string fileContents = reader.ReadToEnd();
        Console.WriteLine(fileContents);
    }
}