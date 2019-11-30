using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class StatusReply
    {
        private readonly static Address statusReplyAddress = new Address("/status.reply");

        //int	1. unused.
        public int Unused { get; set; }

        //int number of unit generators.
        public int NumberOfUnitGenerators { get; set; }

        //int number of synths.
        public int NumberOfSynths { get; set; }

        //int number of groups.
        public int NumberOfGroups { get; set; }

        //int number of loaded synth definitions.
        public int NumberOfLoadedSynthDefinitions { get; set; }

        //float average percent CPU usage for signal processing
        public float AveragePercentCPUUsage { get; set; }

        //float peak percent CPU usage for signal processing
        public float PeakPercentCPUUsage { get; set; }

        //double nominal sample rate
        public double NominalSampleRate { get; set; }

        //double actual sample rate
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
                        Unused = (int)arguments[0],
                        NumberOfUnitGenerators = (int)arguments[1],
                        NumberOfSynths = (int)arguments[2],
                        NumberOfGroups = (int)arguments[3],
                        NumberOfLoadedSynthDefinitions = (int)arguments[4],
                        AveragePercentCPUUsage = (float)arguments[5],
                        PeakPercentCPUUsage = (float)arguments[6],
                        NominalSampleRate = (double)arguments[7],
                        ActualSampleRate = (double)arguments[8]
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
