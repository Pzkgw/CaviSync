using System.ServiceModel;
using System.IO;

namespace MainLib
{
	
	[ServiceContract]
	public interface IFileRepositoryService
	{
		[OperationContract]
		Stream GetFile(string virtualPath);

        [OperationContract] // check daca fisierul sursa s-a modificat si e necesar un update
        bool GetPreUploadCheckResult(string clientIP, string directory, string file, long lastWriteTimeUtcTicks, long fileSize);

        [OperationContract(IsOneWay = true)]
		void PutFile(FileUploadMessage msg);

		[OperationContract(IsOneWay = true)]
		void DeleteFile(string virtualPath);

		[OperationContract]
		StorageFileInfo[] List(string virtualPath);

        [OperationContract (IsOneWay = true)]
        void SendConnectionInfo(string ip, int port, string path);

    }
}
