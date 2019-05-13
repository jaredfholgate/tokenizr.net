using Microsoft.VisualStudio.TestTools.UnitTesting;
using tokenizr.net.constants;
using tokenizr.net.generator;
using tokenizr.net.structures;

namespace tokenizr.net.serialisation.unittests
{
  [TestClass]
  public class SerialisationTests
  {
    [TestMethod]
    public void CanSerliaiseTable()
    {
      var serialiser = new Serialisation();
      var generator = new TableGenerator(new GeneratorSettings { CharacterString = Alphabet.English, Size = 1000 });
      var table = generator.Generate();
      var result = serialiser.Serliaise(table);
      var expectedText = "{\"ForwardTable\":[{\"a\":{\"Item1\":\"" + table.ForwardTable[0]['a'].Item1 + "\",\"Item2\":" + table.ForwardTable[0]['a'].Item2 + "},\"b\":{\"Item1\":\"" + table.ForwardTable[0]['b'].Item1 + "\",\"Item2\":" + table.ForwardTable[0]['b'].Item2 + "},";
      Assert.IsTrue(result.Contains(expectedText));
    }

    [TestMethod]
    public void CanDeSerialiseTable()
    {
      var serialiser = new Serialisation();
      var generator = new TableGenerator(new GeneratorSettings { CharacterString = Alphabet.English, Size = 1000 });
      var table = generator.Generate();
      var result = serialiser.Serliaise(table);
      var resultTable = serialiser.Deserialise<TokenTableSet>(result);
      for (var i = 0; i < 1000; i++)
      {
        foreach (var character in Alphabet.English)
        {
          Assert.AreEqual(table.ForwardTable[i][character].Item1, resultTable.ForwardTable[i][character].Item1);
          Assert.AreEqual(table.ForwardTable[i][character].Item2, resultTable.ForwardTable[i][character].Item2);
        }
      }
    }
  }
}
