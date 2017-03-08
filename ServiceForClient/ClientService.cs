using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using MainLib;

namespace ServiceForClient
{
    public partial class ClientService : ServiceBase
    {
        private Timer tim;

        public static string[] excludeFileExtensions =
            new string[] { "*.tmp", "*.lnk", "*.pst" };

        public ClientService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            tim = new Timer();
            tim.Interval = 1000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();
        }


        private void SyncExec(string dirClient, string localIP_string, FileRepositoryServiceClient client)
        {

            // foreach file in client directory ---> send it
            foreach (string s in Utils.GetDirectoryFileList(dirClient, "___", excludeFileExtensions))
            {
                FileInfo fi = new FileInfo(s);

                if (Utils.IsFileLocked(fi))
                {
                    fi = null;
                    continue;
                }

                long fileSize = fi.Length, lastWriteTimeUtc = fi.LastWriteTimeUtc.Ticks;

                fi = null;

                FileUploadMessage fum = new FileUploadMessage()
                {
                    VirtualPath = s.Substring(dirClient.Length + 1, s.Length - dirClient.Length - 1),
                    LastWriteTimeUtcTicks = lastWriteTimeUtc
                };

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
                        //++tc;
                        //Console.WriteLine(string.Format("{0} fisiere trimise spre server ", tc.ToString()));
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

        private void Tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            tim.Enabled = false;

            FileRepositoryServiceClient client = null;

            try
            {
                IPAddress localIP = Utils.GetLocalIpAddress();
                string dirClient = null, localIP_string = (localIP == null) ? null : localIP.ToString();

                Optiuni.EndpointIP = RegEdit.ClientGetServerIP();

                // init conexiune cu serverul
                client = new FileRepositoryServiceClient(Optiuni.GetEndpointAddress());               

                for (int i = 1; i < 4; i++)
                {
                    dirClient = RegEdit.ClientGetPath(i);

                    if (dirClient != null && dirClient.Length > 2)
                    {
                        Optiuni.dirClient = dirClient;

                        dirClient = Optiuni.GetDirClient();

                        client.SendConnectionInfo(localIP_string, Optiuni.EndpointPort, dirClient);

                        SyncExec(dirClient, localIP_string, client);
                    }
                }
            }
            catch (Exception)
            {
                //Utils.Log(ex.ToString());
            }
            finally
            {
                if (client != null)
                {
                    if (client.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        client.Abort();
                    }
                    else
                    {
                        client.Close();
                    }
                }
            }

            tim.Enabled = true;
        }

        protected override void OnStop()
        {
        }
    }
}
