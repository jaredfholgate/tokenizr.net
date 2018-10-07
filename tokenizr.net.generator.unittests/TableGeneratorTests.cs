using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.constants;
using tokenizr.net.structures;

namespace tokenizr.net.generator.unittests
{
  [TestClass]
  public class TableGeneratorTests
  {
    private const int Size = 2048;

    [TestMethod]
    public void CreateABasicTableWithALengthOf2048AndEnglishLetters()
    {
      var table = GenerateTable(Size, Alphabet.English).ForwardTable;
      Assert.AreEqual(Size, table.Count);
      Assert.AreEqual(Alphabet.English.Length, table[0].Count);

      for (var i = 0; i < Size; i++)
      {
        foreach (var character in Alphabet.English)
        {
          Assert.IsTrue(table[i].ContainsKey(character));
          Assert.IsTrue(table[i][character].Item2 >= 0 && table[i][character].Item2 <= Size - 1);
        }
      }
    }

    [TestMethod]
    public void CreateAForwardAndReverseTableWithALengthOf2048AndEnglishLetters()
    {
      var table = GenerateTable(Size, Alphabet.English);
      Assert.AreEqual(Size, table.ForwardTable.Count);
      Assert.AreEqual(Size, table.ReverseTable.Count);

      for (var i = 0; i < Size; i++)
      {
        foreach (var character in Alphabet.English)
        {
          Assert.IsTrue(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item1 == character);
          Assert.AreEqual(table.ReverseTable[i][table.ForwardTable[i][character].Item1].Item2, table.ForwardTable[i][character].Item2);
        }
      }
    }

    private TokenTableSet GenerateTable(int size, string alphabet)
    {
      var generator = new TableGenerator();
      return generator.Generate(size, alphabet);
    }
  }
}
