using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using MainLib;

namespace ConsoleClient
{
    class Seeker
    {
        private Timer tim;
        private StorageFileInfo[] files = null;

        public static string[] excludeFileExtensions =
            new string[] { "*.tmp", "*.lnk", "*.pst" };

        private string 
            dirLocal = @"C:\Users\bogdan.visoiu\Desktop\doc",
            dirServer = "net.tcp://10.10.10.15:5000";
        public Seeker()
        {

        }

        internal void Execute()
        {
            //ListFiles();
            StartSync();
        }

        private void StartSync()
        {
            //tim = new Timer();
            //tim.Interval = 1000;
            //tim.Elapsed += Tim_Elapsed;
            //tim.Start();
            
            SyncExec();

            //using (FileRepositoryServiceClient client = new FileRepositoryServiceClient("net.tcp://10.10.10.15:5000"))
            //{
            //    files = client.List(null);
            //}

            //foreach (StorageFileInfo fi in files)
            //{
            //    Console.WriteLine(fi.VirtualPath);
            //}

            Console.ReadKey();
        }

        private void SyncExec()
        {
            List<string> filesClient = 
            Utils.GetDirectoryFileList(dirLocal, "___", excludeFileExtensions);

            foreach(string s in filesClient)
            {
                FileInfo f = new FileInfo(s);
                //Console.WriteLine(f.FullName);

                if (!string.IsNullOrEmpty(f.FullName))
                {
                    string virtualPath = Path.GetFileName(f.FullName);

                    using (Stream uploadStream = new FileStream(f.FullName, FileMode.Open))
                    {
                        using (FileRepositoryServiceClient client = new FileRepositoryServiceClient(dirServer))
                        {
                            client.PutFile(new FileUploadMessage() { VirtualPath = virtualPath, DataStream = uploadStream });
                        }
                    }

                    //RefreshFileList();
                }


            }
        }

        private void Tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }

        private void ListFiles()
        {
            

            using (FileRepositoryServiceClient client = new FileRepositoryServiceClient(dirServer))
            {
                files = client.List(null);
            }

            foreach (StorageFileInfo fi in files)
            {
                Console.WriteLine(fi.VirtualPath);
            }

            Console.ReadKey();
        }


    }
}
