using System;

namespace CoreOSC.Tests
{
    internal class Program
    {
        private static void Main()
        {
            /*var message = new CoreOSC.OscMessage("/Knob", 0.5f);
			var sender = new CoreOSC.UDPSender("127.0.0.1", 10000);

			while (true)
			{
				var inp = Console.ReadLine();
				float f = Convert.ToSingle(inp);
				message.Arguments[0] = f;
				sender.Send(message);
			}*/

            void cb(OscPacket packet)
            {
                var msg = ((OscBundle)packet).Messages[0];
                Console.WriteLine(msg.Arguments[0].ToString());
            }

            using (var l1 = new UDPListener(10001, cb)) ;

            Console.ReadLine();
        }
    }
}