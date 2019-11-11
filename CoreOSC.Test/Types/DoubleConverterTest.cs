using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class DoubleConverterTest
    {
        [TestCase(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, +0.0)]
        [TestCase(0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, +1.0)]
        [TestCase(0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, +2.0)]
        [TestCase(0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, -0.0)]
        [TestCase(0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, -1.0)]
        [TestCase(0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, -2.0)]
        public void Serialize(
            byte expectedByte0,
            byte expectedByte1,
            byte expectedByte2,
            byte expectedByte3,
            byte expectedByte4,
            byte expectedByte5,
            byte expectedByte6,
            byte expectedByte7,
            double input)
        {
            var expectedOutput = new DWord[] {
                new DWord(expectedByte0, expectedByte1, expectedByte2, expectedByte3),
                new DWord(expectedByte4, expectedByte5, expectedByte6, expectedByte7),
            };
            var sut = new DoubleConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase(+0.0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(+1.0, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(+2.0, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(-0.0, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(-1.0, 0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(-2.0, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        public void Deserialize(
            double expectedValue,
            byte byte0,
            byte byte1,
            byte byte2,
            byte byte3,
            byte byte4,
            byte byte5,
            byte byte6,
            byte byte7
            )
        {
            var input = new DWord[] {
                new DWord(byte0, byte1, byte2, byte3),
                new DWord(byte4, byte5, byte6, byte7),
                new DWord(0xFF, 0xFF, 0xFF, 0xFF) };
            var expectedDWords = new DWord[] { new DWord(0xFF, 0xFF, 0xFF, 0xFF) };
            var sut = new DoubleConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
