using CoreOSC.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreOSC.Test.Types
{
    class OscMessageTest
    {
        [Test]
        public void DeserializeEmptyMessage()
        {
            var dWords = new DWord[]
            {
                new DWord(47, 0, 0, 0),
                new DWord(44, 0, 0, 0),
                new DWord(1, 2, 3, 4),
            };
            var expectedResult = new DWord[]
            {
                new DWord(1, 2, 3, 4),
            };
            var expectedValue = new OscMessage(new Address("/"), new object[0]);

            var sut = new OscMessageConverter();
            var result = sut.Deserialize(dWords, out var value);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedValue.Address, value.Address);
            Assert.AreEqual(expectedValue.Arguments, value.Arguments);
        }

        [Test]
        public void DeserializeOscFalse()
        {
            var dWords = new DWord[]
            {
                new DWord(47, 0, 0, 0),
                new DWord(44, 70, 0, 0),
                new DWord(1, 2, 3, 4),
            };
            var expectedResult = new DWord[]
            {
                new DWord(1, 2, 3, 4),
            };
            var expectedValue = new OscMessage(new Address("/"), new object[] { OscFalse.False });

            var sut = new OscMessageConverter();
            var result = sut.Deserialize(dWords, out var value);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedValue.Address, value.Address);
            Assert.AreEqual(expectedValue.Arguments, value.Arguments);
        }

        [Test]
        public void SerializeEmptyMessage()
        {
            var input = new OscMessage(new Address("/"), new object[0]);
            var expectedResult = new DWord[]
            {
                new DWord(47, 0, 0, 0),
                new DWord(44, 0, 0, 0),
            };

            var sut = new OscMessageConverter();
            var result = sut.Serialize(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void SerializeOscFalse()
        {
            var input = new OscMessage(new Address("/"), new object[] { OscFalse.False });
            var expectedResult = new DWord[]
            {
                new DWord(47, 0, 0, 0),
                new DWord(44, 70, 0, 0),
            };

            var sut = new OscMessageConverter();
            var result = sut.Serialize(input);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
