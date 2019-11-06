using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CoreOSC.Tests
{
    [TestFixture]
    internal class IntegrationTest
    {
        [TestCase]
        public void TestMessage()
        {
            using (var listener = new UDPListener(55555))
            {
                var sender = new CoreOSC.UDPSender("localhost", 55555);

                // Test every message type (except Symbol)
                var msg1 = new CoreOSC.OscMessage(
                    "/test/address",

                    23,
                    42.42f,
                    "hello world",
                    new byte[3] { 2, 3, 4 },
                    -123456789123,
                    new Timetag(DateTime.Now.Date).Tag,
                    new Timetag(DateTime.Now.Date.AddMonths(1)),
                    (double)1234567.890,
                    new Symbol("wut wut"),
                    (char)'x',
                    new RGBA(20, 40, 60, 255),
                    new Midi(3, 110, 55, 66),
                    true,
                    false,
                    null,
                    Double.PositiveInfinity
                );

                sender.Send(msg1);

                OscMessage msgRevc = null;
                int retries = 5;
                do
                {
                    Task.Delay(1).Wait();
                    msgRevc = listener.Receive() as OscMessage;
                } while (msgRevc == null && retries-- > 0);

                Assert.AreEqual("/test/address", msgRevc.Address);
                Assert.AreEqual(16, msgRevc.Arguments.Count);

                Assert.AreEqual(23, msgRevc.Arguments[0]);
                Assert.AreEqual(42.42f, msgRevc.Arguments[1]);
                Assert.AreEqual("hello world", msgRevc.Arguments[2]);
                Assert.AreEqual(new byte[3] { 2, 3, 4 }, msgRevc.Arguments[3]);
                Assert.AreEqual(-123456789123, msgRevc.Arguments[4]);
                Assert.AreEqual(new Timetag(DateTime.Now.Date), msgRevc.Arguments[5]);
                Assert.AreEqual(new Timetag(DateTime.Now.Date.AddMonths(1)), msgRevc.Arguments[6]);
                Assert.AreEqual((double)1234567.890, msgRevc.Arguments[7]);
                Assert.AreEqual(new Symbol("wut wut"), msgRevc.Arguments[8]);
                Assert.AreEqual((char)'x', msgRevc.Arguments[9]);
                Assert.AreEqual(new RGBA(20, 40, 60, 255), msgRevc.Arguments[10]);
                Assert.AreEqual(new Midi(3, 110, 55, 66), msgRevc.Arguments[11]);
                Assert.AreEqual(true, msgRevc.Arguments[12]);
                Assert.AreEqual(false, msgRevc.Arguments[13]);
                Assert.AreEqual(null, msgRevc.Arguments[14]);
                Assert.AreEqual(Double.PositiveInfinity, msgRevc.Arguments[15]);
            }
        }

        [TestCase]
        public void TestBundle()
        {
            using (var listener = new UDPListener(55555))
            {
                var sender1 = new CoreOSC.UDPSender("localhost", 55555);
                var msg1 = new CoreOSC.OscMessage("/test/address1", 23, 42.42f, "hello world", new byte[3] { 2, 3, 4 });
                var msg2 = new CoreOSC.OscMessage("/test/address2", 34, 24.24f, "hello again", new byte[5] { 5, 6, 7, 8, 9 });
                var dt = DateTime.Now;
                var bundle = new CoreOSC.OscBundle(Utils.DateTimeToTimetag(dt), msg1, msg2);

                sender1.Send(bundle);
                OscBundle recv = null;
                int retries = 5;
                do
                {
                    Task.Delay(1).Wait();
                    recv = listener.Receive() as OscBundle;
                } while (recv == null && retries-- > 0);

                Assert.AreEqual(dt.Date, recv.Timestamp.Date);
                Assert.AreEqual(dt.Hour, recv.Timestamp.Hour);
                Assert.AreEqual(dt.Minute, recv.Timestamp.Minute);
                Assert.AreEqual(dt.Second, recv.Timestamp.Second);
                Assert.AreEqual(dt.Millisecond, recv.Timestamp.Millisecond);

                Assert.AreEqual("/test/address1", recv.Messages[0].Address);
                Assert.AreEqual(4, recv.Messages[0].Arguments.Count);
                Assert.AreEqual(23, recv.Messages[0].Arguments[0]);
                Assert.AreEqual(42.42f, recv.Messages[0].Arguments[1]);
                Assert.AreEqual("hello world", recv.Messages[0].Arguments[2]);
                Assert.AreEqual(new byte[3] { 2, 3, 4 }, recv.Messages[0].Arguments[3]);

                Assert.AreEqual("/test/address2", recv.Messages[1].Address);
                Assert.AreEqual(4, recv.Messages[1].Arguments.Count);
                Assert.AreEqual(34, recv.Messages[1].Arguments[0]);
                Assert.AreEqual(24.24f, recv.Messages[1].Arguments[1]);
                Assert.AreEqual("hello again", recv.Messages[1].Arguments[2]);
                Assert.AreEqual(new byte[5] { 5, 6, 7, 8, 9 }, recv.Messages[1].Arguments[3]);
            }
        }
    }
}