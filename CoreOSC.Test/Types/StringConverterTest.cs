using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class StringConverterTest
    {
        [Test]
        public void SerializeZeroChars()
        {
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeOneChar()
        {
            var expectedOutput = new DWord[] { new DWord(97, 0, 0, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("a");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeTwoChars()
        {
            var expectedOutput = new DWord[] { new DWord(97, 98, 0, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("ab");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeThreeChars()
        {
            var expectedOutput = new DWord[] { new DWord(97, 98, 99, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("abc");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFourChars()
        {
            var expectedOutput = new DWord[] { new DWord(97, 98, 99, 100), new DWord(0, 0, 0, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("abcd");

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFiveChars()
        {
            var expectedOutput = new DWord[] { new DWord(97, 98, 99, 100), new DWord(101, 0, 0, 0) };
            var sut = new StringConverter();

            var result = sut.Serialize("abcde");

            Assert.AreEqual(expectedOutput, result);
        }

        [TestCase]
        public void DeserializeZeroDWords()
        {
            var input = new DWord[] { };
            var expectedDWords = new DWord[] { };
            var sut = new StringConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual("", value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [TestCase("", 0, 0, 0, 0)]
        [TestCase("a", 97, 0, 0, 0)]
        [TestCase("ab", 97, 98, 0, 0)]
        [TestCase("abc", 97, 98, 99, 0)]
        public void DeserializeOneDWord(
            string expectedValue,
            byte byte0,
            byte byte1,
            byte byte2,
            byte byte3)
        {
            var input = new DWord[] { new DWord(byte0, byte1, byte2, byte3) };
            var expectedDWords = new DWord[] { };
            var sut = new StringConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeTwoDWords()
        {
            var input = new DWord[] { new DWord(97, 98, 99, 100), new DWord(101, 0, 0, 0) };
            var expectedDWords = new DWord[] { };
            var sut = new StringConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual("abcde", value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
