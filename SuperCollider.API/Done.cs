namespace SuperCollider.API
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CoreOSC;

    public class Done
    {
        private readonly static Address doneAddress = new Address("/done");

        public static Done FromMessage(OscMessage oscMessage)
        {
            if (oscMessage.Address.Equals(doneAddress))
            {
                return new Done();
            }
            else
            {
                throw new Exception($"Unknown message: expected {doneAddress.Value}, but got {oscMessage.Address.Value}");
            }
        }
    }
}
