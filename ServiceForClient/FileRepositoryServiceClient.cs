using System;
using MainLib;
using System.ServiceModel;


namespace ServiceForClient
{
    public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
    {
        public FileRepositoryServiceClient() : base("FileRepositoryService")
        {

        }
        public void SetEndpointAddress(string s)
        {
            Endpoint.Address = new EndpointAddress(s);
        }

        #region IFileRepositoryService Members

        public System.IO.Stream GetFile(string virtualPath)
        {
            return base.Channel.GetFile(virtualPath);
        }

        public bool GetPreUploadCheckResult(string clientIP, string directory, string file, long lastWriteTimeUtcTicks, long fileSize)
        {
            try
            {
                return base.Channel.GetPreUploadCheckResult(clientIP, directory, file, lastWriteTimeUtcTicks, fileSize);
            }
            catch (Exception) // alte exceptii, nu doar EndpointNotFoundException ---> simple return false
            {
            }
            return false;
        }

        public void PutFile(FileUploadMessage msg)
        {
            // pre-PutFile : SendConnectionInfo, GetPreUploadCheckResult
            try
            {
                base.Channel.PutFile(msg);
            }
            //Serverul nu e pornit:
            //TCP error code 10061: No connection could be made because the target machine actively refused it
            catch (EndpointNotFoundException)
            {
            }
        }

        public void DeleteFile(string virtualPath)
        {
            base.Channel.DeleteFile(virtualPath);
        }

        public StorageFileInfo[] List(string virtualPath)
        {
            return base.Channel.List(virtualPath);
        }

        public void SendConnectionInfo(string ip, int port, string path)
        {
            try
            {
                base.Channel.SendConnectionInfo(ip, port, path);
            }
            catch (Exception)//EndpointNotFoundException)
            {
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (this.State == CommunicationState.Opened)
                this.Close();
        }

        #endregion
    }
}
