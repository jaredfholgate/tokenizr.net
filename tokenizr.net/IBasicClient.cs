using System.Collections.Generic;
using System.Threading.Tasks;
using tokenizr.net.service;

namespace tokenizr.net
{
  public interface IBasicClient
  {
    BasicResult Detokenize(string stringToDetokenize, List<int> seed = null, bool encrypted = false);
    BasicResult Tokenize(string stringToTokenize, bool encrypted = false);
    Task<List<BasicResult>> DetokenizeAsync(List<BasicRequest> stringsToDetokenize, bool encrypted = false);
    Task<List<BasicResult>> TokenizeAsync(List<string> stringsToTokenize, bool encrypted = false);
  }
}