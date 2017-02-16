using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace SyncPtP
{
    class Program
    {
        static void Main(string[] args)
        {

            //PeerIt();

            CFileInfo[] hh = CFileInfoGeneral.GetFiles(@"c:\_sync\", "___", Settings.excludeFileExtensions);

            foreach(CFileInfo fi in hh)
            {
                Console.WriteLine(string.Format("{0}|Sz:{1}", fi.FullName, fi.Length));
            }

            Console.WriteLine(hh.Length);

            Console.ReadKey();
        }

        private static void PeerIt()
        {

            // Creates a secure (not spoofable) PeerName

            PeerName peerName = new PeerName("Server", PeerNameType.Secured);

            PeerNameRegistration pnReg = new PeerNameRegistration();

            pnReg.PeerName = peerName;

            pnReg.Port = 80;



            //OPTIONAL

            //The properties set below are optional.  You can register a PeerName without setting these properties

            pnReg.Comment = "up to 39 unicode char comment";

            pnReg.Data = System.Text.Encoding.UTF8.GetBytes("A data blob associated with the name");



            /*

             * OPTIONAL

             *The properties below are also optional, but will not be set (ie. are commented out) for this example

             *pnReg.IPEndPointCollection = // a list of all {IPv4/v6 address, port} pairs to associate with the peername

             *pnReg.Cloud = //the scope in which the name should be registered (local subnet, internet, etc)

            */



            //Starting the registration means the name is published for others to resolve

            pnReg.Start();

            Console.WriteLine("Registration of Peer Name: { 0} complete.", peerName.ToString());

            Console.WriteLine();



            Console.WriteLine("Press any key to stop the registration and close the program");

            



            pnReg.Stop();
        }
    }
}
