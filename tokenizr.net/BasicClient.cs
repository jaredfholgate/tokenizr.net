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
    private readonly IEncryption _encryption;
    private readonly TokenTableSet _tokenTableSet;
        
    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings, new Encryption(), new Compression()), new Serialisation(), new Compression(), new Encryption(), null)
    {

    }

    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings, TokenTableSet tokenTableSet) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings, new Encryption(), new Compression()), new Serialisation(), new Compression(), new Encryption(), tokenTableSet)
    {

    }

    public BasicClient(ITableGenerator tableGenerator, IBasicService basicService, ISerialisation serialisation, ICompression compression, IEncryption encryption, TokenTableSet tokenTableSet)
    {
      _tableGenerator = tableGenerator;
      _basicService = basicService;
      _serialisation = serialisation;
      _compression = compression;
      _encryption = encryption;
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

    public BasicResult Tokenize(string stringToTokenize)
    {
      return _basicService.Tokenize(stringToTokenize, _tokenTableSet);
    }

    public async Task<List<BasicResult>> TokenizeAsync(List<string> stringsToTokenize)
    {
      return await _basicService.TokenizeAsync(stringsToTokenize, _tokenTableSet);
    }

    public BasicResult Detokenize(BasicRequest request)
    {
      if (_basicService.GetSettings().Behaviour == Behaviour.RandomSeedInconsistent && request.Seed == null && _basicService.GetSettings().Encrypt == false)
      {
        throw new ArgumentException("A valid seed is required to detonkenize a token created using Random Seed tokenization.");
      }
      return _basicService.Detokenize(request, _tokenTableSet);
    }

    public async Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> stringsToDetokenize)
    {
      if (_basicService.GetSettings().Behaviour == Behaviour.RandomSeedInconsistent && stringsToDetokenize[0].Seed == null && _basicService.GetSettings().Encrypt == false)
      {
        throw new ArgumentException("A valid seed is required to detonkenize a token created using Random Seed tokenization.");
      }
      return await _basicService.DetokenizeAsync(stringsToDetokenize, _tokenTableSet);
    }

    public string Serialise(string key, string iv)
    {
      var serlizedBasicClient = new SerlializedBasicClient()
      {
        TokenTable = _tokenTableSet,
        GeneratorSettings = (GeneratorSettings)_tableGenerator.GetSettings(),
        ServiceSettings = (ServiceSettings)_basicService.GetSettings()
      };
      _encryption.SetKeyAndIv(key, iv);
      return _encryption.EncryptString(_compression.Compress(_serialisation.Serliaise(serlizedBasicClient)));
    }
  }
}
