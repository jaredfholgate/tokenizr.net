using System;
using System.Collections.Generic;
using System.Text;

namespace tokenizr.net.service
{
  public class BasicResult
  {
    public string Action { get; set; }

    public bool AllTextReplaced { get; set; }

    public double PercentageReplaced { get; set; }

    public string Value { get; set; }
  }
}
