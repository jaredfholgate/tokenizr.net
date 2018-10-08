﻿namespace tokenizr.net.generator
{
  public interface IGeneratorSettings
  {
    string Alphabet { get; set; }
    bool IncludeSpaces { get; set; }
    int Size { get; set; }
    bool IncludePunctuation { get; set; }
    bool IncludeSpecialCharacters { get; set; }
  }
}