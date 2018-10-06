using System;
using System.Text;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public class BasicService
  {
    public string Tokenize(string source, TokenTableSet table)
    {
      return Encode(source, table.ForwardTable);
    }

    public string Detokenize(string source, TokenTableSet table)
    {
      return Encode(source, table.ReverseTable);
    }

    private string Encode(string source, TokenTable table)
    {
      var result = new StringBuilder();

      var columnIndex = 0;

      foreach (var character in source)
      {
        if (table[columnIndex].ContainsKey(character))
        {
          var newCharacter = table[columnIndex][character];
          result.Append(newCharacter.Item1);
          columnIndex = newCharacter.Item2;
        }
        else
        {
          result.Append(character);
        }
      }

      return result.ToString();
    }
  }
}
