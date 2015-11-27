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
        protected override List<Rencontre> RencontresDeLaRonde
        {
            get
            {
                return Tournoi.GetInstance().RencontresRonde1;
            }
        }

        public override string InformationsChoixInitialisation
        {
            get
            {
                return "La génération de la première ronde va bloquer définitivement la configuration des équipes du tournoi.\n"
                  + string.Format("Il y a {0} équipe{1} d'inscrite{1} au tournoi.",
                                  Tournoi.GetInstance().Equipes.Count,
                                  (Tournoi.GetInstance().Equipes.Count > 1) ? "s" : string.Empty);
            }
        }

        public override System.Windows.Visibility InformationAvantGenerationVisibility
        {
            get
            {
                return (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration)
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

        public override Coach.eTypeRosterJoue? TypeNextRosterJoue
        {
            get { return Coach.eTypeRosterJoue.EchangeRonde2; }
        }

        public override int NumeroRonde
        {
            get { return 1; }
        }

        #region Phases du tournoi
        protected override void ChangerEtatTournoiSurAnnulationOrganisation()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.Configuration;
        }

        protected override void ChangerEtatTournoiPhaseOrganisation()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.GenerationRonde1;
        }
        
        protected override void ChangerEtatTournoiPhaseSaisie()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.SaisieRonde1;
        }

        protected override void ChangerEtatTournoiValiderRonde()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.GenerationRonde2;
        }

        protected override void InitialiserEtatRonde()
        {
            if (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.GenerationRonde1)
                _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
            else if (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.SaisieRonde1)
                _typEtatRonde = eTypeEtatRonde.SaisieScore;
            else if (Tournoi.GetInstance().PhaseEnCours > Tournoi.eTypePhaseTournoi.SaisieRonde1)
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
        
        public override Skitter.ViewModel.ViewModels.Classements.ClassementCoachesViewModel GetClassementDesCoaches()
        {
            return null;
        }
        #endregion
    }
}
