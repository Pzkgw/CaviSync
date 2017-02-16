using ServiceServe;
using ServiceClient;
using System;
using Microsoft.Synchronization.Files;

namespace Tester
{
    class Program
    {

        static ServiceCli source;
        static ServiceServer dest;
        //static int tms;
        static DateTime dts, dtc;
        static void Main(string[] args)
        {

            dest = new ServiceServer();
            dest.Start(@"c:\_sync1\");
            dest.pro.sync.DetectingChanges += Sync_DetectingChangesServer;
            dest.pro.sync.DetectedChanges += Sync_DetectedChangesServer;

            source = new ServiceCli();
            source.Start(@"c:\___\");
            source.pro.sync.DetectingChanges += Sync_DetectingChangesClient;
            source.pro.sync.DetectedChanges += Sync_DetectedChangesClient;
            source.pro.sync.AppliedChange += Sync_AppliedChange;


            Console.ReadKey();
        }

        private static void Sync_DetectedChanges1(object sender, DetectedChangesEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Sync_DetectingChanges1(object sender, DetectingChangesEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Sync_AppliedChange(object sender, AppliedChangeEventArgs args)
        {
            switch (args.ChangeType)
            {
                case ChangeType.Create:
                    Console.WriteLine("-- Applied CREATE for file " + args.NewFilePath);
                    break;
                case ChangeType.Delete:
                    Console.WriteLine("-- Applied DELETE for file " + args.OldFilePath);
                    break;
                case ChangeType.Update:
                    Console.WriteLine("-- Applied OVERWRITE for file " + args.OldFilePath);
                    break;
                case ChangeType.Rename:
                    Console.WriteLine("-- Applied RENAME for file " + args.OldFilePath +
                                      " as " + args.NewFilePath);
                    break;
            }
        }

        private static void Sync_DetectingChangesClient(object sender, DetectingChangesEventArgs e)
        {
            dtc = DateTime.Now;
            //Console.WriteLine(string.Format("DetectingChanges start"));
        }

        private static void Sync_DetectedChangesClient(object sender, DetectedChangesEventArgs e)
        {
            Console.WriteLine(string.Format(" Client time for detection(ms): {0,4}  {1}",
                DateTime.Now.Subtract(dtc).Milliseconds, source.pro.id));
        }

        private static void Sync_DetectingChangesServer(object sender, DetectingChangesEventArgs e)
        {
            dts = DateTime.Now;
            //Console.WriteLine(string.Format("DetectingChanges start"));
        }

        private static void Sync_DetectedChangesServer(object sender, DetectedChangesEventArgs e)
        {
            Console.WriteLine(string.Format(" Server time for detection(ms): {0,4}  {1}",
                DateTime.Now.Subtract(dts).Milliseconds, dest.pro.id));
        }


    }
}
