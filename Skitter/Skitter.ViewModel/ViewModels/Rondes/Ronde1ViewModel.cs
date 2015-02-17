using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Rondes
{
    public class Ronde1ViewModel : RondeViewModel
    {
        public override string InformationsChoixInitialisation
        {
            get
            {
                return "La génération de la première ronde va bloquer définitivement la configuration des participants au tournoi.\n"
                  + string.Format("Il y a {0} participant{1} inscrit{1} au tournoi.",
                                  Tournoi.ListeParticipants.Count,
                                  (Tournoi.ListeParticipants.Count > 1) ? "s" : string.Empty);
            }
        }

        public override System.Windows.Visibility InformationAvantGenerationVisibility
        {
            get
            {
                return (Tournoi.TypePhaseTournoi <= Tournoi.eTypePhaseTournoi.ConfigurationParticipants)
                          ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        
        public override bool IsBoutonOrganiserSelonClassementEnabled {
            get {
                return false;
            }
        }

        public override Coach.eTypeRosterJoue TypeRosterJoue
        {
            get { return Coach.eTypeRosterJoue.RosterClassique; }
        }

        public override int NumeroRonde
        {
            get { return 1; }
        }

        #region Phases du tournoi
        protected override void ChangerEtatTournoiSurAnnulationOrganisation()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.ConfigurationParticipants;
        }

        protected override void ChangerEtatTournoiPhaseOrganisation()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.GenerationRonde1;
        }
        
        protected override void ChangerEtatTournoiPhaseSaisie()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.SaisieRonde1;
        }

        protected override void ChangerEtatTournoiValiderRonde()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.GenerationRonde2;
        }

        protected override void InitialiserEtatRonde()
        {
            if (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.GenerationRonde1)
                _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
            else if (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.SaisieRonde1)
                _typEtatRonde = eTypeEtatRonde.SaisieScore;
            else if (Tournoi.TypePhaseTournoi > Tournoi.eTypePhaseTournoi.SaisieRonde1)
                _typEtatRonde = eTypeEtatRonde.AffichageResultats;
            else
                _typEtatRonde = eTypeEtatRonde.ChoixInitialisation;
        }
        #endregion
        
        #region Valeurs classées (ne retournent rien, pas de classement en ronde 1)
        public override List<Equipe> GetListeEquipesSelonClassement()
        {
            return new List<Equipe>();
        }
        #endregion
    }
}
