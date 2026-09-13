using System.Text;

namespace codecrafters_git.Helpers;

public static class BlobPathHelper
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
}