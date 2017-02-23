using System;

namespace MainLib
{
    public delegate void FileEventHandler(object sender, FileEventArgs e);
    public delegate void InfoSendEventHandler(object sender, InfoEventArgs e);

    public class InfoEventArgs : EventArgs
    {
        string _ip;
        int _port;
        string _dir;
        public InfoEventArgs(string ip, int port, string dir)
        {
            _ip = ip;
            _port = port;
            _dir = dir;
        }

        public string IP
        {
            get { return _ip; }
        }


        public int Port
        {
            get { return _port; }
        }

        public string Path
        {
            get { return _dir; }
        }
    }

        public class FileEventArgs : EventArgs
    {

        double _ExecTime = 0;
        string _VirtualPath = null;

        public FileEventArgs(string vPath, long lastWriteTimeUtcTicks, double execTime)
        {
            _VirtualPath = vPath;
            _LastWriteTimeUtcTicks = lastWriteTimeUtcTicks;
            _ExecTime = execTime;
        }

        public double ExecTime
        {
            get { return _ExecTime; }
        }        

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



    }
}
