using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace MainLib
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true,
        InstanceContextMode = InstanceContextMode.Single)]
    public class FileRepositoryService : IFileRepositoryService
    {

        #region Events

        public event FileEventHandler FileRequested;
        public event FileEventHandler FileUploaded;
        public event FileEventHandler FileDeleted;
        public event InfoSendEventHandler InfoSend;

        #endregion

        #region IFileRepositoryService Members

        /// <summary>
        /// Gets or sets the repository directory.
        /// </summary>
        public string RepositoryDirectory { get; set; }

        /// <summary>
        /// Gets or sets the repository directory.
        /// </summary>
        public string RepositoryHost { get; set; }

        /// <summary>
        /// Gets a file from the repository
        /// </summary>
        public Stream GetFile(string virtualPath)
        {
            string filePath = Path.Combine(RepositoryDirectory, virtualPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File was not found", Path.GetFileName(filePath));

            SendFileRequested(virtualPath);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public bool GetPreUploadCheckResult(string path, long lastWriteTimeUtcTicks, long fileSize)
        {
            string filePath = Path.Combine(RepositoryDirectory, path);
            if (File.Exists(filePath))
            {
                // if (file source vs file dest) utc write time sau un size diferit --> update
                FileInfo fi = new FileInfo(filePath);

                if (fi != null && fi.LastWriteTimeUtc.Ticks == lastWriteTimeUtcTicks && fi.Length == fileSize)
                    return false;

            }
            return true;
        }

        /// <summary>
        /// Uploads a file into the repository
        /// </summary>
        public void PutFile(FileUploadMessage msg)
        {
            var sw = Stopwatch.StartNew();
            string filePath = Path.Combine(RepositoryDirectory, msg.VirtualPath);
            string dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                msg.DataStream.CopyTo(outputStream);
            }

            sw.Stop();

            SendFileUploaded(filePath, msg.LastWriteTimeUtcTicks, sw.Elapsed.TotalMilliseconds);
        }

        /// <summary>
        /// Deletes a file from the repository
        /// </summary>
        public void DeleteFile(string virtualPath)
        {
            string filePath = Path.Combine(RepositoryDirectory, virtualPath);

            if (File.Exists(filePath))
            {
                SendFileDeleted(virtualPath);
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Lists files from the repository at the specified virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path. This can be null to list files from the root of
        /// the repository.</param>
        public StorageFileInfo[] List(string virtualPath)
        {
            string basePath = RepositoryDirectory;

            if (!string.IsNullOrEmpty(virtualPath))
                basePath = Path.Combine(RepositoryDirectory, virtualPath);

            DirectoryInfo dirInfo = new DirectoryInfo(basePath);
            FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

            return (from f in files
                    select new StorageFileInfo()
                    {
                        Size = f.Length,
                        VirtualPath = f.FullName.Substring(f.FullName.IndexOf(RepositoryDirectory) + RepositoryDirectory.Length + 1),
                        LastWriteTimeUtc = f.LastWriteTimeUtc
                    }).ToArray();
        }

         
        public void SendConnectionInfo(string ip, int port, string path)
        {
            //IPAddress ipAddress = Utils.GetLocalIpAddress();
            //ip = (ipAddress != null) ? ipAddress.ToString() : null;
            //port = Optiuni.EndpointPort;
            SendConnectionInfoEventUpdate(ip, port, path);
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the FileRequested event
        /// </summary>
        protected void SendFileRequested(string vPath)
        {
            if (FileRequested != null)
                FileRequested(this, new FileEventArgs(vPath, 0, 0));
        }

        /// <summary>
        /// Raises the FileUploaded event
        /// </summary>
        protected void SendFileUploaded(string vPath, long LastWriteTimeUtcTicks, double time)
        {
            if (FileUploaded != null)
                FileUploaded(this, new FileEventArgs(vPath, LastWriteTimeUtcTicks, time));
        }

        /// <summary>
        /// Raises the FileDeleted event
        /// </summary>
        protected void SendFileDeleted(string vPath)
        {
            if (FileDeleted != null)
                FileDeleted(this, new FileEventArgs(vPath, 0, 0));
        }


        protected void SendConnectionInfoEventUpdate(string ip, int port, string path)
        {
            if (InfoSend != null)
                InfoSend(this, new InfoEventArgs(ip, port, path));
        }


        #endregion
    }
}
