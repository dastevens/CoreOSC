using NUnit.Framework;
using System;
using System.Threading;

namespace CoreOSC.Tests
{
    [TestFixture]
    internal class ListenerTest
    {
        /// <summary>
        /// Opens a listener on a specified port, then closes it and attempts to open another on the same port
        /// Opening the second listener will fail unless the first one has been properly closed.
        /// </summary>
        [TestCase]
        public void CloseListener()
        {
            using (var l1 = new UDPListener(55555))
            {
                _ = l1.Receive();
            }

            using (var l2 = new UDPListener(55555))
            {
                _ = l2.Receive();
            }
        }

        /// <summary>
        /// Tries to open two listeners on the same port, results in an exception
        /// </summary>
        [TestCase]
        public void CloseListenerException()
        {
            bool ex = false;
            try
            {
                using (var l1 = new UDPListener(55555))
                {
                    _ = l1.Receive();
                    var l2 = new UDPListener(55555);
                }
            }
            catch (Exception)
            {
                ex = true;
            }

            Assert.IsTrue(ex);
        }

        /// <summary>
        /// Single message receive
        /// </summary>
        [TestCase]
        public void ListenerSingleMSG()
        {
            using (var listener = new UDPListener(55555))
            {
                var sender = new CoreOSC.UDPSender("localhost", 55555);

                var msg = new CoreOSC.OscMessage("/test/", 23.42f);

                sender.Send(msg);

                while (true)
                {
                    var pack = listener.Receive();
                    if (pack == null)
                        Thread.Sleep(1);
                    else
                        break;
                }
            }
        }

        /// <summary>
        /// Bombard the listener with messages, check if they are all received
        /// </summary>
        [TestCase]
        public void ListenerLoadTest()
        {
            using (var listener = new UDPListener(55555))
            {
                var sender = new CoreOSC.UDPSender("localhost", 55555);

                var msg = new CoreOSC.OscMessage("/test/", 23.42f);

                for (int i = 0; i < 1000; i++)
                    sender.Send(msg);

                for (int i = 0; i < 1000; i++)
                {
                    var receivedMessage = listener.Receive();
                    Assert.NotNull(receivedMessage);
                }
            }
        }
    }
}