using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.AccessControl;

namespace MainLib
{
    /// <summary>
    /// Metode utile generale
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Get all files from a directory, exluding cerain extensions
        /// First excluded extensions is for dir, next for files
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="excludedExtensions"></param>
        /// <returns></returns>
        public static List<string> GetDirectoryFileList(String directory, string excludeDirNameStart, params string[] excludedExtensions)
        {

            List<string> result = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(directory);

            while (stack.Count > 0)
            {
                string temp = stack.Pop();

                try
                {
                    // 'Where' -> fara anumite fisiere
                    result.AddRange(
                        Directory.GetFiles(temp).Where(s => !(
                        s.EndsWith(excludedExtensions[0].Substring(2)) ||
                        s.EndsWith(excludedExtensions[1].Substring(2)) ||
                        s.EndsWith(excludedExtensions[2].Substring(2)))));

                    foreach (string directoryName in
                      Directory.GetDirectories(temp))
                    {
                        // 'if' - > fara anumite directoare
                        if (directoryName.Length > 2 &&
                            directoryName[directoryName.Length - 1] != excludeDirNameStart[0] &&
                            directoryName[directoryName.Length - 2] != excludeDirNameStart[1] &&
                            directoryName[directoryName.Length - 3] != excludeDirNameStart[2])
                            stack.Push(directoryName);
                    }
                }
                catch //(Exception ex)
                {
                    //throw new Exception("Error retrieving file or directory.");
                    return null;
                }
            }

            return result;
        }



        /// <summary>
        /// As intented, get local IP
        /// </summary>
        /// <returns>Local IP</returns>
        public static IPAddress GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address;
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address
                : null;
        }


        /// <summary>
        /// True = path valid cu AccessControl sau URI care respecta conventia de nume UNC
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPath(string directoryPath)
        {
            try
            {
                AuthorizationRuleCollection rules = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                return true;
            }
            catch
            {
                Uri uri = null;

                try
                {
                    uri = new Uri(directoryPath);
                }
                catch
                {

                }

                if (uri != null)
                {
                    if (uri.IsUnc) return true;
                }

                return false;
            }
        }



        /// <summary>
        /// Command Prompt -> Executa comanda
        /// </summary>
        /// <param name="command"></param>
        public static int ExecuteCommand(string command)
        {
            int retVal = 0;

            ProcessStartInfo pi;
            Process proc;

            pi = new ProcessStartInfo("cmd.exe", "/c " + command);
            pi.CreateNoWindow = true;
            pi.UseShellExecute = false;

            proc = Process.Start(pi);
            proc.WaitForExit();

            retVal = proc.ExitCode;
            proc.Close();


            return retVal;
            //MessageBox.Show("ExitCode: " + ExitCode.ToString(), "ExecuteCommand");
        }


        public static bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }



    }
}
