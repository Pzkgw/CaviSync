using ServiceServe;
using ServiceClient;
using System;
using Microsoft.Synchronization.Files;

namespace Tester
{
    class Program
    {

        static ServiceCli c;
        //static int tms;
        static DateTime dt;
        static void Main(string[] args)
        {
            
            //s = new ServiceServer();
            //s.Start(@"c:\_sync1\");

            c = new ServiceCli();   
                     
            c.Start(@"c:\___\");
            c.pro.sync.DetectingChanges += Sync_DetectingChanges;
            c.pro.sync.DetectedChanges += Sync_DetectedChanges;
            c.pro.sync.AppliedChange += Sync_AppliedChange;


            Console.ReadKey();
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

        private static void Sync_DetectingChanges(object sender, Microsoft.Synchronization.Files.DetectingChangesEventArgs e)
        {
            dt = DateTime.Now;
            //Console.WriteLine(string.Format("DetectingChanges start"));
        }

        private static void Sync_DetectedChanges(object sender, Microsoft.Synchronization.Files.DetectedChangesEventArgs e)
        {
            Console.WriteLine(string.Format(" Changes detected time(ms): {0}", DateTime.Now.Subtract(dt).Milliseconds));
        }


    }
}
