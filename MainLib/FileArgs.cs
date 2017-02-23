using System;

namespace MainLib
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs : EventArgs
    {

        double _ExecTime = 0;

        public double ExecTime
        {
            get { return _ExecTime; }
        }


        string _VirtualPath = null;

        public string VirtualPath
        {
            get { return _VirtualPath; }
        }

        long _LastWriteTimeUtcTicks = 0;
        /// <summary>
        /// Gets the time, in coordinated universal time (UTC), 
        /// when the current file or directory was last written to
        /// </summary>
        public long LastWriteTimeUtcTicks
        {
            get { return _LastWriteTimeUtcTicks; }
        }


        public FileEventArgs(string vPath, long lastWriteTimeUtcTicks, double execTime)
        {
            _VirtualPath = vPath;
            _LastWriteTimeUtcTicks = lastWriteTimeUtcTicks;
            _ExecTime = execTime;
        }
    }
}
