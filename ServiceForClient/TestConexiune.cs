
using System;
using System.Net;
using System.ServiceModel;
using MainLib;

namespace ServiceForClient
{
    public static class TestConexiune
    {
        public static string DoEeet()
        {
            FileRepositoryServiceClient client = null;
            string retVal = null;

            try
            {
                Optiuni.EndpointIP = RegEdit.ClientGetServerIP();
                client = new FileRepositoryServiceClient(Optiuni.GetEndpointAddress());
                {
                    if(!client.GetPreUploadCheckResult(
                        Optiuni.preTestClientIP, Optiuni.preTestDirectoryName, Optiuni.preTestFileName,
                        512, 1024))
                    {
                        retVal = "Testing communication failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                retVal = ex.ToString();
            }
            finally
            {
                if (client != null)
                {
                    if (client.State == CommunicationState.Faulted)
                    {
                        client.Abort();
                    }
                    else
                    {
                        client.Close();
                    }
                }
            }

            return retVal;

        }

    }
}
