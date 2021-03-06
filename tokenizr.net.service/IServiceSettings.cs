﻿namespace tokenizr.net.service
{
  public interface IServiceSettings
  {
    Behaviour Behaviour { get; set; }
    Mask Mask { get; set; }
    int Cycles { get; set; }
    bool SeedPerCycle { get; set; }
    bool Encrypt { get; set; }
    string Key { get; set; }
    string IV { get; set; }
  }
}