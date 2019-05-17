namespace tokenizr.net.service
{
  public class ServiceSettings : IServiceSettings
  {
    public ServiceSettings()
    {
      Behaviour = Behaviour.LengthBasedInconsistent;
    }
    public Behaviour Behaviour { get; set; }
    public Mask Mask { get; set; }
  }
}
