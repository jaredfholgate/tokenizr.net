namespace tokenizr.net.service
{
  public interface IServiceSettings
  {
    Behaviour Behaviour { get; set; }
    Mask Mask { get; set; }
    int Cycles { get; set; }
  }
}