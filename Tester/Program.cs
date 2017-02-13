using ServiceServe;
using ServiceClient;
using LibClient;

namespace Tester
{
    class Program
    {

        //static ServiceServer s0;
        //static ServiceCli s1;
        static void Main(string[] args)
        {
            /*
            s0 = new ServiceServer();
            s1 = new ServiceCli();


            s0.Start(@"c:\_sync1\");
            s1.Start(@"c:\___\");
            */

            Provider c = new Provider();
            c.Start(@"c:\___\");

        }
    }
}
