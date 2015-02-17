using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.ViewModel.Fonctionnel
{
    static class Conv
    {
        /// <summary>
        /// Convertit une chaine en valeur entière, avec une valeur existante qui est conservée si la conversion ne fonctionne pas.
        /// </summary>
        public static int IntStringVersInt(int valeurExistante, string nouvelleValeur)
        {
            int iConvert = 0;
            if (int.TryParse(nouvelleValeur, out iConvert))
                return iConvert;
            return valeurExistante;
        }

        public static string ListToString(List<string> lsChaines, string sSeparateur)
        {
            string sSep = string.Empty;
            string sRetour = string.Empty;
            foreach(string s in lsChaines)
            {
                sRetour += sSep + s;
                sSep = sSeparateur;
            }

            return sRetour;
        }
    }
}
