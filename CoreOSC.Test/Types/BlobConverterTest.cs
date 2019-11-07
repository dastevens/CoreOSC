using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class BlobConverterTest
    {
        [Test]
        public void SerializeZeroBytes()
        {
            var input = new byte[] { };
            var expectedOutput = new DWord[] { new DWord() };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeOneByte()
        {
            var input = new byte[] { 1 };
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 1), new DWord(1, 0, 0, 0) };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeTwoBytes()
        {
            var input = new byte[] { 1, 2 };
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 2), new DWord(1, 2, 0, 0) };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeThreeBytes()
        {
            var input = new byte[] { 1, 2, 3 };
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 3), new DWord(1, 2, 3, 0) };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFourBytes()
        {
            var input = new byte[] { 1, 2, 3, 4 };
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 4), new DWord(1, 2, 3, 4) };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void SerializeFiveBytes()
        {
            var input = new byte[] { 1, 2, 3, 4, 5 };
            var expectedOutput = new DWord[] { new DWord(0, 0, 0, 5), new DWord(1, 2, 3, 4), new DWord(5, 0, 0, 0) };
            var sut = new BlobConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }


        [Test]
        public void DeserializeLengthZero()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 0) };
            var expectedValue = new byte[] { };
            var expectedDWords = new DWord[] { };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeLengthOne()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 1), new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1 };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeLengthTwo()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 2), new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1, 2 };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeLengthThree()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 3), new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1, 2, 3 };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeLengthFour()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 4), new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1, 2, 3, 4 };
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }

        [Test]
        public void DeserializeLengthFive()
        {
            var input = new DWord[] { new DWord(0, 0, 0, 5), new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new byte[] { 1, 2, 3, 4, 5 };
            var expectedDWords = new DWord[] { };
            var sut = new BlobConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
