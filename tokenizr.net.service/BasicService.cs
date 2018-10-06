using System;
using System.Text;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public class BasicService
  {
    private readonly ISettings _settings;

    public BasicService(ISettings settings)
    {
      _settings = settings;
    }

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
      if(!_settings.Consistent)
      {
        var maximum = table.Count - 1;
        columnIndex = source.Length;
        while (columnIndex > maximum)
        {
          columnIndex-=maximum;
        }
      }

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
