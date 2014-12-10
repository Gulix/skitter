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
    public class ResultatsRonde1ViewModel : ResultatsRondeViewModel
    {
        protected override Tournoi.eTypePhaseTournoi GetPhaseSaisieNecessaire()
        {
            return Tournoi.eTypePhaseTournoi.SaisieRonde1;
        }
            
        public override string MessageBloquant
        {
            get { return "Le classement de la ronde 1 ne sera disponible qu'une fois la ronde 1 terminée."; }
        }
        
        public override List<Rencontre> GetListeRencontres()
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde1);
            
            return lsRencontres;
        }

        protected override int NumeroRonde
        {
            get { return 1; }
        }
    }
}
