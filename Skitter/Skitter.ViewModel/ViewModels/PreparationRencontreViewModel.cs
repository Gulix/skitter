using System;
using System.Linq;
using System.ComponentModel;
using Skitter.Object;
using System.Collections.Generic;
using System.Windows.Media;

namespace Skitter.ViewModel.ViewModels
{
    public class PreparationRencontreViewModel : INotifyPropertyChanged
    {
        #region Variables
        Rencontre _rencontre;

        PreparationEquipeViewModel _equipe1ViewModel;
        PreparationEquipeViewModel _equipe2ViewModel;

        bool _bEquipe1Selectionnee;
        bool _bEquipe2Selectionnee;

        int _iNumeroTable;
        int _iNumeroRonde;

        event Action EchangerEquipeAction;
        #endregion

        #region Accesseur
        public string IndicationTable
        {
            get { return string.Format("Table {0}", _iNumeroTable); }
        }

        private void RenseignerEquipe1(PreparationEquipeViewModel equipe)
        {
            _equipe1ViewModel = equipe;

            _rencontre.IdParticipant1 = _equipe1ViewModel.IdEquipe;
            _rencontre.ListeDuels[0].IdCoach1 = _equipe1ViewModel.CoachRang1.IdCoach;
            _rencontre.ListeDuels[1].IdCoach1 = _equipe1ViewModel.CoachRang2.IdCoach;
            _rencontre.ListeDuels[2].IdCoach1 = _equipe1ViewModel.CoachRang3.IdCoach;

            _equipe1ViewModel.ModifierRencontre(_rencontre);
        }

        private void RenseignerEquipe2(PreparationEquipeViewModel equipe)
        {
            _equipe2ViewModel = equipe;

            _rencontre.IdParticipant2 = _equipe2ViewModel.IdEquipe;
            _rencontre.ListeDuels[0].IdCoach2 = _equipe2ViewModel.CoachRang1.IdCoach;
            _rencontre.ListeDuels[1].IdCoach2 = _equipe2ViewModel.CoachRang2.IdCoach;
            _rencontre.ListeDuels[2].IdCoach2 = _equipe2ViewModel.CoachRang3.IdCoach;

            _equipe2ViewModel.ModifierRencontre(_rencontre);
        }

        public PreparationEquipeViewModel Equipe1
        {
            get { return _equipe1ViewModel; }
        }

        public PreparationEquipeViewModel Equipe2
        {
            get { return _equipe2ViewModel; }
        }
        #endregion

        #region Sélection d'équipe pour inversion
        public bool Equipe1Selectionnee
        {
            get { return _bEquipe1Selectionnee; }
            set 
            { 
                _bEquipe1Selectionnee = value;
                RaiseSelectionPropertyChanged();
                RafraichirControle();
                OnSelectionEquipe();
            }
        }

        public string MessageBoutonSelectionEquipe1
        {
            get { return Equipe1Selectionnee ? "Désélectionner" : "Sélectionner"; }
        }

        public Brush BackgroundBrushEquipe1
        {
            get
            {
                if (Equipe1Selectionnee)
                    return GetBrushSelected();
                else if (IsRencontreDejaJouee())
                    return GetBrushRencontreDejaJouee();
                else
                    return Brushes.Transparent;
            }
        }

        public string MessageEquipe1
        {
            get
            {
                if (IsRencontreDejaJouee())
                    return "Rencontre déjà jouée.";
                return string.Empty;
            }
        }
        
        public bool Equipe2Selectionnee
        {
            get { return _bEquipe2Selectionnee; }
            set 
            { 
                _bEquipe2Selectionnee = value;
                RaiseSelectionPropertyChanged();
                RafraichirControle();
                OnSelectionEquipe();
            }
        }

        public string MessageBoutonSelectionEquipe2
        {
            get { return Equipe2Selectionnee ? "Désélectionner" : "Sélectionner"; }
        }

        void RaiseSelectionPropertyChanged()
        {
            RaisePropertyChanged("Equipe1Selectionnee");
            RaisePropertyChanged("MessageBoutonSelectionEquipe1");
            RaisePropertyChanged("Equipe2Selectionnee");
            RaisePropertyChanged("MessageBoutonSelectionEquipe2");
        }
        #endregion

        #region Contrôles de l'équipe
        public Brush BackgroundBrushEquipe2
        {
            get
            {
                if (Equipe2Selectionnee)
                    return GetBrushSelected();
                else if (IsRencontreDejaJouee())
                    return GetBrushRencontreDejaJouee();
                else
                    return Brushes.Transparent;
            }
        }

        public string MessageEquipe2
        {
            get
            {
                if (IsRencontreDejaJouee())
                    return "Rencontre déjà jouée.";
                return string.Empty;
            }
        }

        private void OnSelectionEquipe()
        {
            if (EchangerEquipeAction != null)
                EchangerEquipeAction();
        }

