using System;
using System.IO;
using System.Net;
using System.Timers;
using MainLib;

namespace ConsoleClient
{
    class Seeker
    {
        private Timer tim;

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

        static int tc = 0;
        private void SyncExec()
        {
            Optiuni.EndpointIP = "10.10.10.15";
            Optiuni.dirClient = @"c:\__###\SDL2\";

            try
            {
                IPAddress localIP = Utils.GetLocalIpAddress();
                string localIP_string = (localIP == null) ? null : localIP.ToString();

                string dirClient = Optiuni.GetDirClient();

                // init conexiune cu serverul
                using (FileRepositoryServiceClient client = new FileRepositoryServiceClient(Optiuni.GetEndpointAddress()))
                {
                    client.SendConnectionInfo(localIP_string, Optiuni.EndpointPort, dirClient);

                    // foreach file in client directory ---> send it
                    foreach (string s in Utils.GetDirectoryFileList(dirClient, "___", excludeFileExtensions))
                    {

                        FileInfo fi = new FileInfo(s);
                        long fileSize = 0;

                            fileSize = fi.Length;

                            FileUploadMessage fum = new FileUploadMessage()
                            {
                                VirtualPath = s.Substring(dirClient.Length + 1, s.Length - dirClient.Length - 1),
                                LastWriteTimeUtcTicks = fi.LastWriteTimeUtc.Ticks
                            };

                            fi = null;

                            if (client.GetPreUploadCheckResult(
                                localIP_string,
                                dirClient,
                                fum.VirtualPath,
                                fum.LastWriteTimeUtcTicks
                                , fileSize))
                            {
                                using (Stream uploadStream = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.None))
                                {
                                    fum.DataStream = uploadStream;
                                    client.PutFile(fum);
                                    ++tc;
                                    Console.WriteLine(string.Format("{0} fisiere trimise spre server ", tc.ToString()));
                                }
                            }
                            else
                            {
                                // ::telnet server
                                //Console.WriteLine(
                                //    string.Format("ZERO:{0} {1} {2} {3}",
                                //    localIP_string,
                                //    dirClient,
                                //    fum.VirtualPath,
                                //    fum.LastWriteTimeUtcTicks
                                //    ));
                            }


                        

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

            StorageFileInfo[] files = null;

            using (FileRepositoryServiceClient client = new FileRepositoryServiceClient(Optiuni.GetEndpointAddress()))
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
