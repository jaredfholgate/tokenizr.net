using tokenizr.net.constants;
using tokenizr.net.generator;
using tokenizr.net.service;
using tokenizr.net.compression;
using tokenizr.net.serialisation;

namespace tokenizr.net
{
  public class BasicClientFactory
  {
    private readonly ISerialisation _serialisation;
    private readonly ICompression _compression;

    public BasicClientFactory() : this(new Serialisation(), new Compression())
    {
    }

    public BasicClientFactory(ISerialisation serialisation, ICompression compression)
    {
      _serialisation = serialisation;
      _compression = compression;
    }

    /// <summary>
    /// Instantiate a BasicClient with set of common defaults. Use this class as an example for more customised setups.
    /// </summary>
    /// <param name="basicClientType">The common defaults to apply.</param>
    /// <param name="Consistent">Whether the toeknisation will be conistent or more randomised. Choose consistent if you want to search on partial words or phrases at the start of a string.</param>
    /// <returns>A BasicClient with common settings.</returns>
    public static BasicClient GetClient(BasicClientType basicClientType, bool consistent = false)
    {
      var size = 2048;
      BasicClient basicClient = null;
      switch(basicClientType)
      {
        case BasicClientType.BasicEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = size }, new ServiceSettings() { Consistent = consistent });
          break;

        case BasicClientType.FullEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = size, IncludeSpaces = true, IncludePunctuation = true, IncludeSpecialCharacters = true }, new ServiceSettings() { Consistent = consistent });
          break;

        case BasicClientType.BasicNumbers:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = size }, new ServiceSettings());
          break;

        case BasicClientType.CreditCard:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = size }, new ServiceSettings() { Mask = Mask.Parse("{{4*}}-{{4*}}-{{4*}}-{{4^}}") });
          break;

        case BasicClientType.FullUnicode:
            basicClient = new BasicClient(new GeneratorSettings() { CharacterArray = new unicode.Generator().Generate(), Size = size }, new ServiceSettings() { Consistent = consistent });
            break;

      }
      return basicClient;
    }

    public BasicClient Deserialise(string encryptionKey, string client)
    {
      var serlizedBasicClient = _serialisation.Deserialise<SerlializedBasicClient>(_compression.Decompress(client));
      return new BasicClient(serlizedBasicClient.GeneratorSettings, serlizedBasicClient.ServiceSettings, serlizedBasicClient.TokenTable);
    }
  }
}
