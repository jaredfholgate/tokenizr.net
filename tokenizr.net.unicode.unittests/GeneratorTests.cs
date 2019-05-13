using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace tokenizr.net.unicode.unittests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void CanGenerateListOfUnicodeCharaters()
        {
            var result = new Generator().Generate();

            Assert.AreEqual(65535, result.Count);
            Assert.AreEqual('A', result[65]);
        }
    }
}
