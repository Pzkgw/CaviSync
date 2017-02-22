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

        public Seeker()
        {

        }

        internal void Execute()
        {
            Initialize();
            //ListFiles();
            //StartSyncOnce();
            StartSyncTimed();
        }

        private void Initialize()
        {
            
        }

        private void StartSyncTimed()
        {
            tim = new Timer();
            tim.Interval = 1000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();

            Console.ReadKey();
        }

        private void StartSyncOnce()
        {
            SyncExec();

            Console.ReadKey();
        }

        private void SyncExec()
        {
            List<string> filesClient =
            Utils.GetDirectoryFileList(Optiuni.dirClient, "___", excludeFileExtensions);

            foreach (string s in filesClient)
            {
                FileInfo f = new FileInfo(s);
                //Console.WriteLine(f.FullName);

                bool isl = Utils.IsFileLocked(f);
                //Console.WriteLine(isl.ToString());

                if (!string.IsNullOrEmpty(f.FullName) && !isl)
                {
                    string virtualPath = f.FullName.Substring(Optiuni.dirClient.Length + 1); // Path.GetFileName(f.FullName);

                    using (Stream uploadStream = new FileStream(f.FullName, FileMode.Open))
                    {
                        using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
                        {
                            client.SetEndpointAddress();
                            client.PutFile(new FileUploadMessage() { VirtualPath = virtualPath, DataStream = uploadStream });
                        }
                    }

                }

            }

            //RefreshFileList();

        }

        private void Tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            tim.Enabled = false;
            SyncExec();
            tim.Enabled = true;
        }

        private void ListFiles()
        {

            using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
            {
                client.SetEndpointAddress();
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
