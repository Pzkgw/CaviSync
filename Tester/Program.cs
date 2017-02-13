using ServiceServe;
using ServiceClient;
using System;

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


            Console.ReadKey();
        }

        private static void Sync_DetectingChanges(object sender, Microsoft.Synchronization.Files.DetectingChangesEventArgs e)
        {
            dt = DateTime.Now;
            Console.WriteLine(string.Format("DetectingChanges start"));
        }

        private static void Sync_DetectedChanges(object sender, Microsoft.Synchronization.Files.DetectedChangesEventArgs e)
        {
            Console.WriteLine(string.Format("DetectingChanges end in {0}ms", DateTime.Now.Subtract(dt).Milliseconds));
        }


    }
}
