using System;
using System.IO;
using Microsoft.Synchronization.Files;

namespace Library
{
    public class BaseProvider : IDisposable
    {
        public FileSyncProvider sync = null;
        public FileSyncOptions optFileSync;
        public FileSyncScopeFilter optFilter;

        public Guid id;

        public Exception ex;

        public BaseProvider()
        {
        }

        public void Init()
        {
            optFileSync = FileSyncOptions.ExplicitDetectChanges;

            FileAttributes excludeFileAttributes =
            FileAttributes.System | FileAttributes.Hidden;

            optFilter = new FileSyncScopeFilter();

            optFilter.AttributeExcludeMask = excludeFileAttributes;
            foreach (string s in Settings.excludeFileExtensions)
                optFilter.FileNameExcludes.Add(s);

            id = Guid.NewGuid();
        }

        public void Start(string path)
        {
            ex = null;
            try
            {
                sync = new FileSyncProvider(id, path, optFilter, optFileSync);
                //,set.metadataDirectoryPath, Settings.metaFileLoc,
                // set.tempDirectoryPath, set.pathToSaveConflictLoserFiles
            }
            catch (Exception exc)
            {
                ex = exc;
            }
            finally
            {
                if (sync != null && ex != null) sync.Dispose();
            }
        }

        public void DetectChanges()
        {
            if (sync == null) return;

            try
            {
                // if((detect time) > (time interval(500 ms))) --> uncatched error
                sync.DetectChanges();
            }
            catch
            // Sa nu interactioneze 2 detectii, altfel :
            //Attempted to read or write protected memory. 
            { }
        }

        public void Dispose()
        {
            if (sync != null)
            {
                sync.Dispose();
            }
        }
    }
}
