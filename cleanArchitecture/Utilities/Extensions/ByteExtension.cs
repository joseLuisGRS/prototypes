namespace Utilities.Extensions;

/// <summary>
/// Class used to compare bytes
/// </summary>
public static class ByteExtension
{
    public static bool Compare(this byte[] bytes, byte[] param)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] != param[i])
                return false;
        }

        return true;
    }
}
