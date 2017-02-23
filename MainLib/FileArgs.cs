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

        string _LastWriteTimeUtc = null;
        /// <summary>
        /// Gets the time, in coordinated universal time (UTC), 
        /// when the current file or directory was last written to
        /// </summary>
        public string LastWriteTimeUtc
        {
            get { return _LastWriteTimeUtc; }
        }


        public FileEventArgs(string vPath, string lastWriteTimeUtc, double execTime)
        {
            _VirtualPath = vPath;
            _LastWriteTimeUtc = lastWriteTimeUtc;
            _ExecTime = execTime;
        }
    }
}
