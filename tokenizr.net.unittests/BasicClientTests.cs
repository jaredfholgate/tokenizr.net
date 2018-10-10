using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.compression;
using tokenizr.net.generator;
using tokenizr.net.serialisation;
using tokenizr.net.service;

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
