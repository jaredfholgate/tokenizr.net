using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.service;

namespace tokenizr.net
{
  public interface IBasicClient
  {
    BasicResult Detokenize(string stringToDetokenize, List<int> seed = null);
    BasicResult Tokenize(string stringToTokenize);
    Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> stringsToDetokenize);
    Task<List<BasicResult>> TokenizeAsync(List<string> stringsToTokenize);
  }
}