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
      var service = new BasicService(new Settings());
      var testString = "Hello, this is a test string.";
      var resultString = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(testString.Length, resultString.Length);
      Assert.AreNotEqual(testString, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInNonConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
      var testString = "Hello, this is a test string.";
      var resultString = service.Tokenize(testString, tokenTable);
      resultString = service.Detokenize(resultString, tokenTable);
      Assert.AreEqual(testString, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings{ Consistent = true });
      var testString = "Hello, this is a test string.";
      var resultString = service.Tokenize(testString, tokenTable);
      resultString = service.Detokenize(resultString, tokenTable);
      Assert.AreEqual(testString, resultString);
    }

    [TestMethod]
    public void StartOfStringIsConsistentWhenInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings { Consistent = true });
      var testString1 = "Hello, this is a test string.";
      var testString2 = "Hello, this is a test string. Test 2.";
      var testString3 = "Hello, this is a test string. Testing, testing, testng. Test 3!";
      var resultString1 = service.Tokenize(testString1, tokenTable);
      var resultString2 = service.Tokenize(testString2, tokenTable);
      var resultString3 = service.Tokenize(testString3, tokenTable);

      Assert.IsTrue(resultString2.StartsWith(resultString1));
      Assert.IsTrue(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void StartOfStringIsNotConsistentWhenNotInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
      var testString1 = "Hello, this is a test string.";
      var testString2 = "Hello, this is a test string. Test 2.";
      var testString3 = "Hello, this is a test string. Testing, testing, testng. Test 3!";
      var resultString1 = service.Tokenize(testString1, tokenTable);
      var resultString2 = service.Tokenize(testString2, tokenTable);
      var resultString3 = service.Tokenize(testString3, tokenTable);

      Assert.IsFalse(resultString2.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void CanHandleStringsLongerThanArrayLength()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
      var testString = Alphabet;
      while(testString.Length < tokenTable.ForwardTable.Count)
      {
        testString += Alphabet;
      }
      var resultString = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(testString.Length, resultString.Length);
      Assert.AreNotEqual(testString, resultString);
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator();
      return generator.Generate(size, alphabet);
    }
  }
}
