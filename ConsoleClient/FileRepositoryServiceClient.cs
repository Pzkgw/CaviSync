﻿using System;
using System.ServiceModel;
using MainLib;

namespace ConsoleClient
{
    public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
    {
        //public FileRepositoryServiceClient() : base("FileRepositoryService")
        //{
        //    //
        //}

        public FileRepositoryServiceClient(string s) : base(BindClient.Get(s))
        {
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
            catch (Exception ex) // alte exceptii, nu doar EndpointNotFoundException ---> simple return false
            {
                Console.WriteLine(ex.ToString());
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
            catch (Exception ex) // alte exceptii, nu doar EndpointNotFoundException ---> simple return false
            {
                Console.WriteLine(ex.ToString());
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
