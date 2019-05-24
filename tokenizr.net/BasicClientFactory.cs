using tokenizr.net.constants;
using tokenizr.net.generator;
using tokenizr.net.service;
using tokenizr.net.compression;
using tokenizr.net.serialisation;
using tokenizr.net.encryption;

namespace tokenizr.net
{
  public class BasicClientFactory
  {
    private readonly ISerialisation _serialisation;
    private readonly ICompression _compression;
    private readonly IEncryption _encryption;

    public BasicClientFactory() : this(new Serialisation(), new Compression(), new Encryption())
    {
    }

    public BasicClientFactory(ISerialisation serialisation, ICompression compression, IEncryption encryption)
    {
      _serialisation = serialisation;
      _compression = compression;
      _encryption = encryption;
    }

    /// <summary>
    /// Instantiate a BasicClient with set of common defaults. Use this class as an example for more customised setups.
    /// </summary>
    /// <param name="basicClientType">The common defaults to apply.</param>
    /// <param name="behaviour">Whether the tokenisation will be conistent or more randomised. Choose consistent if you want to search on partial words or phrases at the start of a string.</param>
    /// <param name="cycles">The number of cycles of tokenizatuion to run. More cycles is more secure, but more processor intensive.</param>
    /// <returns>A BasicClient with common settings.</returns>
    public static BasicClient GetClient(BasicClientType basicClientType, Behaviour behaviour = Behaviour.LengthBasedInconsistent, int tableSize = -1, int cycles = 256, bool seedPerCycle = false, bool encrypt = false, string key = "", string iv = "")
    {
      var largeSize = 2048;
      var smallSize = 100;
      BasicClient basicClient = null;
      switch(basicClientType)
      {
        case BasicClientType.BasicEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = tableSize == -1 ? largeSize : tableSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, SeedPerCycle = seedPerCycle, Encrypt = encrypt, Key = key, IV = iv });
          break;

        case BasicClientType.FullEnglish:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.English, Size = tableSize == -1 ? largeSize : tableSize, IncludeSpaces = true, IncludePunctuation = true, IncludeSpecialCharacters = true }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, SeedPerCycle = seedPerCycle, Encrypt = encrypt, Key = key, IV = iv });
          break;

        case BasicClientType.BasicNumbers:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = tableSize == -1 ? largeSize : tableSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, SeedPerCycle = seedPerCycle, Encrypt = encrypt, Key = key, IV = iv });
          break;

        case BasicClientType.CreditCard:
          basicClient = new BasicClient(new GeneratorSettings() { CharacterString = Alphabet.Numbers, Size = tableSize == -1 ? largeSize : tableSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, Mask = Mask.Parse("{{4*}}-{{4*}}-{{4*}}-{{4^}}"), SeedPerCycle = seedPerCycle, Encrypt = encrypt, Key = key, IV = iv });
          break;

        case BasicClientType.FullUnicode:
            basicClient = new BasicClient(new GeneratorSettings() { CharacterArray = new unicode.Generator().Generate(), Size = tableSize == -1 ? smallSize : tableSize }, new ServiceSettings() { Behaviour = behaviour, Cycles = cycles, SeedPerCycle = seedPerCycle, Encrypt = encrypt, Key = key, IV = iv });
            break;

      }
      return basicClient;
    }

    public BasicClient Deserialise(string key, string iv, string client)
    {
      _encryption.SetKeyAndIv(key, iv);
      var serlizedBasicClient = _serialisation.Deserialise<SerlializedBasicClient>(_compression.Decompress(_encryption.DecryptString(client)));
      return new BasicClient(serlizedBasicClient.GeneratorSettings, serlizedBasicClient.ServiceSettings, serlizedBasicClient.TokenTable);
    }
  }
}
