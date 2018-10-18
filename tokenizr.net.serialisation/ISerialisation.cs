using tokenizr.net.structures;

namespace tokenizr.net.serialisation
{
  public interface ISerialisation
  {
    T Deserialise<T>(string table);
    string Serliaise<T>(T table);
  }
}