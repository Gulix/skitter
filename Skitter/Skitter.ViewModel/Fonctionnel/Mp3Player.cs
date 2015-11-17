using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace Skitter.ViewModel.Fonctionnel
{
    public static class Mp3Player
    {
        public static void Play(string sHymne)
        {
            string sFullPath = FileAndDirectory.GetAnthemFullPath(sHymne);
            if (string.IsNullOrEmpty(sFullPath))
                return;
            
            ProcessStartInfo startInfo = new ProcessStartInfo();
        	startInfo.UseShellExecute = false;
        	startInfo.FileName = FileAndDirectory.DlcPlayerExe;
            startInfo.Arguments = "-p \"" + sFullPath + "\"";
        
        	try
        	{
        	    // Start the process with the info we specified.
        	    // Call WaitForExit and then the using statement will close.
        	    using(Process process = Process.Start(startInfo))
        	    {
        	        process.WaitForExit();
        	    }
        	}
        	catch(Exception exc)
        	{
        	    MessageBox.Show("An error occured.\n" + exc.Message);
        	}
        }
    }
}
