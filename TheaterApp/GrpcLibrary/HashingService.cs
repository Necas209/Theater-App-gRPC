﻿using System.Runtime.InteropServices;
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

    public static bool SecureStringEqual(SecureString secureString1, SecureString secureString2)
    {
        ArgumentNullException.ThrowIfNull(secureString1);
        ArgumentNullException.ThrowIfNull(secureString2);
        if (secureString1.Length != secureString2.Length)
            return false;
        var ssBStr1Ptr = IntPtr.Zero;
        var ssBStr2Ptr = IntPtr.Zero;
        try
        {
            ssBStr1Ptr = Marshal.SecureStringToBSTR(secureString1);
            ssBStr2Ptr = Marshal.SecureStringToBSTR(secureString2);
            var str1 = Marshal.PtrToStringBSTR(ssBStr1Ptr);
            var str2 = Marshal.PtrToStringBSTR(ssBStr2Ptr);
            return str1.Equals(str2);
        }
        finally
        {
            if (ssBStr1Ptr != IntPtr.Zero) Marshal.ZeroFreeBSTR(ssBStr1Ptr);
            if (ssBStr2Ptr != IntPtr.Zero) Marshal.ZeroFreeBSTR(ssBStr2Ptr);
        }
    }
}