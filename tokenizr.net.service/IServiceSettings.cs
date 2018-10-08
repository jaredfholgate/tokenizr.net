namespace tokenizr.net.service
{
  public interface IServiceSettings
  {
    bool Consistent { get; set; }

    Mask Mask { get; set; }
  }
}