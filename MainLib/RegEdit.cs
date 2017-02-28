using System;
using Microsoft.Win32;

namespace MainLib
{
    public static class RegEdit
    {
        public static void ServerUpdate(string path) // IPAddress ip, int port, Guid guid,
        {
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
            if (path != null) keySv.SetValue("Path", path);

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
            o = keySv.GetValue("Path");

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
