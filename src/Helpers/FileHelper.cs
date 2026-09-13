using System.Security.Cryptography;

namespace codecrafters_git.Helpers;

public static class FileHelper
{
    public static string ComputeSha1(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        using var sha1 = SHA1.Create();
        var bytes = BlobFileHelper.GetBlobContentFromFile(filePath).Select(c => Convert.ToByte(c)).ToArray();
        byte[] hashBytes = sha1.ComputeHash(bytes);

        return Convert.ToHexString(hashBytes);
    }

    public static string GetFileContent(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        return File.ReadAllText(filePath);
    }
}