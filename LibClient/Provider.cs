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
    public class Provider
    {
        public FileSyncProvider pro = null;
        FileSyncOptions optFileSync;
        FileSyncScopeFilter optFilter;

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
            try
            {
                pro = new FileSyncProvider(path, optFilter, optFileSync);
                //,set.metadataDirectoryPath, Settings.metaFileLoc,
                // set.tempDirectoryPath, set.pathToSaveConflictLoserFiles
            }
            finally
            {
                if (pro != null) pro.Dispose();
            }
        }

        public void DetectChanges()
        {
            if (pro == null) return;

            
                pro.DetectChanges();
        }

    }
}
