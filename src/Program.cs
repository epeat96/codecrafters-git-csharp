using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

const string objectsDirectory = ".git/objects";

StringBuilder sb = new();

if (args.Length < 1)
{
    Console.WriteLine("Please provide a command.");
    return;
}

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.Error.WriteLine("Logs from your program will appear here!");

string command = args[0];

switch (command)
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

        var flag = args.Skip(1).First();
        var hash = args.Skip(2).First();

        if (!flag.Equals("-p"))
        {
            throw new ArgumentException("cat-file only supports the '-p' flag");
        }

        var parentDir = sb.AppendJoin(hash.First(), hash.Skip(1).First()).ToString();
        Console.WriteLine($"ParentDir: {parentDir}");
        sb = sb.Clear();
        var remaining = sb.Insert(0, hash.Skip(2).ToList()).ToString();
        Console.WriteLine($"Remaining: {remaining}");

        Debug.Assert(remaining != null, nameof(remaining) + " != null");
        Debug.Assert(parentDir != null, nameof(parentDir) + " != null");
        DecompressFile(Path.Combine(Path.Combine(objectsDirectory, parentDir), remaining));
        break;
    }
    default:
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