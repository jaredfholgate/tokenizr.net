using System.Collections.Generic;

namespace tokenizr.net.service
{
  public class BasicResult
  {
    public string Action { get; set; }

    public bool AllTextReplaced { get; set; }

    public double PercentReplaced { get; set; }

    public string Value { get; set; }

    public List<int> Seed { get; set; }
  }
}
