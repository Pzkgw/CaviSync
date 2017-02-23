using System;
using System.ServiceModel;
using System.IO;

namespace MainLib
{
    class Program
    {
        static ServiceHost host = null;
        static FileRepositoryService service = null;

        static void Main(string[] args)
        {

            service = new FileRepositoryService();
            service.RepositoryDirectory = "Depozit";


            service.FileRequested += new FileEventHandler(Service_FileRequested);
            service.FileUploaded += new FileEventHandler(Service_FileUploaded);
            service.FileDeleted += new FileEventHandler(Service_FileDeleted);

            host = new ServiceHost(service);
            host.Faulted += new EventHandler(Host_Faulted);

            try
            {
                host.Open();
                Console.WriteLine("Press a key to close the service");
                Console.ReadKey();
            }
            finally
            {
                host.Close();
            }
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
            Console.WriteLine(string.Format("{2} Upload  {0}  {1}", e.VirtualPath, new DateTime(e.LastWriteTimeUtcTicks, DateTimeKind.Utc), e.ExecTime));

            // update destination file modification date with value from the source file 
            string path = e.VirtualPath;
            FileInfo fi = null;

            if (File.Exists(path) && e.LastWriteTimeUtcTicks>0)
            {
                fi = new FileInfo(path);
                DateTime dt = new DateTime(e.LastWriteTimeUtcTicks, DateTimeKind.Utc);
                if (dt != DateTime.MaxValue)
                {
                    fi.LastWriteTime = dt;
                    fi.LastWriteTimeUtc = dt;
                }
            }
        }

        static void Service_FileDeleted(object sender, FileEventArgs e)
        {
            Console.WriteLine(string.Format("Delete  {0}  {1}", e.VirtualPath, DateTime.Now));
        }

    }
}
