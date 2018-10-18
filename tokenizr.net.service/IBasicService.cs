using System.Collections.Generic;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public interface IBasicService
  {
    IServiceSettings GetSettings();
    List<BasicResult> Detokenize(List<string> sources, TokenTableSet table);
    BasicResult Detokenize(string source, TokenTableSet table);
    List<BasicResult> Tokenize(List<string> sources, TokenTableSet table);
    BasicResult Tokenize(string source, TokenTableSet table);
  }
}