using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.generator;
using tokenizr.net.service;

namespace tokenizr.net.unittests
{
  [TestClass]
  public class BasicClientTests
  {
    [TestMethod]
    public void CanGenerateAStandardTable()
    {
      var client = new BasicClient(new GeneratorSettings() { CharacterString = "abc", Size= 200 }, new ServiceSettings());
      var result = client.GenerateTokenTable();
      Assert.AreEqual(200, result.ForwardTable.Count);
    }
  }
}
