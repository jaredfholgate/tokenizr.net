using System;
using System.Collections.Generic;
using System.Text;
using tokenizr.net.generator;
using tokenizr.net.service;
using tokenizr.net.structures;

namespace tokenizr.net
{
  public class SerlializedBasicClient
  {
    public GeneratorSettings GeneratorSettings { get; set; }
    public ServiceSettings ServiceSettings { get; set; }
    public TokenTableSet TokenTable { get; set; }
  }
}
