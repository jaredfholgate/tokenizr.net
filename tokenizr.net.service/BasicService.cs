using System;
using System.Text;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public class BasicService
  {
    public string Tokenize(string textToTokenize, TokenTableSet table)
    {
      var tokenizedString = new StringBuilder();

      var columnIndex = 0;

      foreach(var character in textToTokenize)
      {
        if (table.ForwardTable[columnIndex].ContainsKey(character))
        {
          var newCharacter = table.ForwardTable[columnIndex][character];
          tokenizedString.Append(newCharacter.Item1);
          columnIndex = newCharacter.Item2;
        }
        else
        {
          tokenizedString.Append(character);
        }
      }

      return tokenizedString.ToString();
    }

    public string Detokenize(string textToTokenize, TokenTableSet table)
    {
      var tokenizedString = new StringBuilder();

      var columnIndex = 0;

      foreach (var character in textToTokenize)
      {
        if (table.ReverseTable[columnIndex].ContainsKey(character))
        {
          var newCharacter = table.ReverseTable[columnIndex][character];
          tokenizedString.Append(newCharacter.Item1);
          columnIndex = newCharacter.Item2;
        }
        else
        {
          tokenizedString.Append(character);
        }
      }

      return tokenizedString.ToString();
    }
  }
}
