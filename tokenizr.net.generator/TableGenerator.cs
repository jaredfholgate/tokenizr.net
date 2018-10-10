using System;
using tokenizr.net.structures;
using System.Collections.Generic;
using System.Linq;
using tokenizr.net.constants;

namespace tokenizr.net.generator
{
  public class TableGenerator : ITableGenerator
  {
    private readonly IGeneratorSettings _settings;

    public TableGenerator(IGeneratorSettings settings)
    {
      _settings = settings;
    }

    public TokenTableSet Generate()
    {
      var tableSet = new TokenTableSet();
      var forwardTable = new TokenTable();
      var reverseTable = new TokenTable();
      var alphabet = _settings.Alphabet + (_settings.IncludeSpaces ? Characters.Space : string.Empty) + (_settings.IncludePunctuation ? Characters.Punctuation : string.Empty ) + (_settings.IncludeSpecialCharacters ? Characters.SpecialCharacters : string.Empty);

      for (var i = 0; i <_settings.Size; i++)
      {
        GenerateRandomColumn(_settings.Size, alphabet, forwardTable, reverseTable);
      }

      tableSet.ForwardTable = forwardTable;
      tableSet.ReverseTable = reverseTable;

      return tableSet;
    }

    public void GenerateRandomColumn(int size, string alphabet, TokenTable forwardTable, TokenTable reverseTable)
    {
      var forwardColumn = new Dictionary<char, Tuple<char, int>>();
      var reverseColumn = new Dictionary<char, Tuple<char, int>>();

      var replacements = alphabet.ToCharArray().ToList();
      replacements.Shuffle();
      for(var i = 0; i < alphabet.Length; i++)
      {
        var columnReference = ThreadSafeRandom.ThisThreadsRandom.Next(0, size - 1);
        forwardColumn.Add(alphabet[i], new Tuple<char, int>(replacements[i], columnReference));
        reverseColumn.Add(replacements[i], new Tuple<char, int>(alphabet[i], columnReference));
      }

      forwardTable.Add(forwardColumn);
      reverseTable.Add(reverseColumn);
    }
  }
}
