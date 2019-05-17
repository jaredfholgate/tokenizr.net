using System;
using tokenizr.net.compression;
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
       
    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings), new Serialisation(), new Compression(), null)
    {

    }

    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings, TokenTableSet tokenTableSet) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings), new Serialisation(), new Compression(), tokenTableSet)
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

    public BasicResult Tokenize(string stringToTokenize)
    {
      return _basicService.Tokenize(stringToTokenize, _tokenTableSet);
    }

    public BasicResult Detokenize(string stringToDetokenize, int seed = -1)
    {
      if (_basicService.GetSettings().Behaviour == Behaviour.RandomSeedInconsistent && seed == -1)
      {
        throw new ArgumentException("A valid seed is required to detonkenize a token created using Random Seed tokenization.");
      }
      return _basicService.Detokenize(stringToDetokenize, _tokenTableSet, seed);
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
