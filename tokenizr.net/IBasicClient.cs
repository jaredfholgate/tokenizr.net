using tokenizr.net.service;

namespace tokenizr.net
{
  public interface IBasicClient
  {
    BasicResult Detokenize(string stringToDetokenize);
    BasicResult Tokenize(string stringToTokenize);
  }
}