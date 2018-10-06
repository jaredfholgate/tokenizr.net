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
    private const string TestString1 = "Hello, this is a test string.";
    private readonly string TestString2 = $"{TestString1} Test 2.";
    private readonly string TestString3 = $"{TestString1} Testing, testing, testng. Test 3!";

    [TestMethod]
    public void CanTokeniseABasicString()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
      var resultString = service.Tokenize(TestString1, tokenTable);
      Assert.AreEqual(TestString1.Length, resultString.Length);
      Assert.AreNotEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInNonConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
       var resultString = service.Tokenize(TestString1, tokenTable);
      resultString = service.Detokenize(resultString, tokenTable);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings{ Consistent = true });
      var resultString = service.Tokenize(TestString1, tokenTable);
      resultString = service.Detokenize(resultString, tokenTable);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void StartOfStringIsConsistentWhenInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings { Consistent = true });

      var resultString1 = service.Tokenize(TestString1, tokenTable);
      var resultString2 = service.Tokenize(TestString2, tokenTable);
      var resultString3 = service.Tokenize(TestString3, tokenTable);

      Assert.IsTrue(resultString2.StartsWith(resultString1));
      Assert.IsTrue(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void StartOfStringIsNotConsistentWhenNotInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());

      var resultString1 = service.Tokenize(TestString1, tokenTable);
      var resultString2 = service.Tokenize(TestString2, tokenTable);
      var resultString3 = service.Tokenize(TestString3, tokenTable);

      Assert.IsFalse(resultString2.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void CanHandleStringsLongerThanArrayLength()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());
      var testString = TestString1;
      while(testString.Length < tokenTable.ForwardTable.Count)
      {
        testString += TestString1;
      }
      var resultString = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(testString.Length, resultString.Length);
      Assert.AreNotEqual(testString, resultString);
    }

    [TestMethod]
    public void DifferentTablesGenerateDifferentResults()
    {
      var tokenTable = GenerateTable(Size, Alphabet);
      var service = new BasicService(new Settings());

      var resultString1 = service.Tokenize(TestString1, tokenTable);

      tokenTable = GenerateTable(Size, Alphabet);
      var resultString2 = service.Tokenize(TestString1, tokenTable);
        
      Assert.AreNotEqual(resultString1, resultString2);
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator();
      return generator.Generate(size, alphabet);
    }
  }
}
