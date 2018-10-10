using tokenizr.net.structures;

namespace tokenizr.net.serialisation
{
  public interface ISerialisation
  {
    TokenTableSet Deserialise(string table);
    string Serliaise(TokenTableSet table);
  }
}