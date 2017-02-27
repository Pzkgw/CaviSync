
using System.Net;
using System.Text;

namespace MainLib
{
    public static class Optiuni
    {
        public const int EndpointPort = 5000;
        public const string EndpointType = "net.tcp";
        public static IPAddress EndpointIP = IPAddress.Parse("10.10.10.15");

        public static string
            dirClient =
            @"c:\___\",
            //@"C:\Users\bogdan.visoiu\Desktop\doc",
            dirServer = "Depozit";

        public static string GetDirClient()
        {
            return dirClient.EndsWith("\\") ? dirClient.Substring(0, dirClient.Length - 1) : dirClient;
        }

        public static string GetDirServer()
        {
            return dirServer;
        }

        public static string GetEndpointAddress()
        {
            return string.Format("{0}://{1}:{2}", EndpointType, EndpointIP, EndpointPort);
        }

        public static string MakeNonComprehensiveDirectoryStringForServer(string ip, string clientDir)
        {
            return string.Format("{0}{1}", ip,
                clientDir.Replace('\\','@').Replace(':','$'));
        }
    }
}
