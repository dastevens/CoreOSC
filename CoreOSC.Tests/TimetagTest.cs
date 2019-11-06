using NUnit.Framework;
using System;

namespace CoreOSC.Tests
{
    [TestFixture]
    public class TimetagTest
    {
        [TestCase]
        public void TestTimetag()
        {
            UInt64 time = (ulong)60 * (ulong)60 * (ulong)24 * (ulong)365 * (ulong)108;
            time = time << 32;
            time = time + (ulong)(Math.Pow(2, 32) / 2);
            var date = Utils.TimetagToDateTime(time);

            Assert.AreEqual(DateTime.Parse("2007-12-06 00:00:00.500"), date);
        }

        [TestCase]
        public void TestDateTimeToTimetag()
        {
            var dt = DateTime.Now;

            var l = Utils.DateTimeToTimetag(dt);
            var dtBack = Utils.TimetagToDateTime(l);

            Assert.AreEqual(dt.Date, dtBack.Date);
            Assert.AreEqual(dt.Hour, dtBack.Hour);
            Assert.AreEqual(dt.Minute, dtBack.Minute);
            Assert.AreEqual(dt.Second, dtBack.Second);
            Assert.AreEqual(dt.Millisecond, dtBack.Millisecond);
        }
    }
}