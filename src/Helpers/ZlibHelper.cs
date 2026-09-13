using System.IO.Compression;

namespace codecrafters_git.Helpers;

public static class ZlibHelper
{
    public static void DecompressFile(string compressedFile)
    {
        using (FileStream compressedStream = File.OpenRead(compressedFile))
        using (ZLibStream decompressionStream = new ZLibStream(compressedStream, CompressionMode.Decompress))
        using (StreamReader reader = new StreamReader(decompressionStream))
        {
            string fileContents = reader.ReadToEnd().Split('\0').Skip(1).First();
            Console.Write(fileContents);
        }
    }

    public static void CompressFile(string hash, string objectDirPath, byte[] content)
    {
        var parentDir = BlobPathHelper.GetParentDirFromHash(hash);
        var fileName = BlobPathHelper.GetFileNameFromHash(hash);

        var parentDirPath = Path.Combine(objectDirPath, fileName);
        var filePath = Path.Combine(parentDirPath, fileName);

        Directory.CreateDirectory(parentDirPath);
        using FileStream compressedStream = File.OpenWrite(Path.Combine(parentDirPath, filePath));
        using (ZLibStream compressionStream = new ZLibStream(compressedStream, CompressionMode.Compress))
        using (StreamWriter writer = new StreamWriter(compressionStream))
        {
            writer.Write($"blob {content.Length}\0{content}");
        }

        Console.Write(hash);
    }
}