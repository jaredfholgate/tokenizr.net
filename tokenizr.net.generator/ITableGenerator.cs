using tokenizr.net.structures;

namespace tokenizr.net.generator
{
  public interface ITableGenerator
  {
    TokenTableSet Generate();
    IGeneratorSettings GetSettings();
  }
}