using Newtonsoft.Json;
using System;
using tokenizr.net.structures;

namespace tokenizr.net.serialisation
{
  public class Serialisation
  {
    public string Serliaise(TokenTableSet table)
    {
      return JsonConvert.SerializeObject(table);
    }

    public TokenTableSet Deserialise(string table)
    {
      return JsonConvert.DeserializeObject<TokenTableSet>(table);
    }
  }
}
