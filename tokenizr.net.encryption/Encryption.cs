using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace tokenizr.net.encryption
{
  public class Encryption : IEncryption
  {
    public byte[] Key { get; set; }
    public byte[] IV { get; set; }

    public void SetKeyAndIv(string key, string iv)
    {
      if(string.IsNullOrWhiteSpace(key))
      {
        throw new ArgumentNullException("Key is required");
      }
      if (string.IsNullOrWhiteSpace(iv))
      {
        throw new ArgumentNullException("IV is required");
      }
      key = key.Length <= 32 ? key.PadLeft(32,'a') : key.Substring(0, 32);
      Key = Encoding.UTF8.GetBytes(key);
      iv = iv.Length <= 16 ? iv.PadLeft(16, 'a') : iv.Substring(0, 16);
      IV = Encoding.UTF8.GetBytes(iv);
    }

    public string EncryptString(string stringToEncrypt)
    {
      var bytes = EncryptStringToBytes(stringToEncrypt);
      var encryptedString = Convert.ToBase64String(bytes);
      return encryptedString;
    }

    public string DecryptString(string stringToDecrypt)
    {
      var bytes = Convert.FromBase64String(stringToDecrypt);
      var decryptedString = DecryptStringFromBytes(bytes);
      return decryptedString;
    }

    private byte[] EncryptStringToBytes(string plainText)
    {
      // Check arguments.
      if (plainText == null || plainText.Length <= 0)
        throw new ArgumentNullException("plainText");
      if (Key == null || Key.Length <= 0)
        throw new ArgumentNullException("Key");
      if (IV == null || IV.Length <= 0)
        throw new ArgumentNullException("IV");
      byte[] encrypted;

      // Create an Aes object
      // with the specified key and IV.
      using (Aes aesAlg = Aes.Create())
      {
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        // Create an encryptor to perform the stream transform.
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for encryption.
        using (MemoryStream msEncrypt = new MemoryStream())
        {
          using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
              //Write all data to the stream.
              swEncrypt.Write(plainText);
            }
            encrypted = msEncrypt.ToArray();
          }
        }
      }

      // Return the encrypted bytes from the memory stream.
      return encrypted;
    }
    private string DecryptStringFromBytes(byte[] cipherText)
    {
      // Check arguments.
      if (cipherText == null || cipherText.Length <= 0)
        throw new ArgumentNullException("cipherText");
      if (Key == null || Key.Length <= 0)
        throw new ArgumentNullException("Key");
      if (IV == null || IV.Length <= 0)
        throw new ArgumentNullException("IV");

      // Declare the string used to hold
      // the decrypted text.
      string plaintext = null;

      // Create an Aes object
      // with the specified key and IV.
      using (Aes aesAlg = Aes.Create())
      {
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        // Create a decryptor to perform the stream transform.
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        {
          using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {

              // Read the decrypted bytes from the decrypting stream
              // and place them in a string.
              plaintext = srDecrypt.ReadToEnd();
            }
          }
        }

      }

      return plaintext;

    }
  }
}
