using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace tokenizr.net.unittests
{
  [TestClass]
  public class BasicClientFactoryTests
  {
    [TestMethod]
    public void CanGenerateABasicEnglishBasicClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.BasicEnglish);
      var testString = "abc,def,ghi,123???{{}}";
      var result = client.Tokenize(testString).Value;

      Assert.AreNotEqual(testString, result);
      for (int i = 0; i < result.Length; i++)
      {
        if(",?{}".ToCharArray().Contains(testString[i]))
        {
          Assert.AreEqual(testString[i], result[i]);
        }
      }

      result = client.Detokenize(result).Value;
      Assert.AreEqual(testString, result);
    }

    [TestMethod]
    public void CanGenerateABasicNumberBasicClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.BasicNumbers);
      var testString = "abc,def,ghi,123???{{}}";
      var result = client.Tokenize(testString).Value;
      Assert.AreNotEqual(testString, result);
      for (int i = 0; i < result.Length; i++)
      {
        if ("abcdefghi,?{}".ToCharArray().Contains(testString[i]))
        {
          Assert.AreEqual(testString[i], result[i]);
        }
      }
      result = client.Detokenize(result).Value;
      Assert.AreEqual(testString, result);
    }

    [TestMethod]
    public void CanGenerateAFullEnglishBasicClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullEnglish);
      var testString = "abc,def,ghi,123???{{}}";
      var result = client.Tokenize(testString).Value;
      Assert.AreNotEqual(testString, result);

      result = client.Detokenize(result).Value;
      Assert.AreEqual(testString, result);
    }

    [TestMethod]
    public void CanGenerateACreditCardBasicClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.CreditCard);
      var testString = "1234-5678-9012-3456";
      var result = client.Tokenize(testString).Value;
      Assert.AreNotEqual(testString, result);
      for (int i = 0; i < result.Length; i++)
      {
        if (i == 15 || i == 16 || i == 17 || i == 18 || testString[i] == '-')
        {
          Assert.AreEqual(testString[i], result[i]);
        }
      }
      result = client.Detokenize(result).Value;
      Assert.AreEqual(testString, result);
    }
  }
}
