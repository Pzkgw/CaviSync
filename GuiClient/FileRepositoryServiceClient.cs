﻿using System;
using MainLib;
using System.ServiceModel;

namespace FileServerWinClient
{
    public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
    {
        public FileRepositoryServiceClient() : base("FileRepositoryService")
        {
            Endpoint.Address = new EndpointAddress("net.tcp://10.10.10.15:5000");
        }

        #region IFileRepositoryService Members

        public System.IO.Stream GetFile(string virtualPath)
        {
            return base.Channel.GetFile(virtualPath);
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

        public StorageFileInfo[] List()
        {
            return List(null);
        }

        public StorageFileInfo[] List(string virtualPath)
        {
            return base.Channel.List(virtualPath);
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
