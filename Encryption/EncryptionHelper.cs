using System.Security.Cryptography;
using System.Text;

namespace Rajby_web.Encryption
{
  public static class EncryptionHelper
  {
    private static readonly string Key = "16charsecretkey!";

    public static string Encrypt(string plainText)
    {
      if (string.IsNullOrEmpty(plainText))
        return plainText;

      var keyBytes = Encoding.UTF8.GetBytes(Key);
      var validKey = GetValidKey(keyBytes);

      using (var aes = Aes.Create())
      {
        aes.Key = validKey;
        aes.IV = new byte[16]; // Zero initialization vector

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        {
          using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          using (var sw = new StreamWriter(cs))
          {
            sw.Write(plainText);
          }
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    public static string Decrypt(string encryptedText)
    {
      if (string.IsNullOrEmpty(encryptedText))
        return encryptedText;

      var keyBytes = Encoding.UTF8.GetBytes(Key);
      var validKey = GetValidKey(keyBytes);

      using (var aes = Aes.Create())
      {
        aes.Key = validKey;
        aes.IV = new byte[16]; // Zero initialization vector

        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (var sr = new StreamReader(cs))
        {
          return sr.ReadToEnd();
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
