using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;

namespace LibClient
{
    public class Provider : IDisposable
    {
        public FileSyncProvider sync = null;
        FileSyncOptions optFileSync;
        FileSyncScopeFilter optFilter;

        Exception ex;

        public Provider()
        {
            optFileSync = FileSyncOptions.ExplicitDetectChanges;

            FileAttributes excludeFileAttributes =
            FileAttributes.System | FileAttributes.Hidden;

            optFilter = new FileSyncScopeFilter();

            optFilter.AttributeExcludeMask = excludeFileAttributes;
            foreach (string s in Settings.excludeFileExtensions)
                optFilter.FileNameExcludes.Add(s);
        }

        public void Start(string path)
        {
            ex = null;
            try
            {
                sync = new FileSyncProvider(path, optFilter, optFileSync);
                //,set.metadataDirectoryPath, Settings.metaFileLoc,
                // set.tempDirectoryPath, set.pathToSaveConflictLoserFiles
            }
            catch(Exception exc)
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

            sync.DetectChanges();
            try
            {
                
            }
            catch { }
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
