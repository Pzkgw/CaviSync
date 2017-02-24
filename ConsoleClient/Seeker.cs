using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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


        private void StartSyncOnce()
        {
            SyncExec();

            Console.ReadKey();
        }

        private void StartSyncTimed()
        {
            tim = new Timer();
            tim.Interval = 1000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();

            Console.ReadKey();
        }


        private void SyncExec()
        {
            IPAddress localIP = Utils.GetLocalIpAddress();
            string localIP_string = (localIP == null) ? null : localIP.ToString();
            // init conexiune cu serverul
            using (FileRepositoryServiceClient client = new FileRepositoryServiceClient())
            {
                // Trebuie setat exact dupa constructorul FileRepositoryServiceClient
                client.SetEndpointAddress(Optiuni.GetEndpointAddress());
                client.SendConnectionInfo(localIP_string, Optiuni.EndpointPort, Optiuni.dirClient);


                // foreach file in client directory ---> send it
                foreach (string s in Utils.GetDirectoryFileList(Optiuni.dirClient, "___", excludeFileExtensions))
                {
                    FileInfo fi = new FileInfo(s);
                    long fileSize = 0;

                    if (!Utils.IsFileLocked(fi))
                    {
                        fileSize = fi.Length;

                        FileUploadMessage fum = new FileUploadMessage()
                        {
                            VirtualPath = s.Substring(Optiuni.dirClient.Length + 1, s.Length - Optiuni.dirClient.Length - 1),
                            LastWriteTimeUtcTicks = fi.LastWriteTimeUtc.Ticks
                        };

                        fi = null;

                        using (Stream uploadStream = new FileStream(s, FileMode.Open))
                        {

                            fum.DataStream = uploadStream;

                            if (client.GetPreUploadCheckResult(fum.VirtualPath, fum.LastWriteTimeUtcTicks, fileSize))
                            {
                                client.PutFile(fum);
                            }

                        }
                    }

                }

            }

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
                client.SetEndpointAddress(Optiuni.GetEndpointAddress());
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
