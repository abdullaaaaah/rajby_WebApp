using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rajby_web.Encryption
{
  public static class EncryptionHelper
  {
    private static readonly string Key = "16charsecretkey!"; // 16-character key

    public static string Encrypt(string plainText)
    {
      if (string.IsNullOrEmpty(plainText))
        return plainText;

      // Convert key to bytes
      var keyBytes = Encoding.UTF8.GetBytes(Key);
      var validKey = GetValidKey(keyBytes);

      // Create AES object
      using (var aes = Aes.Create())
      {
        aes.Key = validKey;
        aes.GenerateIV(); // Dynamically generate IV

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        {
          // Write IV to the stream first
          ms.Write(aes.IV, 0, aes.IV.Length);

          using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          using (var sw = new StreamWriter(cs))
          {
            sw.Write(plainText);
          }

          // Convert encrypted bytes to Base64 and return
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    private static byte[] GetValidKey(byte[] keyBytes)
    {
      if (keyBytes.Length < 16)
        return keyBytes.Concat(new byte[16 - keyBytes.Length]).ToArray();
      if (keyBytes.Length > 32)
        return keyBytes.Take(32).ToArray();
      return keyBytes;
    }
  }
}
