using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Rajby_web.Encryption
{
  public static class EncryptionHelper
  {
    private static readonly string Key = "Your16CharKey123"; // Ensure this is a 16-char key
    private static readonly string Salt = "YourSaltValue123"; // Ensure this is a secure salt

    public static string Encrypt(string plainText)
    {
      if (string.IsNullOrEmpty(plainText))
        return plainText;

      // Generate Key and IV using Rfc2898DeriveBytes
      using (var keyDerivation = new Rfc2898DeriveBytes(Key, Encoding.UTF8.GetBytes(Salt), 1000))
      {
        var aes = Aes.Create();
        aes.Key = keyDerivation.GetBytes(32); // AES key size (256 bits)
        aes.IV = keyDerivation.GetBytes(16); // AES block size (128 bits)

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream())
        {
          using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          using (var sw = new StreamWriter(cs))
          {
            sw.Write(plainText);
          }
          return Convert.ToHexString(ms.ToArray()); // Use Hex encoding instead of Base64
        }
      }
    }

    public static string Decrypt(string encryptedText)
    {
      if (string.IsNullOrEmpty(encryptedText))
        return encryptedText;

      try
      {
        // Convert Hex string back to bytes
        var encryptedBytes = Convert.FromHexString(encryptedText);

        // Generate Key and IV using Rfc2898DeriveBytes
        using (var keyDerivation = new Rfc2898DeriveBytes(Key, Encoding.UTF8.GetBytes(Salt), 1000))
        {
          var aes = Aes.Create();
          aes.Key = keyDerivation.GetBytes(32); // AES key size (256 bits)
          aes.IV = keyDerivation.GetBytes(16); // AES block size (128 bits)

          using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
          using (var ms = new MemoryStream(encryptedBytes))
          using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          using (var sr = new StreamReader(cs))
          {
            return sr.ReadToEnd();
          }
        }
      }
      catch
      {
        return "Decryption failed. Invalid encrypted text or key.";
      }
    }
  }
}
