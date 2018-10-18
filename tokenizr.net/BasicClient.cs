using System;
using System.Collections.Generic;
using System.Text;
using tokenizr.net.compression;
using tokenizr.net.generator;
using tokenizr.net.serialisation;
using tokenizr.net.service;
using tokenizr.net.structures;

namespace tokenizr.net
{
  public class BasicClient
  {
    private readonly ITableGenerator _tableGenerator;
    private readonly IBasicService _basicService;
    private readonly ISerialisation _serialisation;
    private readonly ICompression _compression;
    private TokenTableSet _tokenTableSet;

    public BasicClient(IGeneratorSettings generatorSettings, IServiceSettings serviceSettings) : this(new TableGenerator(generatorSettings), new BasicService(serviceSettings), new Serialisation(), new Compression())
    {

    }

    public BasicClient(ITableGenerator tableGenerator, IBasicService basicService, ISerialisation serialisation, ICompression compression)
    {
      _tableGenerator = tableGenerator;
      _basicService = basicService;
      _serialisation = serialisation;
      _compression = compression;
      _tokenTableSet = Generate();
    }

    public TokenTableSet Generate()
    {
      return _tableGenerator.Generate();
    }

    public BasicResult Tokenize(string stringToTokenize)
    {
      return _basicService.Tokenize(stringToTokenize, _tokenTableSet);
    }

    public BasicResult Detokenize(string stringToDetokenize)
    {
      return _basicService.Detokenize(stringToDetokenize, _tokenTableSet);
    }
  }
}
