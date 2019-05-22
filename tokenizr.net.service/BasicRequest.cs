using System;
using System.Collections.Generic;
using System.Text;

namespace tokenizr.net.service
{
  public class BasicRequest
  {
    public BasicRequest()
    {

    }
    public BasicRequest(string source, List<int> seed = null)
    {
      Source = source;
      Seed = seed;
    }
    public string Source { get; set; }
    public List<int> Seed { get; set; }
  }
}
