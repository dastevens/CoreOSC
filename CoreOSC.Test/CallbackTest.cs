using NUnit.Framework;
using System;
using System.Threading;

namespace CoreOSC.Tests
{
    [TestFixture]
    internal class CallbackTest
    {
        [TestCase]
        public void TestCallback()
        {
            var cbCalled = false;
            // The cabllback function
            void cb(OscPacket packet)
            {
                var msg = (OscMessage)packet;
                Assert.AreEqual(2, msg.Arguments.Count);
                Assert.AreEqual(23, msg.Arguments[0]);
                Assert.AreEqual("hello world", msg.Arguments[1]);
                cbCalled = true;
            }

            using (var l1 = new UDPListener(55555, cb))
            {
                var sender = new CoreOSC.UDPSender("localhost", 55555);
                var msg1 = new CoreOSC.OscMessage("/test/address", 23, "hello world");
                sender.Send(msg1);

                // Wait until callback processes its message
                var start = DateTime.Now;
                while (cbCalled == false && start.AddSeconds(2) > DateTime.Now)
                    Thread.Sleep(1);

                Assert.IsTrue(cbCalled);
            }
        }

        [TestCase]
        public void TestByteCallback()
        {
            var cbCalled = false;
            // The cabllback function
            void cb(byte[] packet)
            {
                var msg = (OscMessage)OscPacket.GetPacket(packet);
                Assert.AreEqual(2, msg.Arguments.Count);
                Assert.AreEqual(23, msg.Arguments[0]);
                Assert.AreEqual("hello world", msg.Arguments[1]);
                cbCalled = true;
            }

            using (var l1 = new UDPListener(55555, cb))
            {
                var sender = new CoreOSC.UDPSender("localhost", 55555);
                var msg1 = new CoreOSC.OscMessage("/test/address", 23, "hello world");
                sender.Send(msg1);

                // Wait until callback processes its message
                var start = DateTime.Now;
                while (cbCalled == false && start.AddSeconds(2) > DateTime.Now)
                    Thread.Sleep(1);

                Assert.IsTrue(cbCalled);
            }
        }
    }
}