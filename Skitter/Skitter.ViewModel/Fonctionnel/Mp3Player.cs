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
            string sFichierComplet = FileAndDirectory.GetCheminCompletPourHymne(sHymne);
            if (string.IsNullOrEmpty(sFichierComplet))
                return;
//
//            ProcessStartInfo _info =
//                new ProcessStartInfo("cmd", @"/C " + FileAndDirectory.DlcPlayerExe + " -p " + sFichierComplet);
//
//            // The following commands are needed to redirect the
//            // standard output.  This means that it will be redirected
//            // to the Process.StandardOutput StreamReader.
//            _info.RedirectStandardOutput = true;
//
//            // Set UseShellExecute to false.  This tells the process to run
//            // as a child of the invoking program, instead of on its own.
//            // This allows us to intercept and redirect the standard output.
//            _info.UseShellExecute = false;
//
//            // Set CreateNoWindow to true, to supress the creation of
//            // a new window
//            _info.CreateNoWindow = true;
//
//            // Create a process, assign its ProcessStartInfo and start it
//            Process _p = new Process();
//            _p.StartInfo = _info;
//            _p.Start();
            
            ProcessStartInfo startInfo = new ProcessStartInfo();
        	startInfo.UseShellExecute = false;
        	startInfo.FileName = FileAndDirectory.DlcPlayerExe;
        	startInfo.Arguments = "-p \"" + sFichierComplet + "\"";
        
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
        	    MessageBox.Show("Une erreur est survenue.\n" + exc.Message);
        	}
        }
    }
}
