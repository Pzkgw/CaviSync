using System;
using Microsoft.Win32;

namespace MainLib
{
    public static class RegEdit
    {
        public const string emptyDirectory = " ";

        const string
            ServerPathKeyName = "Path",
            ClientPathKeyName = "DirLocal",
            ServerIP = "ServerIP";
        public static void ServerUpdate(string path) // IPAddress ip, int port, Guid guid,
        {
            if (path == null) return;

            RegistryKey keySv;
            //= RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
            //                          Environment.Is64BitOperatingSystem
            //                              ? RegistryView.Registry64
            //                              : RegistryView.Registry32);

            // HKEY_LOCAL_MACHINE\

            keySv = Registry.LocalMachine.CreateSubKey(Optiuni.regPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return;

            keySv = keySv.CreateSubKey(Optiuni.regPathSrv, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return;

            //Microsoft.Win32.Registry.LocalMachine.CreateSubKey(Optiuni.regPathSrv + strCDir,
            //    RegistryKeyPermissionCheck.ReadWriteSubTree);

            //if (IPAddress.TryParse(ipLocal.ToString(), out ipLocalNonStr))

            //if (ip != null) keySv.SetValue("Ip", ip);
            //if (port > 0) keySv.SetValue("Port", port);
            //if (guid != Guid.Empty) keySv.SetValue("Guid", guid);
            keySv.SetValue(ServerPathKeyName, path);

            keySv.Close();

        }

        public static string ServerGetPath()
        {
            RegistryKey keySv = null;
            string retVal = null;

            keySv = Registry.LocalMachine.OpenSubKey(Optiuni.regPath, false);
            if (keySv == null) return retVal;

            keySv = keySv.OpenSubKey(Optiuni.regPathSrv, false);
            if (keySv == null) return retVal;


            object o = null;
            o = keySv.GetValue(ServerPathKeyName);

            if (o != null)
            {
                retVal = o.ToString();
            }

            keySv.Close();

            return retVal;

        }


        public static bool ClientUpdate(string serverIP) // IPAddress ip, int port, Guid guid,
        {
            if (serverIP == null) return false;

            RegistryKey keySv;

            keySv = Registry.LocalMachine.CreateSubKey(Optiuni.regPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return false;

            keySv = keySv.CreateSubKey(Optiuni.regPathCli, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return false;

            keySv.SetValue(ServerIP, serverIP);

            keySv.Close();

            return true;

        }

        public static bool ClientUpdate(string path, int idx) // IPAddress ip, int port, Guid guid,
        {
            if (path == null) return false;

            RegistryKey keySv;

            keySv = Registry.LocalMachine.CreateSubKey(Optiuni.regPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return false;

            keySv = keySv.CreateSubKey(Optiuni.regPathCli, RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (keySv == null) return false;

            // fara dubluri in lista
            if (!path.Equals(emptyDirectory))
            {
                for (int i = 1; i < 4; i++)
                {
                    if (i != idx && path.Equals(keySv.GetValue(ClientPathKeyName + i)))
                    {
                        keySv.Close();
                        return false;
                    }
                }
            }

            keySv.SetValue(ClientPathKeyName + idx, path);

            keySv.Close();

            return true;
        }

        public static string ClientGetPath(int idx)
        {
            RegistryKey keySv = null;
            string retVal = null;

            keySv = Registry.LocalMachine.OpenSubKey(Optiuni.regPath, false);
            if (keySv == null) return retVal;

            keySv = keySv.OpenSubKey(Optiuni.regPathCli, false);
            if (keySv == null) return retVal;


            object o = null;
            o = keySv.GetValue(ClientPathKeyName + idx);

            if (o != null)
            {
                retVal = o.ToString();
            }

            keySv.Close();

            return retVal;

        }

        public static string ClientGetServerIP()
        {
            RegistryKey keySv = null;
            string retVal = null;

            keySv = Registry.LocalMachine.OpenSubKey(Optiuni.regPath, false);
            if (keySv == null) return retVal;

            keySv = keySv.OpenSubKey(Optiuni.regPathCli, false);
            if (keySv == null) return retVal;

            object o = null;
            o = keySv.GetValue(ServerIP);

            if (o != null)
            {
                retVal = o.ToString();
            }

            keySv.Close();

            return retVal;
        }


        /*
        public void UpdateDeriv(int idClient, int port, string path)
        {
            RegistryKey keyCl = null;

            string keyStr = Optiuni.registryPath + strCDir + path;

            keyCl = Registry.LocalMachine.OpenSubKey(keyStr, true);

            if (keyCl == null)
            {
                keyCl = Registry.LocalMachine.CreateSubKey(keyStr,
                    RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (keyCl.Name.Length > 254)
            {
                //"Name too long");
            }
            else
            {
                keyCl.SetValue("SyncTime", DateTime.Now.ToString());
            }

            keyCl.Close();
        }
        */



    }
}
