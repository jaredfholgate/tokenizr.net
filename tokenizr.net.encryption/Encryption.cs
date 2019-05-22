﻿using System;
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
      Key = Encoding.UTF8.GetBytes(key.PadLeft(32));
      IV = Encoding.UTF8.GetBytes(iv.PadLeft(16));
    }

    public string EncryptString(string stringToEncrypt)
    {
      var doubleArray = stringToEncrypt.Select(c => (int)c).ToArray();
      //var test = new String(doubleArray.Select(o => (char)o).ToArray());
      var flatArray = string.Join("|", doubleArray);
      var bytes = EncryptStringToBytes(flatArray);
      var result = Convert.ToBase64String(bytes);
      return result;
    }

    public string DecryptString(string stringToDecrypt)
    {
      var bytes = Convert.FromBase64String(stringToDecrypt);
      var flatArray = DecryptStringFromBytes(bytes);
      var doubleArray = flatArray.Split('|').Select(o => Convert.ToInt32(o)).ToArray();
      var unicodeString = new String(doubleArray.Select(o => (char)o).ToArray());
      return unicodeString;
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