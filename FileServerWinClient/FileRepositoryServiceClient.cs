using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileServer.Services;
using System.ServiceModel;
using System.IO;

namespace FileServerWinClient
{
	public class FileRepositoryServiceClient : ClientBase<IFileRepositoryService>, IFileRepositoryService, IDisposable
	{
		public FileRepositoryServiceClient()
			: base("FileRepositoryService")
		{
		}

		#region IFileRepositoryService Members

		public System.IO.Stream GetFile(string virtualPath)
		{
			return base.Channel.GetFile(virtualPath);
		}

		public void PutFile(FileUploadMessage msg)
		{
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
