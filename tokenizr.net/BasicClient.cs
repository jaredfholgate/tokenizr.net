using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.compression;
using tokenizr.net.encryption;
using tokenizr.net.generator;
using tokenizr.net.serialisation;
using tokenizr.net.service;
using tokenizr.net.structures;

namespace tokenizr.net
{
  public class BasicClient : IBasicClient
  {
    private readonly ITableGenerator _tableGenerator;
    private readonly IBasicService _basicService;
    private readonly ISerialisation _serialisation;
    private readonly ICompression _compression;
    private readonly TokenTableSet _tokenTableSet;
        
    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings, new Encryption(), new Compression()), new Serialisation(), new Compression(), null)
    {

    }

    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings, TokenTableSet tokenTableSet) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings, new Encryption(), new Compression()), new Serialisation(), new Compression(), tokenTableSet)
    {

    }

    public BasicClient(ITableGenerator tableGenerator, IBasicService basicService, ISerialisation serialisation, ICompression compression, TokenTableSet tokenTableSet)
    {
      _tableGenerator = tableGenerator;
      _basicService = basicService;
      _serialisation = serialisation;
      _compression = compression;
      if (tokenTableSet == null)
      {
        _tokenTableSet = GenerateTokenTable();
      }
      else
      {
        _tokenTableSet = tokenTableSet;
      }
    }

    public TokenTableSet GenerateTokenTable()
    {
      return _tableGenerator.Generate();
    }

    public BasicResult Tokenize(string stringToTokenize, bool encrypt = false)
    {
      return _basicService.Tokenize(stringToTokenize, _tokenTableSet, encrypt);
    }

    public async Task<List<BasicResult>> TokenizeAsync(List<string> stringsToTokenize, bool encrypt = false)
    {
      return await _basicService.TokenizeAsync(stringsToTokenize, _tokenTableSet, encrypt);
    }

    public BasicResult Detokenize(string stringToDetokenize, List<int> seed = null, bool encrypted = false)
    {
      if (_basicService.GetSettings().Behaviour == Behaviour.RandomSeedInconsistent && seed == null && encrypted == false)
      {
        throw new ArgumentException("A valid seed is required to detonkenize a token created using Random Seed tokenization.");
      }
      return _basicService.Detokenize(new BasicRequest(stringToDetokenize, seed), _tokenTableSet, encrypted);
    }

    public async Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> stringsToDetokenize, bool encrypted = false)
    {
      if (_basicService.GetSettings().Behaviour == Behaviour.RandomSeedInconsistent && stringsToDetokenize[0].Seed == null && encrypted == false)
      {
        throw new ArgumentException("A valid seed is required to detonkenize a token created using Random Seed tokenization.");
      }
      return await _basicService.DetokenizeAsync(stringsToDetokenize, _tokenTableSet, encrypted);
    }

    public string Serialise(string encryptionKey)
    {
      var serlizedBasicClient = new SerlializedBasicClient()
      {
        TokenTable = _tokenTableSet,
        GeneratorSettings = (GeneratorSettings)_tableGenerator.GetSettings(),
        ServiceSettings = (ServiceSettings)_basicService.GetSettings()
      };
      return _compression.Compress(_serialisation.Serliaise(serlizedBasicClient));
    }
  }
}
