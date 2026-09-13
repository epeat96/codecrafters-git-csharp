using System.Text;

namespace codecrafters_git.Helpers;

public static class BlobFileHelper
{
    public static string GetParentDirFromHash(string hash)
    {
        var sb = new StringBuilder();
        return sb.AppendJoin("", hash.First(), hash.Skip(1).First()).ToString();
    }

    public static string GetFileNameFromHash(string hash)
    {
        var sb = new StringBuilder();
        return sb.AppendJoin("", hash.Skip(2)).ToString();
    }

    public static string GetBlobContentFromFile(string filePath)
    {
        var content = FileHelper.GetFileContent(filePath);
        return $"blob {content.Length}\0{content}";
    }
}