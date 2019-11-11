using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class TimetagConverterTest
    {
        [Test]
        public void Deserialize()
        {
            var input = new DWord[] {
                new DWord(1, 2, 3, 4),
                new DWord(5, 6, 7, 8),
                new DWord(9, 10, 11, 12)
            };
            var expectedDWords = new DWord[] { new DWord(9, 10, 11, 12) };
            var expectedValue = new Timetag(0x0102030405060708);

            var sut = new TimetagConverter();
            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void Serialize()
        {
            var value = new Timetag(0x0102030405060708);
            var expectedResult = new DWord[] {
                new DWord(1, 2, 3, 4),
                new DWord(5, 6, 7, 8),
            };

            var sut = new TimetagConverter();
            var result = sut.Serialize(value);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
