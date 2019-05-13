using System.Collections.Generic;

namespace tokenizr.net.generator
{
  public interface IGeneratorSettings
  {
    string CharacterString { get; set; }
    List<char> CharacterArray { get; set; }
    bool IncludeSpaces { get; set; }
    int Size { get; set; }
    bool IncludePunctuation { get; set; }
    bool IncludeSpecialCharacters { get; set; }
  }
}