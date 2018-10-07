namespace tokenizr.net.generator
{
  public class GeneratorSettings : IGeneratorSettings
  {
    public int Size { get; set; }
    public string Alphabet { get; set; }
    public bool IncludeSpaces { get; set; }
  }
}
