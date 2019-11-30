using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class VersionReply
    {
        public string ProgramName { get; set; }
        public int MajorVersionNumber { get; set; }
        public int MinorVersionNumber { get; set; }
        public string PatchVersionName { get; set; }
        public string GitBranchName { get; set; }
        public string CommitHash { get; set; }

        private readonly static Address versionReplyAddress = new Address("/version.reply");

        public static VersionReply FromMessage(OscMessage oscMessage)
        {
            if (oscMessage.Address.Equals(versionReplyAddress))
            {
                var arguments = oscMessage.Arguments.ToArray();
                if (arguments.Length == 6)
                {
                    return new VersionReply
                    {
                        ProgramName = (string)arguments[0],
                        MajorVersionNumber = (int)arguments[1],
                        MinorVersionNumber = (int)arguments[2],
                        PatchVersionName = (string)arguments[3],
                        GitBranchName = (string)arguments[4],
                        CommitHash = (string)arguments[5],
                    };
                }
                else
                {
                    throw new Exception($"{versionReplyAddress.Value} has {arguments.Length} arguments, expecting 6");
                }
            }
            else
            {
                throw new Exception($"Unknown message: expected {versionReplyAddress.Value}, but got {oscMessage.Address.Value}");
            }
        }

    }
}
