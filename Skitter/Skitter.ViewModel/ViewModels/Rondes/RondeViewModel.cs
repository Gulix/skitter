﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Nustache.Core;
using Skitter.Object;
using Skitter.ViewModel.Fonctionnel;
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.ViewModel.ViewModels.Rondes
{
    /// <summary>
    /// Classe abstraite permettant de mutualiser le code commun à toutes les rondes
    /// </summary>
    public abstract class RondeViewModel : INotifyPropertyChanged
    {
        #region Etat de la ronde
        public enum eTypeEtatRonde
        {
            PasAccessible = 1,
            ChoixInitialisation = 2,
            OrganisationMatches = 3,
            SaisieScore = 4,
            AffichageResultats = 5
        }

        protected eTypeEtatRonde _typEtatRonde;
        #endregion

        #region Méthodes abstraites spécifiques à chaque ronde
        protected abstract void ChangerEtatTournoiSurAnnulationOrganisation();
        protected abstract void ChangerEtatTournoiPhaseOrganisation();
        protected abstract void ChangerEtatTournoiPhaseSaisie();
        protected abstract void ChangerEtatTournoiValiderRonde();
        protected abstract void InitialiserEtatRonde();

        public abstract Visibility InformationAvantGenerationVisibility { get; }
        public abstract string InformationsChoixInitialisation { get; }
        
        public abstract bool IsBoutonOrganiserSelonClassementEnabled { get; }
        
        public abstract List<Equipe> GetListeEquipesSelonClassement();
        
        public abstract Coach.eTypeRosterJoue TypeRosterJoue { get; }
        public abstract int NumeroRonde { get; }
        #endregion

        #region Classements précédents
        protected ClassementCoachesViewModel GetClassementDesCoaches()
        {
            List<Rencontre> lsRencontres = Tournoi.GetRencontresAvant(NumeroRonde);
            if ((lsRencontres == null) || !lsRencontres.Any())
                return null;
            
            return new Classements.ClassementCoachesViewModel(lsRencontres);
        }
        #endregion

        #region Liste des rencontres
        List<PreparationRencontreViewModel> _lsPrepaRencontresVM;
        public List<PreparationRencontreViewModel> PreparationRencontresViewModel
        {
            get
            {
                if (_lsPrepaRencontresVM == null)
                    GenererRencontresViewModel();
                return _lsPrepaRencontresVM;
            }
        }

        private void GenererRencontresViewModel()
        {
            _lsPrepaRencontresVM = new List<PreparationRencontreViewModel>();
            for (int iRenc = 0; iRenc < RencontresDeLaRonde.Count; iRenc++)
                _lsPrepaRencontresVM.Add(new PreparationRencontreViewModel(RencontresDeLaRonde[iRenc], iRenc + 1, NumeroRonde, TypeRosterJoue, VerifierEchangeEquipe));
            RaisePropertyChanged("PreparationRencontresViewModel");
        }

        private void VerifierEchangeEquipe()
        {
            // On parcourt les Rencontres, pour vérifier si deux équipes sont sélectionnées
            if (PreparationRencontresViewModel.Count(vm => vm.Equipe1Selectionnee)
                + PreparationRencontresViewModel.Count(vm => vm.Equipe2Selectionnee) == 2)
            {
                List<PreparationRencontreViewModel> lsRencontresVM =
                    PreparationRencontresViewModel.Where(vm => vm.Equipe1Selectionnee || vm.Equipe2Selectionnee).ToList();
                
                // Echange entre les deux équipes d'une même rencontre
                if (lsRencontresVM.Count == 1)
                {
                    PreparationRencontreViewModel.RealiserEchangeEquipe(lsRencontresVM[0], lsRencontresVM[0], PreparationRencontreViewModel.eTypeEchange.Echange1Vers2);
                }
                // Echange entre deux équipes "1"
                else if (lsRencontresVM.All(vm => vm.Equipe1Selectionnee))
                {
                    PreparationRencontreViewModel.RealiserEchangeEquipe(lsRencontresVM[0], lsRencontresVM[1], PreparationRencontreViewModel.eTypeEchange.Echange1Vers1);
                }
                // Echange entre deux équipes "2"
                else if (lsRencontresVM.All(vm => vm.Equipe2Selectionnee))
                {
                    PreparationRencontreViewModel.RealiserEchangeEquipe(lsRencontresVM[0], lsRencontresVM[1], PreparationRencontreViewModel.eTypeEchange.Echange2Vers2);
                }
                // Echange entre une équipe 1 et une équipe 2
                else
                {
                    PreparationRencontreViewModel rencontreEquipe1 = lsRencontresVM.FirstOrDefault(vm => vm.Equipe1Selectionnee);
                    PreparationRencontreViewModel rencontreEquipe2 = lsRencontresVM.FirstOrDefault(vm => vm.Equipe2Selectionnee);
                    PreparationRencontreViewModel.RealiserEchangeEquipe(rencontreEquipe1, rencontreEquipe2, PreparationRencontreViewModel.eTypeEchange.Echange1Vers2);
                }

                foreach (PreparationRencontreViewModel vm in lsRencontresVM)
                    vm.RafraichirControle();
            }
        }

        protected List<Rencontre> RencontresDeLaRonde 
        {
            get { return Tournoi.GetRencontresSelonRonde(this.NumeroRonde); }
        }
        #endregion

        #region Etat "Non-accessible"
        public string MessageNonAccessible
        {
            get { return "La ronde n'est pas accessible."; }
        }

        public Visibility VisibilityPasAccessible
        {
            get { return (_typEtatRonde == eTypeEtatRonde.PasAccessible) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public void RendreAccessible()
        {
            _typEtatRonde = eTypeEtatRonde.ChoixInitialisation;
            RaisePropertyChanged("MessageNonAccessible");
            RaisePropertyChanged("VisibilityPasAccessible");
            RaiseChoixInitialisationPropertiesChanged();
        }
        #endregion

        #region Etat "Choix de l'initialisation"
        private void RaiseChoixInitialisationPropertiesChanged()
        {
            RaisePropertyChanged("ErreurChoixInitialisation");
            RaisePropertyChanged("ErreurChoixInitialisationVisibility");
            RaisePropertyChanged("InformationsChoixInitialisation");
            RaisePropertyChanged("IsBoutonsCreationAleatoireEnabled");
            RaisePropertyChanged("IsBoutonsCreationClassementEnabled");
            RaisePropertyChanged("VisibilityChoixInitialisation");
        }

        public string ErreurChoixInitialisation
        {
            get 
            {
                if (Tournoi.ListeParticipants.Count % 2 == 1)
                    return "Le nombre de participants est impair.";

                if (Tournoi.ListeParticipants.Select(e => new ParticipantViewModel(e))
                                             .Any(vm => !string.IsNullOrEmpty(vm.Erreur)))
                    return "Des erreurs existent pour au moins un des participants.";

                return string.Empty;
            }
        }

        public Visibility ErreurChoixInitialisationVisibility
        {
            get
            {
                return string.IsNullOrEmpty(ErreurChoixInitialisation) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private bool IsBoutonsCreationRondeEnabled
        {
            get { return string.IsNullOrEmpty(ErreurChoixInitialisation); }
        }

        public bool IsBoutonsCreationAleatoireEnabled
        {
            get { return IsBoutonsCreationRondeEnabled; }
        }

        public bool IsBoutonsCreationClassementEnabled
        {
            get { return IsBoutonsCreationRondeEnabled && IsBoutonOrganiserSelonClassementEnabled; }
        }

        public Visibility VisibilityChoixInitialisation
        {
            get { return (_typEtatRonde == eTypeEtatRonde.ChoixInitialisation) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public void InitialiserSelonAleatoire()
        {
            _typEtatRonde = eTypeEtatRonde.OrganisationMatches;

            RencontresDeLaRonde.Clear();
            
            // Nouvelle liste des équipes triées aléatoirement
            Random rand = new Random(Tournoi.ListeParticipants.Sum(p => p.ListeCoaches.Sum(c => c.ValeurRoster)) + NumeroRonde);
            List<Equipe> lsEquipesAleatoire = Tournoi.ListeEquipes.OrderBy(c => rand.Next()).ToList();
            
            for (int i = 0; i < lsEquipesAleatoire.Count; i += 2)
            {
                Rencontre rencontre = new Rencontre();
                Equipe equipe1 = lsEquipesAleatoire[i];
                Equipe equipe2 = lsEquipesAleatoire[i + 1];
                rencontre.IdParticipant1 = equipe1.IdEquipe;
                rencontre.IdParticipant2 = equipe2.IdEquipe;
                for (int iCoach = 0; iCoach < 3; iCoach++)
                    rencontre.ListeDuels.Add(new Duel() { IdCoach1 = equipe1.ListeIdCoaches[iCoach], IdCoach2 = equipe2.ListeIdCoaches[iCoach] });
                                
                RencontresDeLaRonde.Add(rencontre);
            }
            GenererRencontresViewModel();
            ChangerEtatTournoiPhaseOrganisation();

            RaiseChoixInitialisationPropertiesChanged();
            RaiseOrganisationMatchesPropertiesChanged();
        }

        public void InitialiserSelonClassement()
        {
            _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
            
            RencontresDeLaRonde.Clear();
            
            List<Equipe> lsEquipesClassement = GetListeEquipesSelonClassement();
            ClassementCoachesViewModel classementCoaches = GetClassementDesCoaches();
            
            for (int i = 0; i < lsEquipesClassement.Count; i += 2)
            {
                Rencontre rencontre = new Rencontre();
                Equipe equipe1 = lsEquipesClassement[i];
                Equipe equipe2 = lsEquipesClassement[i + 1];
                rencontre.IdParticipant1 = equipe1.IdEquipe;
                rencontre.IdParticipant2 = equipe2.IdEquipe;
                List<int> lsCoachesEquipe1 = GetListeCoachesEquipeSelonClassement(equipe1, classementCoaches);
                List<int> lsCoachesEquipe2 = GetListeCoachesEquipeSelonClassement(equipe2, classementCoaches);
                for (int iNbDuels = 0; iNbDuels < 3; iNbDuels++)
                    rencontre.ListeDuels.Add(new Duel() { IdCoach1 = lsCoachesEquipe1[iNbDuels], IdCoach2 = lsCoachesEquipe2[iNbDuels] });

                RencontresDeLaRonde.Add(rencontre);
            }
            GenererRencontresViewModel();
            ChangerEtatTournoiPhaseOrganisation();
            
            RaiseChoixInitialisationPropertiesChanged();
            RaiseOrganisationMatchesPropertiesChanged();
        }
        
        private List<int> GetListeCoachesEquipeSelonClassement(Equipe equipe, ClassementCoachesViewModel classementCoaches)
        {
            Dictionary<int, int> dicCoachesEquipe = new Dictionary<int, int>();
            for (int iCoach = 0; iCoach < equipe.ListeIdCoaches.Count; iCoach++)
                dicCoachesEquipe.Add(classementCoaches.ObtenirClassementCoach(equipe.ListeIdCoaches[iCoach]), equipe.ListeIdCoaches[iCoach]);
                    
            return dicCoachesEquipe.OrderBy(k => k.Key).Select(k => k.Value).ToList();
        }
        #endregion

        #region Etat "Organisation des matches"
        private void RaiseOrganisationMatchesPropertiesChanged()
        {
            RaisePropertyChanged("VisibilityOrganisationMatches");
            RaisePropertyChanged("Rencontres");
        }

        public Visibility VisibilityOrganisationMatches
        {
            get { return (_typEtatRonde == eTypeEtatRonde.OrganisationMatches) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public void AnnulerOrganisation()
        {
            _typEtatRonde = eTypeEtatRonde.ChoixInitialisation;
            ChangerEtatTournoiSurAnnulationOrganisation();
            RaiseChoixInitialisationPropertiesChanged();
            RaiseOrganisationMatchesPropertiesChanged();
        }

        public void ValiderOrganisation()
        {
            _typEtatRonde = eTypeEtatRonde.SaisieScore;
            ChangerEtatTournoiPhaseSaisie();
            _lsSaisieRencontres = null;
            RaiseOrganisationMatchesPropertiesChanged();
            RaiseSaisieMatchesPropertiesChanged();
        }
        #endregion

        #region Etat "Saisie des matches"
        private void RaiseSaisieMatchesPropertiesChanged()
        {
            RaisePropertyChanged("VisibilitySaisieMatches");
            RaisePropertyChanged("SaisieRencontres");
            RaisePropertyChanged("SaisieRencontreSelectionnee");
            RaisePropertyChanged("IsSaisieMatchesEnabled");
        }

        public Visibility VisibilitySaisieMatches
        {
            get { return (_typEtatRonde >= eTypeEtatRonde.SaisieScore) ? Visibility.Visible : Visibility.Collapsed; }
        }
        
        public bool IsSaisieMatchesEnabled
        {
            get { return (_typEtatRonde == eTypeEtatRonde.SaisieScore); }
        }

        List<SaisieRencontreViewModel> _lsSaisieRencontres;
        SaisieRencontreViewModel _saisieRencontreSelectionnee;
        public List<SaisieRencontreViewModel> SaisieRencontres
        {
            get
            {
                if (_lsSaisieRencontres == null)
                {
                    _lsSaisieRencontres = new List<SaisieRencontreViewModel>();
                    foreach (Rencontre rencontre in RencontresDeLaRonde)
                        _lsSaisieRencontres.Add(new SaisieRencontreViewModel(rencontre, TypeRosterJoue));
                }

                return _lsSaisieRencontres;
            }
        }

        public SaisieRencontreViewModel SaisieRencontreSelectionnee
        {
            get { return _saisieRencontreSelectionnee; }
            set { _saisieRencontreSelectionnee = value; RaisePropertyChanged("SaisieRencontreSelectionnee"); }
        }

        public void AnnulerSaisie()
        {
            _typEtatRonde = eTypeEtatRonde.OrganisationMatches;
            ChangerEtatTournoiPhaseOrganisation();
            RaiseOrganisationMatchesPropertiesChanged();
            RaiseSaisieMatchesPropertiesChanged();
        }

        public void ValiderSaisie()
        {
            _typEtatRonde = eTypeEtatRonde.AffichageResultats;
            ChangerEtatTournoiValiderRonde();
            
            RaiseSaisieMatchesPropertiesChanged();
        }
        #endregion

        #region Génération de la feuille des résultats
        public void GenererHTML()
        {
            GenerationFeuilleResultatViewModel generationVM =
                new GenerationFeuilleResultatViewModel(RencontresDeLaRonde, NumeroRonde, TypeRosterJoue);

            string exeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            Render.FileToFile(Path.Combine(exeDirectory, "template_rencontres.html"), generationVM, Path.Combine(exeDirectory, "rencontres.html"));
            Process.Start(Path.Combine(exeDirectory, "rencontres.html"));
        }

        public void GenererPresentation()
        {
            Dictionary<string, Dictionary<string, Rencontre>> dicRondes = new Dictionary<string,Dictionary<string,Rencontre>>();
            for(int i = 1; i <= NumeroRonde; i++)
            {
                string sRonde = "Ronde" + i.ToString();
                Dictionary<string, Rencontre> dicRencontres = new Dictionary<string, Rencontre>();
                List<Rencontre> lsRencontres = Tournoi.GetRencontresSelonRonde(i);
                for(int iRenc = 0; iRenc < lsRencontres.Count; iRenc++)
                {
                    dicRencontres.Add("Table" + (iRenc + 1).ToString(), lsRencontres[iRenc]);
                }

                dicRondes.Add(sRonde, dicRencontres);
            }
            string sDestination = Path.Combine(FileAndDirectory.ExeDirectory, "Strut", "AnnoncesRondes.htm");
            Render.FileToFile(Path.Combine(FileAndDirectory.ExeDirectory, "Strut", "AnnoncesRondes_template.htm"),
                dicRondes,
                sDestination);
            Process.Start(sDestination);
        }
        #endregion

        #region Constructeurs
        public RondeViewModel()
        {
            InitialiserEtatRonde();
        }
        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string sPropertyChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sPropertyChanged));
        }

        #endregion
    }
}
