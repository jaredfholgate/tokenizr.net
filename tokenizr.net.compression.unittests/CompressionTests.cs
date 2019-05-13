using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.constants;
using tokenizr.net.generator;
using tokenizr.net.serialisation;

namespace tokenizr.net.compression.unittests
{
  [TestClass]
  public class CompressionTests
  {
    [TestMethod]
    public void CanCompressATokenTableAndDecompressIt()
    {
      var compressor = new Compression();
      var serialiser = new Serialisation();
      var generator = new TableGenerator(new GeneratorSettings { CharacterString = Alphabet.English, Size = 1000 });
      var table = generator.Generate();
      var serialised = serialiser.Serliaise(table);
      var result = compressor.Compress(serialised);
      Assert.IsTrue(serialised.Length > result.Length);
      result = compressor.Decompress(result);
      Assert.AreEqual(serialised, result);
    }
  }
}
