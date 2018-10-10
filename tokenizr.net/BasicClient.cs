using System;
using System.Collections.Generic;
using System.Text;
using tokenizr.net.compression;
using tokenizr.net.generator;
using tokenizr.net.serialisation;
using tokenizr.net.service;

namespace tokenizr.net
{
  public class BasicClient
  {
    private readonly ITableGenerator _tableGenerator;
    private readonly IBasicService _basicService;
    private readonly ISerialisation _serialisation;
    private readonly ICompression _compression;

    public BasicClient()
    {
      _tableGenerator = new TableGenerator(new GeneratorSettings());
      _basicService = new BasicService(new ServiceSettings());
      _serialisation = new Serialisation();
      _compression = new Compression();
    }

    public BasicClient(ITableGenerator tableGenerator, IBasicService basicService, ISerialisation serialisation, ICompression compression)
    {
      _tableGenerator = tableGenerator;
      _basicService = basicService;
      _serialisation = serialisation;
      _compression = compression;
    }

    public string Generate()
    {
      var table = _tableGenerator.Generate();
      var result = _compression.Compress(_serialisation.Serliaise(table));
      return result;
    }
  }
}
