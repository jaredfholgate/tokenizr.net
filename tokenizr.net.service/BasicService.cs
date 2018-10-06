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

    public BasicResult Tokenize(string source, TokenTableSet table)
    {
      var result = Encode(source, table.ForwardTable);
      result.Action = "Tokenize";
      return result;
    }

    public BasicResult Detokenize(string source, TokenTableSet table)
    {
      var result = Encode(source, table.ReverseTable);
      result.Action = "Detokenize";
      return result;
    }

    private BasicResult Encode(string source, TokenTable table)
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

      var replacedCount = 0;

      foreach (var character in source)
      {
        if (table[columnIndex].ContainsKey(character))
        {
          var newCharacter = table[columnIndex][character];
          result.Append(newCharacter.Item1);
          columnIndex = newCharacter.Item2;
          replacedCount++;
        }
        else
        {
          result.Append(character);
        }
      }

      var percentReplaced = ((double)replacedCount / source.Length) * 100;

      return new BasicResult { Value = result.ToString(), AllTextReplaced = percentReplaced == 100, PercentageReplaced = percentReplaced };
    }
  }
}
