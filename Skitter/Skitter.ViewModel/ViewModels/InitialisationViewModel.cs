using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels
{
    public class InitialisationViewModel
    {
        public static void InitialiserNouveauTournoi()
        {
            Tournoi.ReinitialiserInstance();
        }

        public static void ChargerTournoiExistant(string sPath)
        {
            Tournoi.ChargerJSON(sPath);
        }
    }
}
