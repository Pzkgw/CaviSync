
using System.Net;

namespace MainLib
{
    public static class Optiuni
    {
        public const string EndpointType = "net.tcp";
        public static IPAddress EndpointIP = IPAddress.Parse("10.10.10.15");
        public static int EndpointPort = 5000;


        public static string dirClient = @"C:\Users\bogdan.visoiu\Desktop\doc";


        public static string GetEndpointAddress()
        {
            return string.Format("{0}://{1}:{2}", EndpointType, EndpointIP, EndpointPort);
        }
    }
}
