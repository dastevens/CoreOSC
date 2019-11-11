using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreOSC.Types;
using NUnit.Framework;

namespace CoreOSC.Test.Types
{
    public class MidiConverterTest
    {
        [Test]
        public void Serialize()
        {
            var input = new Midi(1, 2, 3, 4);
            var expectedOutput = new DWord[] { new DWord(input.Port, input.Status, input.Data1, input.Data2) };
            var sut = new MidiConverter();

            var result = sut.Serialize(input);

            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void Deserialize()
        {
            var input = new DWord[] { new DWord(1, 2, 3, 4), new DWord(5, 6, 7, 8) };
            var expectedValue = new Midi(1, 2, 3, 4);
            var expectedDWords = new DWord[] { new DWord(5, 6, 7, 8) };
            var sut = new MidiConverter();

            var dWords = sut.Deserialize(input, out var value);

            Assert.AreEqual(expectedValue, value);
            Assert.AreEqual(expectedDWords, dWords);
        }
    }
}
