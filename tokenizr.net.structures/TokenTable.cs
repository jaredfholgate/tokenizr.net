using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace tokenizr.net.structures
{
  [JsonConverter(typeof(TokenTableSerialiser))]
  public class TokenTable : List<Dictionary<char, Tuple<char, int>>>
  {

  }
}
