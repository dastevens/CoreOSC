using CoreOSC.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreOSC.Test.Types
{
    class OscBundleTest
    {
        [Test]
        public void DeserializeEmptyBundle()
        {
            var input = new[]
            {
                new DWord(
                    (byte)'#',
                    (byte)'b',
                    (byte)'u',
                    (byte)'n'),
                new DWord(
                    (byte)'d',
                    (byte)'l',
                    (byte)'e',
                    (byte)'\0'),
                new DWord(0x01, 0x23, 0x45, 0x67),
                new DWord(0x89, 0xab, 0xcd, 0xef),
                new DWord(0, 0, 0, 0),
                new DWord(0xff, 0xff, 0xff, 0xff),
            };
            var expectedValue = new OscBundle(new Timetag(0x0123456789abcdef), new OscMessage[0]);
            var expectedResult = new[] { new DWord(0xff, 0xff, 0xff, 0xff) };

            var sut = new OscBundleConverter();
            var result = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue.Messages, value.Messages);
            Assert.AreEqual(expectedValue.Timetag, value.Timetag);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SerializeEmptyBundle()
        {
            var value = new OscBundle(new Timetag(0x0123456789abcdef), new OscMessage[0]);
            var expectedResult = new[]
            {
                new DWord(
                    (byte)'#',
                    (byte)'b',
                    (byte)'u',
                    (byte)'n'),
                new DWord(
                    (byte)'d',
                    (byte)'l',
                    (byte)'e',
                    (byte)'\0'),
                new DWord(0x01, 0x23, 0x45, 0x67),
                new DWord(0x89, 0xab, 0xcd, 0xef),
                new DWord(0, 0, 0, 0),
            };

            var sut = new OscBundleConverter();
            var result = sut.Serialize(value);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
