using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;

namespace Library
{
    class CustomProvider : IFileDataRetriever, IDisposable
    {

        public CustomProvider()//(string rootDirectoryPath)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        string IFileDataRetriever.RelativeDirectoryPath
        {
            get
            {
                return string.Empty;
            }
        }

        string IFileDataRetriever.AbsoluteSourceFilePath
        {
            get
            {
                return string.Empty;
            }
        }

        FileData IFileDataRetriever.FileData
        {
            get
            {
                return null;
            }
        }

        Stream IFileDataRetriever.FileStream
        {
            get
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CustomProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
