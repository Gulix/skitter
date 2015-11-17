using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Skitter.ViewModel.Fonctionnel
{
    public static class FileAndDirectory
    {
        public static string ExeDirectory
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
        }

        /// <summary>
        /// Directory where to put the mp3 files for the Anthems
        /// </summary>
        public static string AnthemsDirectory
        {
            get
            {
                return Path.Combine(ExeDirectory, "Anthems");
            }
        }

        /// <summary>
        /// Get the full path to an Anthem
        /// </summary>
        /// <param name="sAnthem">The mp3 file anthem name</param>
        public static string GetAnthemFullPath(string sAnthem)
        {
            string sFullPath = Path.Combine(FileAndDirectory.AnthemsDirectory, sAnthem);
            if (!File.Exists(sFullPath))
            {
                sFullPath = string.Empty;
            }
            return sFullPath;
        }
        
        public static string DlcPlayerExe
        {
            get {return Path.Combine(ExeDirectory, "dlc.exe");}
        }

        public static string StrutDirectory
        {
            get { return Path.Combine(ExeDirectory, "Strut"); }
        }
    }
}
