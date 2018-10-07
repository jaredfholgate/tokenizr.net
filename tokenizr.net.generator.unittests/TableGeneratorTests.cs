using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.constants;
using tokenizr.net.structures;

namespace tokenizr.net.generator.unittests
{
  [TestClass]
  public class TableGeneratorTests
  {
    private const int Size2048 = 2048;
    private const string EnglishWithPunctuationAndSpecialCharacters = Alphabet.English + Characters.Punctuation + Characters.SpecialCharacters;
    private const string EnglishWithSpacesPunctuationAndSpecialCharacters = Alphabet.English + Characters.Space + Characters.Punctuation + Characters.SpecialCharacters;

    [TestMethod]
    public void CanCreateABasicTableWithALengthOf2048AndEnglishLetters()
    {
      var table = GenerateTable(Size2048, Alphabet.English, false, false, false).ForwardTable;
      Assert.AreEqual(Size2048, table.Count);
      Assert.AreEqual(Alphabet.English.Length, table[0].Count);

      for (var i = 0; i < Size2048; i++)
      {
        foreach (var character in Alphabet.English)
        {
          Assert.IsTrue(table[i].ContainsKey(character));
          Assert.IsTrue(table[i][character].Item2 >= 0 && table[i][character].Item2 <= Size2048 - 1);
        }
      }
    }

    [TestMethod]
    public void CanCreateABasicTableWithALengthOf2048AndEnglishWithPunctuationAndSpecialCharacters()
    {
      var table = GenerateTable(Size2048, Alphabet.English, false, true, true).ForwardTable;
      Assert.AreEqual(Size2048, table.Count);
      Assert.AreEqual(EnglishWithPunctuationAndSpecialCharacters.Length, table[0].Count);

      for (var i = 0; i < Size2048; i++)
      {
        foreach (var character in EnglishWithPunctuationAndSpecialCharacters)
        {
          Assert.IsTrue(table[i].ContainsKey(character));
          Assert.IsTrue(table[i][character].Item2 >= 0 && table[i][character].Item2 <= Size2048 - 1);
        }
      }
    }

    [TestMethod]
    public void CanCreateABasicTableWithALengthOf2048AndEnglishWithPunctuationAndSpecialCharactersAndSpaces()
    {
      var table = GenerateTable(Size2048, Alphabet.English, true, true, true).ForwardTable;

      Assert.IsTrue(table[0].ContainsKey(' '));
      Assert.AreEqual(Size2048, table.Count);
      Assert.AreEqual(EnglishWithSpacesPunctuationAndSpecialCharacters.Length, table[0].Count);

      for (var i = 0; i < Size2048; i++)
      {
        foreach (var character in EnglishWithSpacesPunctuationAndSpecialCharacters)
        {
          Assert.IsTrue(table[i].ContainsKey(character));
          Assert.IsTrue(table[i][character].Item2 >= 0 && table[i][character].Item2 <= Size2048 - 1);
        }
      }
    }

    [TestMethod]
    public void CanCreateAForwardAndReverseTableWithALengthOf2048AndEnglishLetters()
    {
      var table = GenerateTable(Size2048, Alphabet.English, false, false, false);
      Assert.AreEqual(Size2048, table.ForwardTable.Count);
      Assert.AreEqual(Size2048, table.ReverseTable.Count);

      for (var i = 0; i < Size2048; i++)
      {
        foreach (var character in Alphabet.English)
        {
          Assert.IsTrue(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item1 == character);
          Assert.AreEqual(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item2, table.ForwardTable[i][character].Item2);
        }
      }
    }


    [TestMethod]
    public void CanCreateAForwardAndReverseTableWithALengthOf2048AndNumbers()
    {
      var table = GenerateTable(Size2048, Alphabet.Numbers, false, false, false);
      Assert.AreEqual(Size2048, table.ForwardTable.Count);
      Assert.AreEqual(Size2048, table.ReverseTable.Count);

      for (var i = 0; i < Size2048; i++)
      {
        foreach (var character in Alphabet.Numbers)
        {
          Assert.IsTrue(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item1 == character);
          Assert.AreEqual(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item2, table.ForwardTable[i][character].Item2);
          Assert.IsTrue(char.IsDigit(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item1));
        }
      }
    }

    private TokenTableSet GenerateTable(int size, string alphabet, bool includeSpaces, bool includePunctuation, bool includeSpecialCharacters)
    {
      var generator = new TableGenerator(new GeneratorSettings { Size = size, Alphabet = alphabet, IncludeSpaces = includeSpaces, IncludePunctuation = includePunctuation, IncludeSpecialCharacters = includeSpecialCharacters });
      return generator.Generate();
    }
  }
}
