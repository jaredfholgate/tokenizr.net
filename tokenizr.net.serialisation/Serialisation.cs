using Newtonsoft.Json;
using System;
using tokenizr.net.structures;

namespace tokenizr.net.serialisation
{
  public class Serialisation : ISerialisation
  {
    public string Serliaise<T>(T table)
    {
      return JsonConvert.SerializeObject(table);
    }

    public T Deserialise<T>(string table)
    {
      return JsonConvert.DeserializeObject<T>(table);
    }
  }
}
