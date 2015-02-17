using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Skitter.Object;
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.ViewModel.ViewModels.Rondes
{
    /// <summary>
    /// Description of Ronde4ViewModel.
    /// </summary>
    public class Ronde4ViewModel : RondeViewModel
    {
        public override string InformationsChoixInitialisation
        {
            get
            {
                return "La génération de la quatrième ronde peut commencer si la saisie de la ronde 3 est terminée.";
            }
        }

        public override Visibility InformationAvantGenerationVisibility
        {
            get
            {
                return (Tournoi.TypePhaseTournoi <= Tournoi.eTypePhaseTournoi.GenerationRonde4)
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
            get { return Coach.eTypeRosterJoue.EchangeRonde4; }
        }

        public override int NumeroRonde
        {
            get { return 4; }
        }

        #region Phases du tournoi
        protected override void ChangerEtatTournoiSurAnnulationOrganisation()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.SaisieRonde3;
        }

        protected override void ChangerEtatTournoiPhaseOrganisation()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.GenerationRonde4;
        }

        protected override void ChangerEtatTournoiPhaseSaisie()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.SaisieRonde4;
        }

        protected override void ChangerEtatTournoiValiderRonde()
        {
            Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.GenerationRonde5;
        }

        protected override void InitialiserEtatRonde()
        {
            if (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.GenerationRonde4)
            {
                if (RencontresDeLaRonde.Any())
                    _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
                else
                    _typEtatRonde = eTypeEtatRonde.ChoixInitialisation;
            }
                
            else if (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.SaisieRonde4)
                _typEtatRonde = eTypeEtatRonde.SaisieScore;
            else if (Tournoi.TypePhaseTournoi > Tournoi.eTypePhaseTournoi.SaisieRonde4)
                _typEtatRonde = eTypeEtatRonde.AffichageResultats;
            else
                _typEtatRonde = eTypeEtatRonde.PasAccessible;
        }
        #endregion
        
        #region Valeurs classées (basées sur les résultats de la ronde 2)
        public override List<Equipe> GetListeEquipesSelonClassement()
        {
            Classements.ResultatsRonde3ViewModel resultats = new Skitter.ViewModel.ViewModels.Classements.ResultatsRonde3ViewModel();
            resultats.TrierGeneral();
            List<Equipe> lsEquipes = new List<Equipe>();
            foreach(ResultatsEquipeViewModel resultatEquipe in resultats.ListeResultatsEquipes)
            {
                lsEquipes.Add(Tournoi.ListeEquipes.FirstOrDefault(e => e.IdEquipe == resultatEquipe.IdEquipe));
            }
            
            return lsEquipes;
        }
        #endregion
    }
}
