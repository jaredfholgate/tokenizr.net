using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tokenizr.net.unittests
{
  [TestClass]
  public class BasicClientTests
  {
    [TestMethod]
    public void CanGenerateAStandardTableThatIsSerialisedCompressedAndEncrypted()
    {
      var client = new BasicClient();
      var result = client.Generate();
      Assert.AreNotEqual(0, result.Length);
    }
  }
}
