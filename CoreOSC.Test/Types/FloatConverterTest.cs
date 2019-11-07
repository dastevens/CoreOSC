using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class FloatConverterTest
    {
        [TestCase(0, 0, 0, 0, 0.0f)]
        [TestCase(0x3F, 0x80, 0x00, 0x00, +1.0f)]
        [TestCase(0x40, 0x00, 0x00, 0x00, +2.0f)]
        [TestCase(0x40, 0x40, 0x00, 0x00, +3.0f)]
        [TestCase(0xC0, 0x40, 0x00, 0x00, -3.0f)]
        [TestCase(0x3F, 0x00, 0x00, 0x00, +0.5f)]
        [TestCase(0x41, 0x20, 0x00, 0x00, +10.0f)]
        [TestCase(0xC2, 0xC8, 0x00, 0x00, -100.0f)]
        [TestCase(0x40, 0x49, 0x0F, 0xF9, +3.1416f)]
        [TestCase(0xBF, 0xA0, 0x00, 0x00, -1.25f)]
        [TestCase(0xC3, 0x78, 0xC0, 0x00, -248.75f)]
        public void Serialize(
            byte expectedByte0,
            byte expectedByte1,
            byte expectedByte2,
            byte expectedByte3,
            float input)
        {
            var expectedOutput = new DWord[] { new DWord(expectedByte0, expectedByte1, expectedByte2, expectedByte3) };
            var sut = new FloatConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase(0.0f, 0, 0, 0, 0)]
        [TestCase(+1.0f, 0x3F, 0x80, 0x00, 0x00)]
        [TestCase(+2.0f, 0x40, 0x00, 0x00, 0x00)]
        [TestCase(+3.0f, 0x40, 0x40, 0x00, 0x00)]
        [TestCase(-3.0f, 0xC0, 0x40, 0x00, 0x00)]
        [TestCase(+0.5f, 0x3F, 0x00, 0x00, 0x00)]
        [TestCase(+10.0f, 0x41, 0x20, 0x00, 0x00)]
        [TestCase(-100.0f, 0xC2, 0xC8, 0x00, 0x00)]
        [TestCase(+3.1416f, 0x40, 0x49, 0x0F, 0xF9)]
        [TestCase(-1.25f, 0xBF, 0xA0, 0x00, 0x00)]
        [TestCase(-248.75f, 0xC3, 0x78, 0xC0, 0x00)]
        public void Deserialize(
            float expectedValue,
            byte byte0,
            byte byte1,
            byte byte2,
            byte byte3
            )
        {
            var input = new DWord[] { new DWord(byte0, byte1, byte2, byte3), new DWord(5, 6, 7, 8) };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new FloatConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