        private bool IsRencontreDejaJouee()
        {
            return Tournoi.GetRencontresAvant(_iNumeroRonde).Any(r => IsRencontresIdentiques(r, _rencontre));
        }

        private bool IsRencontresIdentiques(Rencontre rencontre1, Rencontre rencontre2)
        {
            if ((rencontre1 == null) || (rencontre2 == null))
                return false;

            return ((rencontre1.IdParticipant1 == rencontre2.IdParticipant1) && (rencontre1.IdParticipant2 == rencontre2.IdParticipant2))
                || ((rencontre1.IdParticipant1 == rencontre2.IdParticipant2) && (rencontre1.IdParticipant2 == rencontre2.IdParticipant1));
        }

        private SolidColorBrush GetBrushSelected()
        {
             return new SolidColorBrush(Color.FromArgb(255, 181, 224, 255));
        }

        private SolidColorBrush GetBrushRencontreDejaJouee()
        {
             return new SolidColorBrush(Color.FromArgb(255, 255, 58, 58));
        }

        public void RafraichirControle()
        {
            RaisePropertyChanged("BackgroundBrushEquipe1");
            RaisePropertyChanged("BackgroundBrushEquipe2");
            RaisePropertyChanged("MessageEquipe1");
            RaisePropertyChanged("MessageEquipe2");
            _equipe1ViewModel.RaisePropertiesControles();
            _equipe2ViewModel.RaisePropertiesControles();
        }
        #endregion

        #region Constructeur
        public PreparationRencontreViewModel(Rencontre rencontre, int iNumeroTable, int iNumeroRonde, Coach.eTypeRosterJoue typRosterJoue, Action actEchangerEquipe)
        {
            _rencontre = rencontre;
            _iNumeroTable = iNumeroTable;
            _iNumeroRonde = iNumeroRonde;

            Equipe equipe = Tournoi.GetParticipant(_rencontre.IdParticipant1) as Equipe;
            if (equipe == null)
                throw new ArgumentException("Il n'y a pas d'équipe 1 pour la rencontre.");
            _equipe1ViewModel = new PreparationEquipeViewModel(equipe, rencontre, _iNumeroRonde, typRosterJoue, RafraichirControle);
            equipe = Tournoi.GetParticipant(_rencontre.IdParticipant2) as Equipe;
            if (equipe == null)
                throw new ArgumentException("Il n'y a pas d'équipe 2 pour la rencontre.");
            _equipe2ViewModel = new PreparationEquipeViewModel(equipe, rencontre, _iNumeroRonde, typRosterJoue, RafraichirControle);

            EchangerEquipeAction += actEchangerEquipe;
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

        public enum eTypeEchange
        {
            Echange1Vers1,
            Echange2Vers2,
            Echange1Vers2
        }

        public static void RealiserEchangeEquipe(PreparationRencontreViewModel rencontre1, PreparationRencontreViewModel rencontre2, eTypeEchange typEchange)
        {
            PreparationEquipeViewModel equipeTamponRencontre1 = (typEchange == eTypeEchange.Echange2Vers2) ?
                rencontre1._equipe2ViewModel : rencontre1._equipe1ViewModel;
            equipeTamponRencontre1.RafraichirControle -= rencontre1.RafraichirControle;
            equipeTamponRencontre1.RafraichirControle += rencontre2.RafraichirControle;
            PreparationEquipeViewModel equipeTamponRencontre2 = (typEchange == eTypeEchange.Echange1Vers1) ?
                rencontre2._equipe1ViewModel : rencontre2._equipe2ViewModel;
            equipeTamponRencontre2.RafraichirControle -= rencontre2.RafraichirControle;
            equipeTamponRencontre2.RafraichirControle += rencontre1.RafraichirControle;

            if (typEchange == eTypeEchange.Echange2Vers2)
                rencontre1.RenseignerEquipe2(equipeTamponRencontre2);
            else
                rencontre1.RenseignerEquipe1(equipeTamponRencontre2);

            if (typEchange == eTypeEchange.Echange1Vers1)
                rencontre2.RenseignerEquipe1(equipeTamponRencontre1);
            else
                rencontre2.RenseignerEquipe2(equipeTamponRencontre1);

            rencontre1._bEquipe1Selectionnee = rencontre1._bEquipe2Selectionnee = false;
            rencontre2._bEquipe1Selectionnee = rencontre2._bEquipe2Selectionnee = false;

            rencontre1.RaisePropertyChanged("Equipe1");
            rencontre1.RaisePropertyChanged("Equipe2");
            rencontre2.RaisePropertyChanged("Equipe1");
            rencontre2.RaisePropertyChanged("Equipe2");
            rencontre1.RaiseSelectionPropertyChanged();
            rencontre2.RaiseSelectionPropertyChanged();
        }
    }
}
