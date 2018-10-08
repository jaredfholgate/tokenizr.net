using System;
using System.Collections.Generic;
using System.Text;

namespace tokenizr.net.service
{
  public class Mask
  {
    public Mask()
    {
      Items = new List<MaskItem>();
    }

    public int Length { get { return Items == null ? 0 : Items.Count; } }

    public List<MaskItem> Items { get; set; }

    public static Mask Parse(string mask)
    {
      var maskObject = new Mask();
      foreach (var item in mask)
      {
        switch (item)
        {
          case '*':
            maskObject.Items.Add(new MaskItem { MaskType = MaskType.ReplaceAny });
            break;

          case '^':
            maskObject.Items.Add(new MaskItem { MaskType = MaskType.KeepAnyOriginal });
            break;

          default:
            maskObject.Items.Add(new MaskItem { MaskType = MaskType.MustMatchAndKeep, Match = item });
            break;
        }
      }
      return maskObject;
    }
  }
}
