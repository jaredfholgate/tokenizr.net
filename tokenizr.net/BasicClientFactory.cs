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
    /// <param name="behaviour">Whether the tokenisation will be conistent or more randomised. Choose consistent if you want to search on partial words or phrases at the start of a string.</param>
    /// <param name="cycles">The number of cycles of tokenizatuion to run. More cycles is more secure, but more processor intensive.</param>
    /// <returns>A BasicClient with common settings.</returns>
    public static BasicClient GetClient(BasicClientType basicClientType, Behaviour behaviour = Behaviour.LengthBasedInconsistent, int cycles = 256, string key = "", string iv = "")
    {
      var largeSize = 2048;
      var smallSize = 100;
      BasicClient basicClient = null;
      switch(basicClientType)
      {
        case BasicClientType.BasicEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = largeSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles });
          break;

        case BasicClientType.FullEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = largeSize, IncludeSpaces = true, IncludePunctuation = true, IncludeSpecialCharacters = true }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles });
          break;

        case BasicClientType.BasicNumbers:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = largeSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles });
          break;

        case BasicClientType.CreditCard:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = largeSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, Mask = Mask.Parse("{{4*}}-{{4*}}-{{4*}}-{{4^}}") });
          break;

        case BasicClientType.FullUnicode:
            basicClient = new BasicClient(new GeneratorSettings() { CharacterArray = new unicode.Generator().Generate(), Size = smallSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, Key = key, IV = iv });
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
