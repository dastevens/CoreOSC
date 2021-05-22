using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class StatusReply
    {
        private readonly static Address statusReplyAddress = new Address("/status.reply");

        public int Unused { get; set; }
        public int NumberOfUnitGenerators { get; set; }
        public int NumberOfSynths { get; set; }
        public int NumberOfGroups { get; set; }
        public int NumberOfLoadedSynthDefinitions { get; set; }
        public float AveragePercentCPUUsage { get; set; }
        public float PeakPercentCPUUsage { get; set; }
        public double NominalSampleRate { get; set; }
        public double ActualSampleRate { get; set; }

        public static StatusReply FromMessage(OscMessage oscMessage)
        {
            if (oscMessage.Address.Equals(statusReplyAddress))
            {
                var arguments = oscMessage.Arguments.ToArray();
                if (arguments.Length == 9)
                {
                    return new StatusReply
                    {
                        Unused = Convert.ToInt32(arguments[0]),
                        NumberOfUnitGenerators = Convert.ToInt32(arguments[1]),
                        NumberOfSynths = Convert.ToInt32(arguments[2]),
                        NumberOfGroups = Convert.ToInt32(arguments[3]),
                        NumberOfLoadedSynthDefinitions = Convert.ToInt32(arguments[4]),
                        AveragePercentCPUUsage = Convert.ToSingle(arguments[5]),
                        PeakPercentCPUUsage = Convert.ToSingle(arguments[6]),
                        NominalSampleRate = Convert.ToDouble(arguments[7]),
                        ActualSampleRate = Convert.ToDouble(arguments[8]),
                    };
                }
                else
                {
                    throw new Exception($"{statusReplyAddress.Value} has {arguments.Length} arguments, expecting 9");
                }
            }
            else
            {
                throw new Exception($"Unknown message: expected {statusReplyAddress.Value}, but got {oscMessage.Address.Value}");
            }
        }
    }
}
