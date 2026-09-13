using System.Security.Cryptography;

namespace codecrafters_git.Helpers;

public static class FileHelper
{
    public static string ComputeSha1(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        using var sha1 = SHA1.Create();
        using var stream = File.OpenRead(filePath);
        byte[] hashBytes = sha1.ComputeHash(stream);

        return Convert.ToHexString(hashBytes);
    }

    public static byte[] GetFileContent(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);

        return File.ReadAllBytes(filePath);
    }
}