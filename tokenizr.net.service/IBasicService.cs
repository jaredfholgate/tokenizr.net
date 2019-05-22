using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public interface IBasicService
  {
    IServiceSettings GetSettings();
    List<BasicResult> Detokenize(List<BasicRequest> requests, TokenTableSet table);
    Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> requests, TokenTableSet table);
    BasicResult Detokenize(BasicRequest request, TokenTableSet table);
    List<BasicResult> Tokenize(List<string> sources, TokenTableSet table);
    Task<List<BasicResult>> TokenizeAsync(List<string> sources, TokenTableSet table);
    BasicResult Tokenize(string source, TokenTableSet table);
  }
}