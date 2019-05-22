﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        resultString = client.Detokenize(resultString, result.Seed).Value;

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
      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void PerformanceTestAFullUnicodeBasicClientDetonkenizeAsync()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullUnicode, Behaviour.RandomSeedInconsistent);
      var testString = "I was walking down the street and this happended! ÅßęœŖƢǆǢʥˎˢ˦ϛφϡϠ؅قـؼᵬᵾᶦᾑᾤבּ꭛ﻻ⽪⾀";
      var testStrings = new List<string>();
      for (var i = 0; i < 1000; i++)
      {
        testStrings.Add(testString + i);
      }

      var resultT = client.TokenizeAsync(testStrings).Result;

      var testRequests = resultT.Select(o => new BasicRequest { Source = o.Value, Seed = o.Seed }).ToList();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var resultD = client.DetokenizeAsync(testRequests).Result;

      stopwatch.Stop();
      Console.WriteLine(stopwatch.Elapsed);
      Assert.IsTrue(stopwatch.ElapsedMilliseconds < 10000);
    }

    [TestMethod]
    public void ThrowsAnExceptionWithAFullUnicodeBasicClientAndNoSeedForDetokenise()
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
      Assert.ThrowsException<ArgumentException>(() => resultString = client.Detokenize(resultString).Value, "A valid seed is required to detonkenize a token created using Random Seed tokenization.");
    }
    
    [TestMethod]
    public void CanSerliaseAndDeserialiseClient()
    {
      var client = BasicClientFactory.GetClient(BasicClientType.FullEnglish);
      var testString = "abc,def,ghi,123???{{}}";
      var encryptionKey = "TestKey";
      var result1 = client.Tokenize(testString).Value;
      var serliasedClient = client.Serialise(encryptionKey);

      client = new BasicClientFactory().Deserialise(encryptionKey, serliasedClient);
      var result2 = client.Tokenize(testString).Value;
      Assert.AreEqual(result1, result2);

      result2 = client.Detokenize(result2).Value;
      Assert.AreEqual(testString, result2);

    }
  }
}
