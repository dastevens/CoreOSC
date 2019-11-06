using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreOSC.Tests
{
    [TestFixture]
    internal class ParseTest
    {
        [TestCase]
        public void TestDouble()
        {
            var val = 1234567.2324521e36;

            var msg = new OscMessage("/test/1", val);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);
            Assert.AreEqual(val, ((double)msg2.Arguments[0]));
        }

        [TestCase]
        public void TestBlob()
        {
            var blob = new byte[5] { 23, 65, 255, 12, 6 };

            var msg = new OscMessage("/test/1", blob);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);
            Assert.AreEqual(blob, ((byte[])msg2.Arguments[0]));
        }

        [TestCase]
        public void TestTimetag()
        {
            var val = DateTime.Now;
            var tag = Timetag.FromDateTime(val);

            var msg = new OscMessage("/test/1", tag);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);
            Assert.AreEqual(tag.Tag, ((Timetag)msg2.Arguments[0]).Tag);
        }

        [TestCase]
        public void TestLong()
        {
            var num = 123456789012345;
            var msg = new OscMessage("/test/1", num);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);

            Assert.AreEqual(num, msg2.Arguments[0]);
        }

        [TestCase]
        public void TestArray()
        {
            var list = new List<object>() { 23, true, "hello world" };
            var msg = new OscMessage("/test/1", 9999, list, 24.24f);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);

            Assert.AreEqual(9999, msg2.Arguments[0]);
            Assert.AreEqual(list, msg2.Arguments[1]);
            Assert.AreEqual(list.Count, ((List<object>)(msg2.Arguments[1])).Count);
            Assert.AreEqual(24.24f, msg2.Arguments[2]);
        }

        [TestCase]
        public void TestNoAddress()
        {
            var msg = new OscMessage("", 9999, 24.24f);
            var bytes = msg.GetBytes();

            var msg2 = (OscMessage)OscPacket.GetPacket(bytes);

            Assert.AreEqual("", msg2.Address);
            Assert.AreEqual(9999, msg2.Arguments[0]);
            Assert.AreEqual(24.24f, msg2.Arguments[1]);
        }
    }
}