using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class IntConverterTest
    {
        [TestCase(0x00, 0x00, 0x00, 0x00, 0)]
        [TestCase(0x00, 0x00, 0x00, 0x01, 1)]
        [TestCase(0xFF, 0xFF, 0xFF, 0xFF, -1)]
        [TestCase(0x7F, 0xFF, 0xFF, 0xFF, int.MaxValue)]
        [TestCase(0x80, 0x00, 0x00, 0x00, int.MinValue)]
        public void Serialize(
            byte expectedByte0,
            byte expectedByte1,
            byte expectedByte2,
            byte expectedByte3,
            int input)
        {
            var expectedOutput = new DWord[] { new DWord(expectedByte0, expectedByte1, expectedByte2, expectedByte3) };
            var sut = new IntConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase(0, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(1, 0x00, 0x00, 0x00, 0x01)]
        [TestCase(-1, 0xFF, 0xFF, 0xFF, 0xFF)]
        [TestCase(int.MaxValue, 0x7F, 0xFF, 0xFF, 0xFF)]
        [TestCase(int.MinValue, 0x80, 0x00, 0x00, 0x00)]
        public void Deserialize(
            int expectedValue,
            byte byte0,
            byte byte1,
            byte byte2,
            byte byte3
            )
        {
            var input = new DWord[] { new DWord(byte0, byte1, byte2, byte3), new DWord(5, 6, 7, 8) };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new IntConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
