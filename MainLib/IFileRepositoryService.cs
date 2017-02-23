using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace MainLib
{
	
	[ServiceContract]
	public interface IFileRepositoryService
	{
		[OperationContract]
		Stream GetFile(string virtualPath);

        [OperationContract]
        bool GetPreUploadCheckResult(string path);

        [OperationContract]
		void PutFile(FileUploadMessage msg);

		[OperationContract]
		void DeleteFile(string virtualPath);

		[OperationContract]
		StorageFileInfo[] List(string virtualPath);

        [OperationContract]
        void GetConnectionInfo(ref string ip, ref int port);

        [OperationContract]
        string GetSyncDirectory();

    }
}
