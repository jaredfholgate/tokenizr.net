using System.Collections.Generic;
using System.Text;
using tokenizr.net.constants;
using tokenizr.net.random;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public class BasicService : IBasicService
  {
    private readonly IServiceSettings _settings;

    public BasicService(IServiceSettings settings)
    {
      _settings = settings;
    }

    public IServiceSettings GetSettings()
    {
      return _settings;
    }

    public BasicResult Tokenize(string source, TokenTableSet table)
    {
      return Tokenize(new List<string> { source }, table)[0];
    }

    public BasicResult Detokenize(string source, TokenTableSet table, int seed = -1)
    {
      return Detokenize(new List<string> { source }, table, seed)[0];
    }

    public List<BasicResult> Tokenize(List<string> sources, TokenTableSet table)
    {
      var results = new List<BasicResult>();
      int seed = GetSeed(table.ForwardTable.Count);
      foreach (var source in sources)
      {
        var result = new BasicResult { Value = source };
        for (int i = 0; i < _settings.Cycles; i++)
        {
          result = Encode(result.Value, table.ForwardTable, seed);
        }
        result.Action = ActionType.Tokenize;
        results.Add(result);
      }
      return results;
    }
        
    public List<BasicResult> Detokenize(List<string> sources, TokenTableSet table, int seed = -1)
    {
      var results = new List<BasicResult>();
      foreach (var source in sources)
      {
        var result = new BasicResult { Value = source };
        for (int i = 0; i < _settings.Cycles; i++)
        {
          result = Encode(result.Value, table.ReverseTable, seed);
        }
        result.Action = ActionType.Detokenize;
        results.Add(result);
      }
      return results;
    }

    private BasicResult Encode(string source, TokenTable table, int seed)
    {
      if (string.IsNullOrEmpty(source))
      {
        return new BasicResult { Value = string.Empty, PercentReplaced = -1, AllTextReplaced = false };
      }

      var hasMask = SetupMask(source);

      var result = new StringBuilder();

      var columnIndex = SetupColumnIndex(source, table, seed);

      var replacedCount = 0;

      for (var i = 0; i < source.Length; i++)
      {
        if (table[columnIndex].ContainsKey(source[i]))
        {
          if (hasMask && _settings.Mask.Items[i].MaskType != MaskType.ReplaceAny)
          {
            var maskItem = _settings.Mask.Items[i];
            if (maskItem.MaskType == MaskType.KeepAnyOriginal)
            {
              result.Append(source[i]);
            }
            if (maskItem.MaskType == MaskType.MustMatchAndKeep)
            {
              CheckMaskMatch(source, result, i, maskItem);
            }
          }
          else
          {
            var newCharacter = table[columnIndex][source[i]];
            result.Append(newCharacter.Item1);
            columnIndex = newCharacter.Item2;
            replacedCount++;
          }
        }
        else
        {
          if (hasMask && _settings.Mask.Items[i].MaskType == MaskType.MustMatchAndKeep)
          {
            var maskItem = _settings.Mask.Items[i];
            CheckMaskMatch(source, result, i, maskItem);
          }
          else
          {
            result.Append(source[i]);
          }
        }
      }

      var percentReplaced = ((double)replacedCount / source.Length) * 100;

      return new BasicResult { Value = result.ToString(), AllTextReplaced = percentReplaced == 100, PercentReplaced = percentReplaced, Seed = seed };
    }

    private int SetupColumnIndex(string source, TokenTable table, int seed)
    {
      int columnIndex = 0;

      switch (_settings.Behaviour)
      {
        case Behaviour.Consistent:
          columnIndex = 0;
          break;

        case Behaviour.LengthBasedInconsistent:
          var maximum = table.Count - 1;
          columnIndex = source.Length;
          while (columnIndex > maximum)
          {
            columnIndex -= maximum;
          }
          break;

        case Behaviour.RandomSeedInconsistent:
          columnIndex = seed;
          break;
      }

      return columnIndex;
    }

    private bool SetupMask(string source)
    {
      var hasMask = _settings.Mask != null && _settings.Mask.Length > 0;

      if (hasMask && source.Length != _settings.Mask.Length)
      {
        throw new System.Exception("Mask Length does not match the source string length.");
      }

      return hasMask;
    }

    private static void CheckMaskMatch(string source, StringBuilder result, int i, MaskItem maskItem)
    {
      if (source[i] == maskItem.Match)
      {
        result.Append(source[i]);
      }
      else
      {
        throw new System.Exception($"Mask is set to MustMatchAndKeep does not match. Expected: {maskItem.Match} Found: {source[i]}");
      }
    }
        private int GetSeed(int tableLength)
    {
      var seed = -1;
      if (_settings.Behaviour == Behaviour.RandomSeedInconsistent)
      {
        seed = ThreadSafeRandom.ThisThreadsRandom.Next(0, tableLength - 1);
      }

      return seed;
    }
  }
}
