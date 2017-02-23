using System;
using System.ServiceModel;
using MainLib;

namespace ConsoleClient
{
    public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
    {
        public FileRepositoryServiceClient() : base("FileRepositoryService")
        {            
           
        }

        public void SetEndpointAddress()
        {
            Endpoint.Address = new EndpointAddress(Optiuni.GetEndpointAddress());
        }

        #region IFileRepositoryService Members

        public System.IO.Stream GetFile(string virtualPath)
        {
            return base.Channel.GetFile(virtualPath);
        }

        public bool GetPreUploadCheckResult(string path, long lastWriteTimeUtcTicks, long fileSize)
        {
            return base.Channel.GetPreUploadCheckResult(path, lastWriteTimeUtcTicks, fileSize);
        }

        public void PutFile(FileUploadMessage msg)
        {
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

        //public StorageFileInfo[] List()
        //{
        //    return List(null);
        //}

        public StorageFileInfo[] List(string virtualPath)
        {
            return base.Channel.List(virtualPath);
        }

        public void GetConnectionInfo(ref string ip, ref int port)
        {
            base.Channel.GetConnectionInfo(ref ip, ref port);
        }

        public string GetSyncDirectory()
        {
            return null;
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
