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
    public class ResultatsRonde4ViewModel : ResultatsRondeViewModel
    {
        protected override Tournoi.eTypePhaseTournoi GetPhaseSaisieNecessaire()
        {
            return Tournoi.eTypePhaseTournoi.SaisieRonde4;
        }
            
        public override string MessageBloquant
        {
            get { return "Le classement de la ronde 4 ne sera disponible qu'une fois la ronde 4 terminée."; }
        }
        
        public override List<Rencontre> GetListeRencontres()
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde1);
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde2);
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde3);
            lsRencontres.AddRange(Tournoi.GetInstance().RencontresRonde4);
            
            return lsRencontres;
        }

        protected override int NumeroRonde
        {
            get { return 4; }
        }
    }
}
