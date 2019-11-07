using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class BytesConverterTest
    {
        [Test]
        public void SerializeZeroBytes()
        {
            var input = new byte[] { };
            var expectedOutput = new DWord[] { };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeOneByte()
        {
            var input = new byte[] { 1 };
            var expectedOutput = new DWord[] { new DWord(1, 0, 0, 0) };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeTwoBytes()
        {
            var input = new byte[] { 1, 2 };
            var expectedOutput = new DWord[] { new DWord(1, 2, 0, 0) };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeThreeBytes()
        {
            var input = new byte[] { 1, 2, 3 };
            var expectedOutput = new DWord[] { new DWord(1, 2, 3, 0) };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFourBytes()
        {
            var input = new byte[] { 1, 2, 3, 4 };
            var expectedOutput = new DWord[] { new DWord(1, 2, 3, 4) };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFiveBytes()
        {
            var input = new byte[] { 1, 2, 3, 4, 5 };
            var expectedOutput = new DWord[] { new DWord(1, 2, 3, 4), new DWord(5, 0, 0, 0) };
            var sut = new BytesConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }


        [Test]
        public void DeserializeZeroDWords()
        {
            var input = new DWord[] { };
            var expectedValue = new byte[] { };
            var expectedDWords = new DWord[] { };
            var sut = new BytesConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeOneDWord()
        {
            var input = new DWord[] { new DWord(1, 2, 3, 4) };
            var expectedValue = new byte[] { 1, 2, 3, 4 };
            var expectedDWords = new DWord[] { };
            var sut = new BytesConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeTwoDWords()
        {
            var input = new DWord[] { new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var expectedDWords = new DWord[] { };
            var sut = new BytesConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
