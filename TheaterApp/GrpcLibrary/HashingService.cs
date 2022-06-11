using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace GrpcLibrary;

public static class HashingService
{
    private static byte[] HashSecureString(SecureString input, Func<byte[], byte[]> hash)
    {
        var bStr = Marshal.SecureStringToBSTR(input);
        var length = Marshal.ReadInt32(bStr, -4);
        var bytes = new byte[length];

        var bytesPin = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        try
        {
            Marshal.Copy(bStr, bytes, 0, length);
            Marshal.ZeroFreeBSTR(bStr);
            return hash(bytes);
        }
        finally
        {
            for (var i = 0; i < bytes.Length; i++) bytes[i] = 0;
            bytesPin.Free();
        }
    }

    public static string HashPassword(SecureString password)
    {
        using var mySha256 = SHA256.Create();
        var pwHash = HashSecureString(password, mySha256.ComputeHash);
        return Convert.ToHexString(pwHash).ToLower();
    }
}