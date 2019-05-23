using System;
using System.Collections.Generic;
using System.Text;

namespace tokenizr.net
{
  /// <summary>
  /// Standard Client Types for Common Use Cases
  /// </summary>
  public enum BasicClientType
  {
    /// <summary>
    /// English letters and numbers only. Spaces, punctuation and special characters will not be replaced. Choose this for format preservation. Consistency can be defined for this.
    /// </summary>
    BasicEnglish,
    /// <summary>
    /// English with Spaces, Punctuation and Special Characters. Choose this for completely random string generation. Consistency can be defined for this.
    /// </summary>
    FullEnglish,
    /// <summary>
    /// Numbers 0 through 10 only. Spaces, punctuation and special characters will not be replaced. Choose this for numerical fields where you want format preservation. Consistency will be ignored for this.
    /// </summary>
    BasicNumbers,
    /// <summary>
    /// Numbers 0 through 10 only. Spaces, punctuation and special characters will not be replaced. A credit card mask is defined that will retain the last 4 digits and the dashes. Choose this for credit card numbers. Consistency will be ignored for this.
    /// </summary>
    CreditCard,
    /// <summary>
    /// Every unicode character for every language, with all numbers, punctuation and special characters.
    /// </summary>
    FullUnicode
  }
}
