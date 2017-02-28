using System;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using MainLib;

namespace ServiceForServer
{
    public partial class ServerService : ServiceBase
    {

        ServiceHost host = null;
        FileRepositoryService service = null;

        Timer tim;

        public ServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {


            if (host != null)
            {
                host.Close();
                host = null;
            }

            if (host == null) StartIt();

            base.OnStart(args);

            tim = new Timer();
            tim.Interval = 2000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();

        }

        private void StartIt()
        {
            service = new FileRepositoryService();
            service.RepositoryDirectory = Optiuni.GetDirServer();

            service.InfoSend += new InfoSendEventHandler(Service_InfoSend);
            service.FileUploaded += new FileEventHandler(Service_FileUploaded);

            host = new ServiceHost(service); // typeof(FileRepositoryService),
            host.Faulted += new EventHandler(Host_Faulted);

            try
            {
                host.Open();
            }
            catch (Exception)
            {
                //Log(string.Format("EXCEPTION: {0} : {1}",
                //DateTime.Now.ToString(), ex.Message.ToString().Trim()));

                //if (host.State == CommunicationState.Faulted)
                //{
                //    host.Abort();
                //}
                //else
                //{
                host.Close();
                host = null;
                //}
            }
            //(TimeoutException timeProblem)(CommunicationException commProblem)             
            finally
            {


            }
        }

        private void Tim_Elapsed(object sender, ElapsedEventArgs e)
        {
            tim.Enabled = false;


            tim.Enabled = true;
        }


        protected override void OnStop()
        {
            base.OnStop();

            if (host != null)
            {
                host.Close();
                host = null;
            }
        }


        void Service_InfoSend(object sender, InfoEventArgs e)
        {
            //Console.WriteLine(string.Format(" client {0}:{1} {2}", e.IP, e.Port, e.Path));

            service.RepositoryHost = Optiuni.MakeNonComprehensiveDirectoryStringForServer(e.IP, e.Path);
        }

        void Host_Faulted(object sender, EventArgs e)
        {
            host.Abort();
        }

        void Service_FileUploaded(object sender, FileEventArgs e)
        {

            // update destination file modification date with value from the source file 
            string path = e.VirtualPath;

            FileInfo fi = null;

            try
            {
                if (File.Exists(path) && e.LastWriteTimeUtcTicks > 0)
                {
                    fi = new FileInfo(path);
                    if (!Utils.IsFileLocked(fi))
                    {
                        DateTime dt = new DateTime(e.LastWriteTimeUtcTicks, DateTimeKind.Utc);

                        fi.LastWriteTime = dt;
                        fi.LastWriteTimeUtc = dt;
                        fi = null;
                    }
                    
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                fi = null;
            }
        }






        private static int Log(string s)
        {
            //(new Thread(() =>            {



            try
            {
                using (StreamWriter sw = new StreamWriter("c:\\Log.txt", true))
                {
                    sw.WriteLine(string.Format("{0} : {1}", DateTime.Now.ToString(), s));
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }

            //})).Start();
            return 0;
        }


    }
}
