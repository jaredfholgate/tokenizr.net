using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using tokenizr.net.service;

namespace tokenizr.net.unittests
{
  [TestClass]
  public class BasicClientFactoryTests
  {
    private const string Key = "dfkjhsdi8y8w9efyiuwhfp8wef8we";
    private const string IV = "dsdfsdfsdfsdfsd";

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

      result = client.Detokenize(BasicRequest.FromString(result)).Value;
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
      result = client.Detokenize(BasicRequest.FromString(result)).Value;
      Assert.AreEqual(testString, result);
    }

    [TestMethod]
    public void CanGenerateAFullEnglishBasicClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullEnglish);
      var testString = "abc,def,ghi,123???{{}}";
      var result = client.Tokenize(testString).Value;
      Assert.AreNotEqual(testString, result);

      result = client.Detokenize(BasicRequest.FromString(result)).Value;
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
      result = client.Detokenize(BasicRequest.FromString(result)).Value;
      Assert.AreEqual(testString, result);
    }

    [TestMethod]
    public void CanGenerateAFullUnicodeBasicClient()
    {
        var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent);
        var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
        var result = client.Tokenize(testString);
        var resultString = result.Value;

        Assert.AreNotEqual(testString, resultString);
        for (int i = 0; i < resultString.Length; i++)
        {
           Assert.AreNotEqual(testString[i], resultString[i]);
        }
        resultString = client.Detokenize(new BasicRequest(resultString, result.Seed)).Value;

        Assert.AreEqual(testString, resultString);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientTokenizeAsync()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultT = client.TokenizeAsync(testStrings).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(!resultT.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientTokenizeAsyncWithEncryption()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, encrypt: true, key: Key, iv: IV);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultT = client.TokenizeAsync(testStrings).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(!resultT.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientDetonkenizeAsync()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎ";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        var postFix = i.ToString();
        var test = string.Concat(testString, postFix);
        testStrings.Add(test);
      }

      var resultT = client.TokenizeAsync(testStrings).Result;

      var testRequests = resultT.Select(o => new BasicRequest { Source = o.Value, Seed = o.Seed }).ToList();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultD = client.DetokenizeAsync(testRequests).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(resultD.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientDetonkenizeAsyncWithEncryption()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, encrypt: true, key: Key, iv: IV);
      var testString = "Hello, this is a test string.";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var resultT = client.TokenizeAsync(testStrings).Result;

      var testRequests = resultT.Select(o => new BasicRequest { Source = o.Value }).ToList();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultD = client.DetokenizeAsync(testRequests).Result;

      stopwatch.Stop();

      foreach(var test in testStrings)
      {
        Assert.IsTrue(resultD.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientTokenizeAsyncSeedPerCycle()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, seedPerCycle: true);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultT = client.TokenizeAsync(testStrings).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(!resultT.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientTokenizeAsyncWithEncryptionSeedPerCycle()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, seedPerCycle: true, encrypt: true, key: Key, iv: IV);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultT = client.TokenizeAsync(testStrings).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(!resultT.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientDetonkenizeAsyncSeedPerCycle()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, seedPerCycle: true);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎ";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        var postFix = i.ToString();
        var test = string.Concat(testString, postFix);
        testStrings.Add(test);
      }

      var resultT = client.TokenizeAsync(testStrings).Result;

      var testRequests = resultT.Select(o => new BasicRequest { Source = o.Value, Seed = o.Seed }).ToList();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultD = client.DetokenizeAsync(testRequests).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(resultD.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientDetonkenizeAsyncWithEncryptionSeedPerCycle()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent, seedPerCycle:true, encrypt: true, key: Key, iv: IV);
      var testString = "Hello, this is a test string.";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var resultT = client.TokenizeAsync(testStrings).Result;

      var testRequests = resultT.Select(o => new BasicRequest { Source = o.Value }).ToList();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultD = client.DetokenizeAsync(testRequests).Result;

      stopwatch.Stop();

      foreach (var test in testStrings)
      {
        Assert.IsTrue(resultD.Any(o => o.Value == test));
      }

      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void ThrowsAnExceptionWithAFullUnicodeBasicClientAndNoSeedForDetokenise()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎ";
      var result = client.Tokenize(testString);
      var resultString = result.Value;

      Assert.AreNotEqual(testString, resultString);
      for (int i = 0; i < resultString.Length; i++)
      {
        Assert.AreNotEqual(testString[i], resultString[i]);
      }
      Assert.ThrowsException<ArgumentException>(() => resultString = client.Detokenize(BasicRequest.FromString(resultString)).Value, "A valid seed is required to detonkenize a token created using Random Seed tokenization.");
    }
    
    [TestMethod]
    public void CanSerliaseAndDeserialiseClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullEnglish);
      var testString = "abc,def,ghi,123???{{}}";
      var key = "sdagdafghrtrte453tg34tdfhfdshdf34t34b45EQhfghjhgfrtyeghRWEW9234r";
      var iv = "fdg54g45yTHR54y45yG45g4g";
      var result1 = client.Tokenize(testString).Value;
      var serliasedClient = client.Serialise(key, iv);

      client = new BasicClientFactory().Deserialise(key, iv, serliasedClient);
      var result2 = client.Tokenize(testString).Value;
      Assert.AreEqual(result1, result2);

      result2 = client.Detokenize(BasicRequest.FromString(result2)).Value;
      Assert.AreEqual(testString, result2);

    }
  }
}
