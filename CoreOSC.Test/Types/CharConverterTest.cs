using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class CharConverterTest
    {
        [Test]
        public void Serialize()
        {
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 97) };
            var sut = new CharConverter();

            var result = sut.Serialize('a');

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Deserialize()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 97), new DWord(5, 6, 7, 8) };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new CharConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual('a', value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
