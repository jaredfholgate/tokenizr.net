using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tokenizr.net.encryption.unittests
{
  [TestClass]
  public class EncryptionTests
  {
    private const string Key = "dfkjhsdi8y8w9efyiuwhfp8wef8we";
    private const string IV = "dsdfsdfsdfsdfsd";

    [TestMethod]
    public void CanEncryptString()
    {
      var test = "Blah, blah, blah";
      var encryption = new Encryption();
      encryption.SetKeyAndIv(Key, IV);

      var result = encryption.EncryptString(test);
      Assert.AreEqual("n0R5icIeFngnX0iT+6FIBs93p/JSBzbwFqh5ogSJn/M=", result);
    }

    [TestMethod]
    public void CanDecryptString()
    {
      var test = "Jessica is wonderful!";
      var encryption = new Encryption();
      encryption.SetKeyAndIv(Key, IV);

      var result = encryption.EncryptString(test);
      result = encryption.DecryptString(result);

      Assert.AreEqual(test, result);
    }
  }
}
