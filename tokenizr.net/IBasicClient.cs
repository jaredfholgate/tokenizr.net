using tokenizr.net.service;

namespace tokenizr.net
{
  public interface IBasicClient
  {
    BasicResult Detokenize(string stringToDetokenize, int seed = -1);
    BasicResult Tokenize(string stringToTokenize);
  }
}