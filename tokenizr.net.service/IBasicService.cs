using System.Collections.Generic;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public interface IBasicService
  {
    IServiceSettings GetSettings();
    List<BasicResult> Detokenize(List<string> sources, TokenTableSet table, int columnIndex = -1);
    BasicResult Detokenize(string source, TokenTableSet table, int columnIndex = -1);
    List<BasicResult> Tokenize(List<string> sources, TokenTableSet table);
    BasicResult Tokenize(string source, TokenTableSet table);
  }
}