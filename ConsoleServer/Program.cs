using System;
using System.ServiceModel;
using System.IO;

namespace MainLib
{
    class Program
    {
        static ServiceHost host = null;
        static FileRepositoryService service = null;

        static NetTcpBinding netTcpBinding = null;

        static string str = "net.tcp://localhost:5000";

        //static Uri tcpUri = null;
        static void Main(string[] args)
        {

            service = new FileRepositoryService();
            host = new ServiceHost(service);

            //tcpUri = new Uri(str);

            //host = new ServiceHost(typeof(FileRepositoryService), tcpUri); // ,    

            //service sau ((FileRepositoryService)host.SingletonInstance)
            service.RepositoryDirectory = Optiuni.GetDirServer();

            service.InfoSend += new InfoSendEventHandler(Service_InfoSend);

            service.FileRequested += new FileEventHandler(Service_FileRequested);
            service.FileUploaded += new FileEventHandler(Service_FileUploaded);
            service.FileDeleted += new FileEventHandler(Service_FileDeleted);

            host.Faulted += new EventHandler(Host_Faulted);

            netTcpBinding = BindServer.Get();

            //ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            //host.Description.Behaviors.Add(mBehave);
            //host.AddServiceEndpoint(typeof(IMetadataExchange),
            //  MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

            host.AddServiceEndpoint(typeof(IFileRepositoryService), netTcpBinding, str);

            try
            {
                host.Open();
                Console.WriteLine("Press a key to close the service");
                Console.ReadKey();
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine(timeProblem.Message);
                Console.ReadLine();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine(commProblem.Message);
                Console.ReadLine();
            }
            finally
            {
                if (host.State == CommunicationState.Faulted)
                {
                    host.Abort();
                }
                else
                {
                    host.Close();
                }
            }
        }


        static void Service_InfoSend(object sender, InfoEventArgs e)
        {
            //Console.WriteLine(string.Format(" client {0}:{1} {2}", e.IP, e.Port, e.Path));

            ((FileRepositoryService)host.SingletonInstance).RepositoryHost = Optiuni.MakeNonComprehensiveDirectoryStringForServer(e.IP, e.Path);
        }

        static void Host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Host faulted; reinitialising host");
            host.Abort();
        }

        static void Service_FileRequested(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("Access  {0}  {1}", e.VirtualPath, DateTime.Now));
        }

        static void Service_FileUploaded(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("Upload  {0}  {1}", e.VirtualPath, new DateTime(e.LastWriteTimeUtcTicks, DateTimeKind.Utc))); // , e.ExecTime

            // update destination file modification date with value from the source file 
            string path = e.VirtualPath;
            FileInfo fi = null;

            try
            {
                if (File.Exists(path) && e.LastWriteTimeUtcTicks > 0)
                {
                    fi = new FileInfo(path);
                    DateTime dt = new DateTime(e.LastWriteTimeUtcTicks, DateTimeKind.Utc);
                    fi.LastWriteTime = dt;
                    fi.LastWriteTimeUtc = dt;

                    //Console.WriteLine("GATA"+fi.LastWriteTime.ToString());
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("--EXEC "+ex.ToString());
            }
        }

        static void Service_FileDeleted(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("Delete  {0}  {1}", e.VirtualPath, DateTime.Now));
        }

    }
}
