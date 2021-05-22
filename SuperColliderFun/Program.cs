using CoreOSC;
using CoreOSC.IO;
using SuperCollider.API;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SuperColliderFun
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var port = args.Length > 0 ? Convert.ToInt32(args[0]) : 57100;

            var cancellationTokenSource = new CancellationTokenSource();

            using (var udpClient = new UdpClient("127.0.0.1", port))
            {
                var client = new Client(udpClient);

                Console.WriteLine("Querying version...");
                var versionReply = await client.Version();

                Console.WriteLine($"ProgramName = {versionReply.ProgramName}");
                Console.WriteLine($"Version = {versionReply.MajorVersionNumber}.{versionReply.MinorVersionNumber}{versionReply.PatchVersionName}");
                Console.WriteLine($"Commit = {versionReply.GitBranchName} {versionReply.CommitHash}");

                Console.WriteLine("Querying status...");
                var statusReply = await client.Status();

                Console.WriteLine($"Unused = {statusReply.Unused}");
                Console.WriteLine($"NumberOfUnitGenerators = {statusReply.NumberOfUnitGenerators}");
                Console.WriteLine($"NumberOfSynths = {statusReply.NumberOfSynths}");
                Console.WriteLine($"NumberOfGroups = {statusReply.NumberOfGroups}");
                Console.WriteLine($"NumberOfLoadedSynthDefinitions = {statusReply.NumberOfLoadedSynthDefinitions}");
                Console.WriteLine($"AveragePercentCPUUsage = {statusReply.AveragePercentCPUUsage }");
                Console.WriteLine($"PeakPercentCPUUsage = {statusReply.PeakPercentCPUUsage}");
                Console.WriteLine($"NominalSampleRate = {statusReply.NominalSampleRate}");
                Console.WriteLine($"ActualSampleRate = {statusReply.ActualSampleRate}");

                Console.WriteLine($"Quitting...");

                var done = await client.Quit();

                Console.WriteLine($"Done");
            }
        }
    }
}
