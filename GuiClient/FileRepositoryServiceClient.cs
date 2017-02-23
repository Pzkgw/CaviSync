using System;
using MainLib;
using System.ServiceModel;

namespace FileServerWinClient
{
    public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
    {
        public FileRepositoryServiceClient() : base("FileRepositoryService")
        {
            Endpoint.Address = new EndpointAddress(Optiuni.GetEndpointAddress());
        }

        #region IFileRepositoryService Members

        public System.IO.Stream GetFile(string virtualPath)
        {
            return base.Channel.GetFile(virtualPath);
        }

        public bool GetPreUploadCheckResult(string msg)
        {
            return base.Channel.GetPreUploadCheckResult(msg);
        }

        public void PutFile(FileUploadMessage msg)
        {
            // Cazuri de eroare la conexiune:

            // 1. Serverul nu e pornit:
            //TCP error code 10061: No connection could be made because the target machine actively refused it

            base.Channel.PutFile(msg);

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
