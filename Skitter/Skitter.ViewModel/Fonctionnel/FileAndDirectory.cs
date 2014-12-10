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

        public static string HymnesDirectory
        {
            get
            {
                return Path.Combine(ExeDirectory, "Hymnes");
            }
        }

        public static string GetCheminCompletPourHymne(string sHymne)
        {
            string sFullPath = Path.Combine(FileAndDirectory.HymnesDirectory, sHymne);
            if (File.Exists(sFullPath))
                return sFullPath;
            return string.Empty;
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
