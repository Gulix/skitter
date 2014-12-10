using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Classements
{
    public class ResultatsRonde2ViewModel : ResultatsRondeViewModel
    {
        protected override Tournoi.eTypePhaseTournoi GetPhaseSaisieNecessaire()
        {
            return Tournoi.eTypePhaseTournoi.SaisieRonde2;
        }
            
        public override string MessageBloquant
        {
            get { return "Le classement de la ronde 2 ne sera disponible qu'une fois la ronde 2 terminée."; }
        }
        
        public override List<Rencontre> GetListeRencontres()
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde1);
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde2);
            
            return lsRencontres;
        }

        protected override int NumeroRonde
        {
            get { return 2; }
        }
    }
}
