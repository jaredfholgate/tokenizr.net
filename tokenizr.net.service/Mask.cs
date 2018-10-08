using System;
using System.Collections.Generic;
using System.Linq;
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
      var advancedMask = mask.Contains("{{") && mask.Contains("}}");

      var startBraceCount = 0;
      var countOfItems = 0;
      var previousNumbers = string.Empty;

      foreach (var item in mask)
      {
        switch (item)
        {
          case '*':
            if (advancedMask && startBraceCount == 2 && countOfItems != 0)
            {
              for (var i = 0; i < countOfItems; i++)
              {
                maskObject.Items.Add(new MaskItem { MaskType = MaskType.ReplaceAny });
              }
            }
            else
            {
              maskObject.Items.Add(new MaskItem { MaskType = MaskType.ReplaceAny });
            }
            startBraceCount = 0;
            break;

          case '^':
            if (advancedMask && startBraceCount == 2 && countOfItems != 0)
            {
              for (var i = 0; i < countOfItems; i++)
              {
                maskObject.Items.Add(new MaskItem { MaskType = MaskType.KeepAnyOriginal });
              }
            }
            else
            {
              maskObject.Items.Add(new MaskItem { MaskType = MaskType.KeepAnyOriginal });
            }
            startBraceCount = 0;
            break;

          case '{':
            if (advancedMask)
            {
              startBraceCount += 1;
            }
            else
            {
              maskObject.Items.Add(new MaskItem { MaskType = MaskType.MustMatchAndKeep, Match = item });
            }
            break;

          case '}':
            if (advancedMask)
            {
              startBraceCount = 0;
              previousNumbers = string.Empty;
            }
            else
            {
              maskObject.Items.Add(new MaskItem { MaskType = MaskType.MustMatchAndKeep, Match = item });
            }
            break;

          default:
            if (startBraceCount == 2 && char.IsDigit(item))
            {
              previousNumbers = previousNumbers + item.ToString();
              countOfItems = int.Parse(previousNumbers);
            }
            else
            {
              maskObject.Items.Add(new MaskItem { MaskType = MaskType.MustMatchAndKeep, Match = item });
              startBraceCount = 0;
            }
            break;
        }
      }
      return maskObject;
    }
  }
}
