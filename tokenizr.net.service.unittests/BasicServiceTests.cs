using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.generator;
using tokenizr.net.structures;

namespace tokenizr.net.service.unittests
{
  [TestClass]
  public class BasicServiceTests
  {
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int Size = 2048;

    [TestMethod]
    public void CanTokeniseABasicString()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService();
      var testString = "Hello, this is a test string.";
      var resultString = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(testString.Length, resultString.Length);
      Assert.AreNotEqual(testString, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicString()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService();
      var testString = "Hello, this is a test string.";
      var resultString = service.Tokenize(testString, tokenTable);
      resultString = service.Detokenize(resultString, tokenTable);
      Assert.AreEqual(testString, resultString);
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator();
      return generator.Generate(size, alphabet);
    }
  }
}
