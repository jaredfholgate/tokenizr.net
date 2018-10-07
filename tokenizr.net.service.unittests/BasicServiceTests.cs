using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.constants;
using tokenizr.net.generator;
using tokenizr.net.structures;

namespace tokenizr.net.service.unittests
{
  [TestClass]
  public class BasicServiceTests
  {
    private const int Size = 2048;
    private const string TestString1 = "Hello, this is a test string.";
    private readonly string TestString2 = $"{TestString1} Test 2.";
    private readonly string TestString3 = $"{TestString1} Testing, testing, testng. Test 3!";
    private const string TestStringAllReplaced = "Hellothisisateststring";
    private const string TestStringHalfReplaced = "Hello,,,,,";
    private const string TestStringNoneReplaced = ",,,";

    [TestMethod]
    public void CanTokeniseABasicString()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var resultString = service.Tokenize(TestString1, tokenTable).Value;
      Assert.AreEqual(TestString1.Length, resultString.Length);
      Assert.AreNotEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInNonConsistentModeAndActionTypeIsCorrect()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.AreEqual(ActionType.Tokenize, result.Action);
      var resultString = result.Value;
      result = service.Detokenize(resultString, tokenTable);
      resultString = result.Value;
      Assert.AreEqual(ActionType.Detokenize, result.Action);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings{ Consistent = true });
      var resultString = service.Tokenize(TestString1, tokenTable).Value;
      resultString = service.Detokenize(resultString, tokenTable).Value;
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void StartOfStringIsConsistentWhenInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings { Consistent = true });

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;
      var resultString2 = service.Tokenize(TestString2, tokenTable).Value;
      var resultString3 = service.Tokenize(TestString3, tokenTable).Value;

      Assert.IsTrue(resultString2.StartsWith(resultString1));
      Assert.IsTrue(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void StartOfStringIsNotConsistentWhenNotInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;
      var resultString2 = service.Tokenize(TestString2, tokenTable).Value;
      var resultString3 = service.Tokenize(TestString3, tokenTable).Value;

      Assert.IsFalse(resultString2.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void CanHandleStringsLongerThanArrayLength()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var testString = TestString1;
      while(testString.Length < tokenTable.ForwardTable.Count)
      {
        testString += TestString1;
      }
      var resultString = service.Tokenize(testString, tokenTable).Value;
      Assert.AreEqual(testString.Length, resultString.Length);
      Assert.AreNotEqual(testString, resultString);
    }

    [TestMethod]
    public void DifferentTablesGenerateDifferentResults()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;

      tokenTable = GenerateTable(Size, Alphabet.English);
      var resultString2 = service.Tokenize(TestString1, tokenTable).Value;
        
      Assert.AreNotEqual(resultString1, resultString2);
    }

    [TestMethod]
    public void FlagIndicatesWhenFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestStringAllReplaced, tokenTable);
      Assert.IsTrue(result.AllTextReplaced);
    }

    [TestMethod]
    public void FlagIndicatesWhenNotFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.IsFalse(result.AllTextReplaced);
    }

    [TestMethod]
    public void PercentageIs100WhenFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestStringAllReplaced, tokenTable);
      Assert.AreEqual(100, result.PercentReplaced);
    }

    [TestMethod]
    public void PercentageIs50WhenHalfNotFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestStringHalfReplaced, tokenTable);
      Assert.AreEqual(50, result.PercentReplaced);
    }

    [TestMethod]
    public void PercentageIs0WhenNoneReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new Settings());
      var result = service.Tokenize(TestStringNoneReplaced, tokenTable);
      Assert.AreEqual(0, result.PercentReplaced);
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator();
      return generator.Generate(size, alphabet);
    }
  }
}
