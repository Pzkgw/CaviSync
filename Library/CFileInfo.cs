using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CFileInfo
    {
        //public int FileAttributes;
        //public int CreationTimeUtcSec;
        public int
            LastWriteTimeUtcSecMinHou,
            LastWriteTimeUtcDayMonYea;

        public long Length; // in bytes
        public string FullName;
    }

    public class CFileInfoGeneral
    {
        /// <summary>
        /// Get all files from a directory, exluding cerain extensions
        /// First excluded extensions is for dir, next for files
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="excludedExtensions"></param>
        /// <returns></returns>
        public static CFileInfo[] GetFiles(String directory, string excludeDirNameStart, params string[] excludedExtensions)
        {
            List<CFileInfo> result = new List<CFileInfo>();
            Stack<string> stack = new Stack<string>();
            stack.Push(directory);

            //string[] files;
            //IEnumerable<string> files;

            while (stack.Count > 0)
            {
                string temp = stack.Pop();

                try
                {
                    // 'Where' -> fara anumite fisiere  Utils.ToArray
                    //files = (Directory.GetFiles(temp).Where(s => !FileIsExcluded(s, excludedExtensions)));

                    if (excludedExtensions.Length != 3) throw new Exception();

                    result.AddRange(GetFiles(temp, excludedExtensions));

                    foreach (string directoryName in
                      Directory.GetDirectories(temp))
                    {
                        // 'if' - > fara anumite directoare
                        if (DirIsExcluded(directoryName, excludeDirNameStart))
                        {
                            stack.Push(directoryName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception("Error retrieving file or directory.");
                    return null;
                }
            }

            return result.ToArray();
        }

        private static List<CFileInfo> GetFiles(string dirPath, params string[] excludedExtensions)
        {
            List<CFileInfo> retVal = new List<CFileInfo>();

            DirectoryInfo di = new DirectoryInfo(dirPath);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (!FileIsExcluded(fi.FullName, excludedExtensions))
                {
                    CFileInfo inf = new CFileInfo();

                    inf.FullName = fi.FullName;
                    inf.Length = fi.Length;
                    //inf.FileAttributes = (int)fi.Attributes;

                    inf.LastWriteTimeUtcSecMinHou =
                        fi.LastWriteTimeUtc.Hour * 3600 +
                        fi.LastWriteTimeUtc.Minute * 60 +
                        fi.LastWriteTimeUtc.Second;

                    inf.LastWriteTimeUtcDayMonYea =
                        fi.LastWriteTimeUtc.Year * 365 +
                        fi.LastWriteTimeUtc.Month * 31 +
                        fi.LastWriteTimeUtc.Day;

                    retVal.Add(inf);
                }

            }


            return retVal;
        }

        static bool FileIsExcluded(string s, params string[] excludedExtensions)
        {
            return (s.Length > 2 && (
                        s.EndsWith(excludedExtensions[0].Substring(2)) ||
                        s.EndsWith(excludedExtensions[1].Substring(2)) ||
                        s.EndsWith(excludedExtensions[2].Substring(2))));
        }

        static bool DirIsExcluded(string s, string excludeDirNameStart)
        {
            return(
                s[s.Length - 1] != excludeDirNameStart[0] &&
                s[s.Length - 2] != excludeDirNameStart[1] &&
                s[s.Length - 3] != excludeDirNameStart[2]);
        }

    }
}
