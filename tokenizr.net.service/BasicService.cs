using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tokenizr.net.compression;
using tokenizr.net.constants;
using tokenizr.net.encryption;
using tokenizr.net.random;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public class BasicService : IBasicService
  {
    private readonly IServiceSettings _settings;
    private readonly IEncryption _encryption;
    private readonly ICompression _compression;

    public BasicService(IServiceSettings settings, IEncryption encryption = null, ICompression compression = null)
    {
      _settings = settings;
      _encryption = encryption;
      _compression = compression;
      if (!string.IsNullOrWhiteSpace(settings.Key))
      {
        _encryption.SetKeyAndIv(settings.Key, settings.IV);
      }
    }

    public IServiceSettings GetSettings()
    {
      return _settings;
    }

    public BasicResult Tokenize(string source, TokenTableSet table, bool encrypt = false)
    {
      try
      {
        return TokenizeAsync(new List<string> { source }, table, encrypt).Result[0];
      }
      catch(Exception ex)
      {
        throw ex.InnerException;
      }
    }

    public BasicResult Detokenize(BasicRequest request, TokenTableSet table, bool encrypted = false)
    {
      try
      {
        return DetokenizeAsync(new List<BasicRequest> { request }, table, encrypted).Result[0];
      }
      catch (Exception ex)
      {
        throw ex.InnerException;
      }
    }

    public async Task<List<BasicResult>> TokenizeAsync(List<string> sources, TokenTableSet table, bool encrypt = false)
    {
      var results = new ConcurrentBag<BasicResult>();
      var tasks = new List<Task>();
      foreach (var source in sources)
      {
        tasks.Add(TokenizeString(table, results, source, encrypt));
      }
      await Task.WhenAll(tasks);
      return results.ToList();
    }
       
    public async Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> requests, TokenTableSet table, bool encrypted = false)
    {
      var results = new ConcurrentBag<BasicResult>();
      var tasks = new List<Task>();
      foreach (var request in requests)
      {
        tasks.Add(DetokenizeString(table, results, request, encrypted));
      }
      await Task.WhenAll(tasks);
      return results.ToList();
    }
    
    private BasicResult TokenizeCycle(TokenTableSet table, List<int> seeds, BasicResult result)
    {
      for (int i = 0; i < _settings.Cycles; i++)
      {
        var currentSeed = GetSeed(table.ForwardTable.Count);
        result = Encode(result.Value, table.ForwardTable, currentSeed);
        seeds.Add(currentSeed);
      }

      return result;
    }

    private async Task TokenizeString(TokenTableSet table, ConcurrentBag<BasicResult> results, string source, bool encrypt)
    {
      var seeds = new List<int>();
      var result = new BasicResult { Value = source };
      result = await Task.Run(() => TokenizeCycle(table, seeds, result));
      result.Action = ActionType.Tokenize;

      result.SourceValue = source;
      Encrypt(encrypt, seeds, result);
      results.Add(result);
    }

    private void Encrypt(bool encrypt, List<int> seeds, BasicResult result)
    {
      if (encrypt)
      {
        var seed = string.Join("|", seeds);
        var fullString = seed + "[ENDSEED]" + result.Value;
        var doubleArray = fullString.Select(c => (int)c).ToArray();
        var flatArray = string.Join("|", doubleArray);
        var compressedFlatArray = _compression.Compress(flatArray);
        result.Value = _encryption.EncryptString(compressedFlatArray);
      }
      else
      {
        result.Seed = seeds;
      }
    }

    private BasicResult DetokeniseCycle(TokenTableSet table, BasicRequest request, BasicResult result)
    {
      for (int i = 0; i < _settings.Cycles; i++)
      {
        result = Encode(result.Value, table.ReverseTable, request.Seed == null ? -1 : request.Seed[i]);
      }

      return result;
    }

    private async Task DetokenizeString(TokenTableSet table, ConcurrentBag<BasicResult> results, BasicRequest request, bool encrypted)
    {
      Decrypt(request, encrypted);

      if (request.Seed != null)
      {
        request.Seed.Reverse();
      }
      var result = new BasicResult { Value = request.Source };
      result = await Task.Run(() => DetokeniseCycle(table, request, result));
      result.Action = ActionType.Detokenize;
      result.SourceValue = request.Source;
      results.Add(result);
    }

    private void Decrypt(BasicRequest request, bool encrypted)
    {
      if (encrypted)
      {
        var compressedFlatArray = _encryption.DecryptString(request.Source);
        var flatArray = _compression.Decompress(compressedFlatArray);
        var doubleArray = flatArray.Split('|').Select(o => Convert.ToInt32(o)).ToArray();
        var unicodeString = new String(doubleArray.Select(o => (char)o).ToArray());
        var split = unicodeString.Split(new[] { "[ENDSEED]" }, StringSplitOptions.None);
        request.Seed = split[0].Split('|').Select(o => Convert.ToInt32(o)).ToList();
        request.Source = split[1];
      }
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

      return new BasicResult { Value = result.ToString(), AllTextReplaced = percentReplaced == 100, PercentReplaced = percentReplaced };
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
