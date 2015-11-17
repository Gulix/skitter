using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.ViewModel.ViewModels
{
    public class HymneViewModel
    {
        /// <summary>
        /// Retourne la liste des fichiers présents dans le répertoire des hymnes
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListeHymnesDisponibles()
        {
            // Les MP3 doivent être présents dans le répertoire de l'application, dans un sous-répertoire "Anthems"
            List<string> lsHymnes = new List<string>();
            try
            {
                string[] tHymnes = Directory.GetFiles(FileAndDirectory.AnthemsDirectory, "*.mp3");
                lsHymnes = tHymnes.ToList()
                                  .Select(f => new FileInfo(f))
                                  .Select(fi => fi.Name)
                                  .ToList();
            }
            catch
            {

            }
            return lsHymnes;
        }
    }
}
