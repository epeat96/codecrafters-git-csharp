using System.IO.Compression;
using System.Runtime.Serialization;

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

    public static void CompressFile(string hash, string objectDirPath, string content)
    {
        var parentDir = BlobFileHelper.GetParentDirFromHash(hash);
        var fileName = BlobFileHelper.GetFileNameFromHash(hash);

        var parentDirPath = Path.Combine(objectDirPath, parentDir);
        var filePath = Path.Combine(parentDirPath, fileName);

        Directory.CreateDirectory(parentDirPath);
        using FileStream compressedStream = File.OpenWrite(filePath);
        using (ZLibStream compressionStream = new ZLibStream(compressedStream, CompressionMode.Compress))
        using (StreamWriter writer = new StreamWriter(compressionStream))
        {
            writer.Write(BlobFileHelper.GetBlobContentFromFile(filePath));
            writer.Close();
            compressionStream.Close();
            compressedStream.Close();
        }

        Console.Write(hash);
    }
}