using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.service;

namespace tokenizr.net
{
  public interface IBasicClient
  {
    BasicResult Detokenize(BasicRequest request);
    BasicResult Tokenize(string stringToTokenize);
    Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> stringsToDetokenize);
    Task<List<BasicResult>> TokenizeAsync(List<string> stringsToTokenize);
  }
}