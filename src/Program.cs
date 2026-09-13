using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using codecrafters_git.Helpers;

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

        var parentDir = BlobPathHelper.GetParentDirFromHash(hash);
        var remaining = BlobPathHelper.GetFileDirFromHash(hash);

        Debug.Assert(remaining != null, nameof(remaining) + " != null");
        Debug.Assert(parentDir != null, nameof(parentDir) + " != null");
        ZlibHelper.DecompressFile(Path.Combine(Path.Combine(objectsDirectory, parentDir), remaining));
        break;
    }
    case "hash-object":
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("cat-file requires 2 arguments");
        }

        var flag = args.Skip(1).First();
        var filePath = args.Skip(2).First();
        var hash = FileHelper.ComputeSha1(filePath);

        if (!flag.Equals("-w"))
        {
            throw new ArgumentException("hash-object only supports the '-w' flag");
        }

        var parentDir = BlobPathHelper.GetParentDirFromHash(hash);
        var remaining = BlobPathHelper.GetFileDirFromHash(hash);

        Debug.Assert(remaining != null, nameof(remaining) + " != null");
        Debug.Assert(parentDir != null, nameof(parentDir) + " != null");
        ZlibHelper.CompressFile(Path.Combine(Path.Combine(objectsDirectory, parentDir), remaining),
            FileHelper.GetFileContent(filePath));
        break;
    }
    default:
        throw new ArgumentException($"Unknown command {command}");
}