using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Skitter.Object;
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.ViewModel.ViewModels.Rondes
{
    public class Ronde2ViewModel : RondeViewModel
    {
        protected override List<Rencontre> RencontresDeLaRonde
        {
            get
            {
                return Tournoi.GetInstance().RencontresRonde2;
            }
        }

        public override string InformationsChoixInitialisation
        {
            get
            {
                return "La génération de la seconde ronde peut commencer si la saisie de la ronde 1 est terminée.";
            }
        }

        public override Visibility InformationAvantGenerationVisibility
        {
            get
            {
                return (Tournoi.GetInstance().PhaseEnCours <= Tournoi.eTypePhaseTournoi.GenerationRonde2)
                          ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        
        public override bool IsBoutonOrganiserSelonClassementEnabled {
            get {
                return true;
            }
        }

        public override Coach.eTypeRosterJoue TypeRosterJoue
        {
            get { return Coach.eTypeRosterJoue.EchangeRonde2; }
        }

        public override Coach.eTypeRosterJoue? TypeNextRosterJoue
        {
            get { return Coach.eTypeRosterJoue.RosterClassique; }
        }

        public override int NumeroRonde
        {
            get { return 2; }
        }

        #region Phases du tournoi
        protected override void ChangerEtatTournoiSurAnnulationOrganisation()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.SaisieRonde1;
        }

        protected override void ChangerEtatTournoiPhaseOrganisation()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.GenerationRonde2;
        }

        protected override void ChangerEtatTournoiPhaseSaisie()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.SaisieRonde2;
        }

        protected override void ChangerEtatTournoiValiderRonde()
        {
            Tournoi.GetInstance().PhaseEnCours = Tournoi.eTypePhaseTournoi.GenerationRonde3;
        }

        protected override void InitialiserEtatRonde()
        {
            if (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.GenerationRonde2)
            {
                if (Tournoi.GetInstance().RencontresRonde2.Any())
                    _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
                else
                    _typEtatRonde = eTypeEtatRonde.ChoixInitialisation;
            }
                
            else if (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.SaisieRonde2)
                _typEtatRonde = eTypeEtatRonde.SaisieScore;
            else if (Tournoi.GetInstance().PhaseEnCours > Tournoi.eTypePhaseTournoi.SaisieRonde2)
                _typEtatRonde = eTypeEtatRonde.AffichageResultats;
            else
                _typEtatRonde = eTypeEtatRonde.PasAccessible;
        }
        #endregion
        
        #region Valeurs classées (basées sur les résultats de la ronde 1)
        public override List<Equipe> GetListeEquipesSelonClassement()
        {
            Classements.ResultatsRonde1ViewModel resultats = new Skitter.ViewModel.ViewModels.Classements.ResultatsRonde1ViewModel();
            resultats.TrierGeneral();
            List<Equipe> lsEquipes = new List<Equipe>();
            foreach(ResultatsEquipeViewModel resultatEquipe in resultats.ListeResultatsEquipes)
            {
                lsEquipes.Add(Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.IdEquipe == resultatEquipe.IdEquipe));
            }
            
            return lsEquipes;
        }
        
        public override Skitter.ViewModel.ViewModels.Classements.ClassementCoachesViewModel GetClassementDesCoaches()
        {
            return new Classements.ClassementCoachesViewModel(Tournoi.GetInstance().RencontresRonde1);
        }
        #endregion
    }
}
