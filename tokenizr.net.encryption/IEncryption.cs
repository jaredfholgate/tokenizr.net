namespace tokenizr.net.encryption
{
  public interface IEncryption
  {
    void SetKeyAndIv(string key, string iv);
    string DecryptString(string stringToDecrypt);
    string EncryptString(string stringToEncrypt);
  }
}