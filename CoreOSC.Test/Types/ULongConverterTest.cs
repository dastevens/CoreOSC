using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class ULongConverterTest
    {
        [TestCase(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0UL)]
        [TestCase(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 1UL)]
        [TestCase(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, ulong.MaxValue)]
        public void Serialize(
            byte expectedByte0,
            byte expectedByte1,
            byte expectedByte2,
            byte expectedByte3,
            byte expectedByte4,
            byte expectedByte5,
            byte expectedByte6,
            byte expectedByte7,
            ulong input)
        {
            var expectedOutput = new DWord[] {
                new DWord(expectedByte0, expectedByte1, expectedByte2, expectedByte3),
                new DWord(expectedByte4, expectedByte5, expectedByte6, expectedByte7),
            };
            var sut = new ULongConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase(0UL, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00)]
        [TestCase(1UL, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01)]
        [TestCase(ulong.MaxValue, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF)]
        public void Deserialize(
            ulong expectedValue,
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
                new DWord(5, 6, 7, 8)
            };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new ULongConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
