using System.Collections.Generic;

namespace tokenizr.net.generator
{
  public class GeneratorSettings : IGeneratorSettings
  {
    public int Size { get; set; }
    public string CharacterString { get; set; }
    public List<char> CharacterArray { get; set; }
    public bool IncludeSpaces { get; set; }
    public bool IncludePunctuation { get; set; }
    public bool IncludeSpecialCharacters { get; set; }
  }
}
