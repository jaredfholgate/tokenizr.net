namespace tokenizr.net.service
{
  public class ServiceSettings : IServiceSettings
  {
    public ServiceSettings()
    {
      Cycles = 256;
      Behaviour = Behaviour.LengthBasedInconsistent;
    }
    public Behaviour Behaviour { get; set; }
    public Mask Mask { get; set; }
    public int Cycles { get; set; }
  }
}
