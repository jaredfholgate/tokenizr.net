using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using tokenizr.net.compression;
using tokenizr.net.constants;
using tokenizr.net.encryption;
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
    private const string TestNumbers = "4324-5098-1542-6579-0978-5382";
    private const string Key = "dfkjhsdi8y8w9efyiuwhfp8wef8we";
    private const string IV = "dsdfsdfsdfsdfsd";

    [TestMethod]
    public void CanTokeniseABasicString()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var resultString = service.Tokenize(TestString1, tokenTable).Value;
      Assert.AreEqual(TestString1.Length, resultString.Length);
      Assert.AreNotEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanTokeniseABasicNumber()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var service = new BasicService(new ServiceSettings());
      var resultString = service.Tokenize(TestNumbers, tokenTable).Value;
      Assert.AreEqual(TestNumbers.Length, resultString.Length);
      Assert.AreNotEqual(TestNumbers, resultString);
      for(var i = 0; i < TestNumbers.Length; i++)
      {
        if(TestNumbers[i] != '-')
        {
          Assert.IsTrue(char.IsDigit(resultString[i]));
        }
      }
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInLengthBasedInconsistentModeAndActionTypeIsCorrect()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.AreEqual(ActionType.Tokenize, result.Action);
      var resultString = result.Value;
      result = service.Detokenize(new BasicRequest(resultString), tokenTable);
      resultString = result.Value;
      Assert.AreEqual(ActionType.Detokenize, result.Action);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInRandomSeedInconsistentModeAndActionTypeIsCorrect()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings() { Behaviour = Behaviour.RandomSeedInconsistent });
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.AreEqual(ActionType.Tokenize, result.Action);
      var resultString = result.Value;
      result = service.Detokenize(new BasicRequest(resultString, result.Seed), tokenTable);
      resultString = result.Value;
      Assert.AreEqual(ActionType.Detokenize, result.Action);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInRandomSeedInconsistentModeAndEncrypted()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings() { Behaviour = Behaviour.RandomSeedInconsistent, Encrypt = true, Key = Key, IV = IV }, new Encryption(), new Compression());
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.AreEqual(ActionType.Tokenize, result.Action);
      var resultString = result.Value;
      result = service.Detokenize(new BasicRequest(resultString), tokenTable);
      resultString = result.Value;
      Assert.AreEqual(ActionType.Detokenize, result.Action);
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void CanDeTokeniseABasicStringInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings{ Behaviour = Behaviour.Consistent });
      var resultString = service.Tokenize(TestString1, tokenTable).Value;
      resultString = service.Detokenize(new BasicRequest(resultString), tokenTable).Value;
      Assert.AreEqual(TestString1, resultString);
    }

    [TestMethod]
    public void StartOfStringIsConsistentWhenInConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings { Behaviour = Behaviour.Consistent });

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;
      var resultString2 = service.Tokenize(TestString2, tokenTable).Value;
      var resultString3 = service.Tokenize(TestString3, tokenTable).Value;

      Assert.IsTrue(resultString2.StartsWith(resultString1));
      Assert.IsTrue(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void StartOfStringIsNotConsistentWhenInLengthBasedConsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;
      var resultString2 = service.Tokenize(TestString2, tokenTable).Value;
      var resultString3 = service.Tokenize(TestString3, tokenTable).Value;

      Assert.IsFalse(resultString2.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString1));
      Assert.IsFalse(resultString3.StartsWith(resultString2));
    }

    [TestMethod]
    public void StartOfStringIsNotConsistentWhenInRandomSeedInconsistentMode()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings() { Behaviour = Behaviour.LengthBasedInconsistent });

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
      var service = new BasicService(new ServiceSettings());
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
      var service = new BasicService(new ServiceSettings());

      var resultString1 = service.Tokenize(TestString1, tokenTable).Value;

      tokenTable = GenerateTable(Size, Alphabet.English);
      var resultString2 = service.Tokenize(TestString1, tokenTable).Value;
        
      Assert.AreNotEqual(resultString1, resultString2);
    }

    [TestMethod]
    public void FlagIndicatesWhenFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestStringAllReplaced, tokenTable);
      Assert.IsTrue(result.AllTextReplaced);
    }

    [TestMethod]
    public void FlagIndicatesWhenNotFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestString1, tokenTable);
      Assert.IsFalse(result.AllTextReplaced);
    }

    [TestMethod]
    public void PercentageIs100WhenFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestStringAllReplaced, tokenTable);
      Assert.AreEqual(100, result.PercentReplaced);
    }

    [TestMethod]
    public void PercentageIs50WhenHalfNotFullyReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestStringHalfReplaced, tokenTable);
      Assert.AreEqual(50, result.PercentReplaced);
    }

    [TestMethod]
    public void PercentageIs0WhenNoneReplaced()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(TestStringNoneReplaced, tokenTable);
      Assert.AreEqual(0, result.PercentReplaced);
    }

    [TestMethod]
    public void ZeroLengthStringIsHandled()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      var result = service.Tokenize(string.Empty, tokenTable);
      Assert.AreEqual(0, result.Value.Length);
    }

    [TestMethod]
    public void NullStringIsHandled()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());
      string testString = null;
      var result = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(0, result.Value.Length);
    }

    [TestMethod]
    public void CanHandleABasicMask()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = "****-****-****-****-****-^^^^";
      var service = new BasicService(new ServiceSettings { Mask = Mask.Parse(mask) });
      var result = service.Tokenize(TestNumbers, tokenTable);
      Assert.AreEqual(TestNumbers.Length, result.Value.Length);

      var splitTest = TestNumbers.Split('-');
      var splitResult = result.Value.Split('-');
      var splitMask = mask.Split('-');

      for(var i = 0; i < splitTest.Length; i ++)
      {
        if(splitMask[i] == "****")
        {
          Assert.AreNotEqual(splitTest[i], splitResult[i]);
        }
        if(splitMask[i] == "^^^^")
        {
          Assert.AreEqual(splitTest[i], splitResult[i]);
        }
      }
    }

    [TestMethod]
    public void CanHandleABasicWithNoSeparatorsMask()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = "*************************^^^^";
      var service = new BasicService(new ServiceSettings { Mask = Mask.Parse(mask) });
      var result = service.Tokenize(TestNumbers, tokenTable);
      Assert.AreEqual(TestNumbers.Length, result.Value.Length);

      var splitTest = TestNumbers.Split('-');
      var splitResult = result.Value.Split('-');

      for (var i = 0; i < splitTest.Length; i++)
      {
        if(i < splitTest.Length - 1)
        {
          Assert.AreNotEqual(splitTest[i], splitResult[i]);
        }
        else
        {
          Assert.AreEqual(splitTest[i], splitResult[i]);
        }
      }
    }

    [TestMethod]
    public void CanHandleAMaskMismatch()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = "****-****-****-****-****-^^^";
      var service = new BasicService(new ServiceSettings { Mask = Mask.Parse(mask) });
      var exception = Assert.ThrowsException<Exception>(() => service.Tokenize(TestNumbers, tokenTable));
      Assert.AreEqual("Mask Length does not match the source string length.", exception.Message);
    }

    [TestMethod]
    public void CanHandleAMaskMismatchForMatchAndKeep()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = "****-****-****-****-*****-^^^";
      var service = new BasicService(new ServiceSettings { Mask = Mask.Parse(mask) });
      var exception = Assert.ThrowsException<Exception>(() => service.Tokenize(TestNumbers, tokenTable));
      Assert.AreEqual("Mask is set to MustMatchAndKeep does not match. Expected: - Found: 5", exception.Message);
    }

    [TestMethod]
    public void CanHandleAnAdvancedMask()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = Mask.Parse("{{4*}}-{{4*}}-{{4*}}-{{4*}}-{{4*}}-{{4^}}");
      var service = new BasicService(new ServiceSettings { Mask = mask });
      var result = service.Tokenize(TestNumbers, tokenTable);
      Assert.AreEqual(TestNumbers.Length, result.Value.Length);

      var splitTest = TestNumbers.Split('-');
      var splitResult = result.Value.Split('-');

      for (var i = 0; i < splitTest.Length; i++)
      {
        if (i < splitTest.Length - 1)
        {
          Assert.AreNotEqual(splitTest[i], splitResult[i]);
        }
        else
        {
          Assert.AreEqual(splitTest[i], splitResult[i]);
        }
      }
    }

    [TestMethod]
    public void CanHandleAnAdvancedMaskWithNoSeparators()
    {
      var tokenTable = GenerateTable(Size, Alphabet.Numbers);
      var mask = Mask.Parse("{{4^}}{{21*}}{{4^}}");
      var service = new BasicService(new ServiceSettings { Mask = mask });
      var result = service.Tokenize(TestNumbers, tokenTable);
      Assert.AreEqual(TestNumbers.Length, result.Value.Length);

      var splitTest = TestNumbers.Split('-');
      var splitResult = result.Value.Split('-');

      for (var i = 0; i < splitTest.Length; i++)
      {
        if (i != 0 && i < splitTest.Length - 1)
        {
          Assert.AreNotEqual(splitTest[i], splitResult[i]);
        }
        else
        {
          Assert.AreEqual(splitTest[i], splitResult[i]);
        }
      }
    }

    [TestMethod]
    public void CanHandleBasicMaskWithCurlyBraces()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var mask = Mask.Parse("{********}");
      var testString = "{TestMask}";
      var service = new BasicService(new ServiceSettings { Mask = mask });
      var result = service.Tokenize(testString, tokenTable);
      Assert.AreEqual(testString.Length, result.Value.Length);
      Assert.AreEqual('{', result.Value[0]);
      Assert.AreEqual('}', result.Value[9]);
      Assert.IsFalse(result.Value.Contains("TestMask"));
    }

    [TestMethod]
    public void CanHandleMultipleStrings()
    {
      var tokenTable = GenerateTable(Size, Alphabet.English);
      var service = new BasicService(new ServiceSettings());

      var testStrings = new List<string>();
      for (int i = 0; i < 100; i++)
      {
        testStrings.Add(TestString1 + i.ToString());
      }

      var results = service.TokenizeAsync(testStrings, tokenTable).Result;
      results = service.DetokenizeAsync(results.Select(o => new BasicRequest(o.Value)).ToList(), tokenTable).Result;
      for(var i = 0; i < testStrings.Count; i++)
      {
        Assert.IsTrue(results.Any(o => o.Value == testStrings[i]));
      }
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator(new GeneratorSettings { Size = size, CharacterString = alphabet});
      return generator.Generate();
    }
  }
}
