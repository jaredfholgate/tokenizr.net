namespace tokenizr.net.compression
{
  public interface ICompression
  {
    string Compress(string source);
    string Decompress(string source);
  }
}