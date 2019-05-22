using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.structures;

namespace tokenizr.net.service
{
  public interface IBasicService
  {
    IServiceSettings GetSettings();
    Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> requests, TokenTableSet table);
    BasicResult Detokenize(BasicRequest request, TokenTableSet table);
    Task<List<BasicResult>> TokenizeAsync(List<string> sources, TokenTableSet table);
    BasicResult Tokenize(string source, TokenTableSet table);
  }
}