using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
      var key = table.ForwardTable[0].Keys.ToList()[0];
      var expectedText = "{\"ForwardTable\":{\"columns\":[{\"rows\":[{\"f\":" + (int)key + ",\"t\":" + (int)table.ForwardTable[0][key].Item1 + ",\"n\":" + table.ForwardTable[0][key].Item2 + "},";
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
