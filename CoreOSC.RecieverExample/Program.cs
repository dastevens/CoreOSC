using System;
using CoreOSC;
using System.Threading;

namespace CoreOSC.RecieverExample
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;

            /**
             * Callback handle that will handle 
             * every message recieved from Behringer X Air */
            HandleOscPacket callback = delegate (OscPacket packet)
            {
                var messageReceived = (OscMessage)packet;
                Console.Write(++counter + "#");
                Console.Write(messageReceived.Address.ToString());

                foreach (var test in messageReceived.Arguments)
                {
                    Console.Write("," + test.ToString());
                }

                Console.WriteLine("");
            };

            /**
             * Connect to remote Behringer X Air mixer
             * Remote port is 10024 for X Air
             * Local port (8000) can be whatever you would like
             * Callback is called each time we recieve a message from Behringer */
            using (var duplex = new UDPDuplex("192.168.64.64", 10024, 8000, callback))
            {
                /**
                 * Behringer must recieve a xremote message every 10th second 
                 * for the client to be subscribed to all changes that happens to mixer.
                 * Behringer only supports up too 4 subscribers */
                var message = new CoreOSC.OscMessage("/xremote");
                void xremote()
                {
                    while (true)
                    {
                        duplex.Send(message);
                        Thread.Sleep(8000);
                    }
                }
                new Thread(() => xremote()) { IsBackground = true }.Start();

                /**
                 * Example on how to send a message to an Behringer.
                 * Then message is sent. Behringer will return a message because of /xremote above
                 * and callback will be called, printing out the new message recieved */
                var getMessage = new CoreOSC.OscMessage("/ch/01/mix/fader", 1024);
                while (true)
                {
                    Console.WriteLine("Press enter to set Fader to Max for Channel 1");
                    Console.ReadKey();
                    duplex.Send(getMessage);
                }
            }
        }
    }
}
