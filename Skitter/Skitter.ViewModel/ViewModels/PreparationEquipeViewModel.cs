using System;
using System.Linq;
using System.ComponentModel;
using Skitter.Object;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using Skitter.ViewModel.Fonctionnel;
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.ViewModel.ViewModels
{
    public class PreparationEquipeViewModel : INotifyPropertyChanged
    {
        #region Variables
        Equipe _equipe;
        Rencontre _rencontre;

        CoachRondeViewModel _coachRang1;
        CoachRondeViewModel _coachRang2;
        CoachRondeViewModel _coachRang3;

        Coach.eTypeRosterJoue _typRosterJoue;
        int _iNumeroRonde;

        public event Action RafraichirControle;
        #endregion

        #region Accesseurs
        public string NomEquipe
        {
            get { return _equipe.NomEquipe; }
        }

        public int IdEquipe
        {
            get { return _equipe.IdEquipe; }
        }

        public CoachRondeViewModel CoachRang1
        {
            get { return _coachRang1; }
        }

        public CoachRondeViewModel CoachRang2
        {
            get { return _coachRang2; }
        }

        public CoachRondeViewModel CoachRang3
        {
            get { return _coachRang3; }
        }

        public int NumeroRonde
        {
            get { return _iNumeroRonde; }
        }

        public Coach.eTypeRosterJoue TypeRosterJoue
        {
            get { return _typRosterJoue; }
        }
        #endregion

        #region Constructeur
        public PreparationEquipeViewModel(Equipe equipe, Rencontre rencontre, int iNumeroRonde, Coach.eTypeRosterJoue typRosterJoue,
            Action actRafraichirControle)
        {
            _equipe = equipe;
            _rencontre = rencontre;
            _typRosterJoue = typRosterJoue;
            _iNumeroRonde = iNumeroRonde;

            RafraichirControle += actRafraichirControle;

            InitialiserCoachViewModel();
        }
        #endregion

        public void OnRafraichirControles()
        {
            if (RafraichirControle != null)
                RafraichirControle();
        }

        internal void ModifierRencontre(Rencontre rencontre)
        {
            _rencontre = rencontre;
        }

        #region Initialisation des CoachViewModel
        private void InitialiserCoachViewModel()
        {
            if (_rencontre != null)
            {
                bool bEquipe1 = (_rencontre.IdEquipe1 == _equipe.IdEquipe);
                _coachRang1 = RenseignerCoachSelonDuel(_rencontre.Duel1, bEquipe1);
                _coachRang2 = RenseignerCoachSelonDuel(_rencontre.Duel2, bEquipe1);
                _coachRang3 = RenseignerCoachSelonDuel(_rencontre.Duel3, bEquipe1);
            }

            if ((_coachRang1 == null) && (_coachRang2 == null) && (_coachRang3 == null))
            {
                _coachRang1 = new CoachRondeViewModel(_equipe.Capitaine, _iNumeroRonde);
                _coachRang2 = new CoachRondeViewModel(_equipe.Equipier1, _iNumeroRonde);
                _coachRang3 = new CoachRondeViewModel(_equipe.Equipier2, _iNumeroRonde);
            }
        }

        private CoachRondeViewModel RenseignerCoachSelonDuel(Duel duel, bool bEquipe1)
        {
            if (duel != null)
            {
                if ((bEquipe1 && (duel.IdCoach1 == _equipe.Capitaine.IdCoach)) || (!bEquipe1 && (duel.IdCoach2 == _equipe.Capitaine.IdCoach)))
                    return new CoachRondeViewModel(_equipe.Capitaine, _iNumeroRonde);
                else if ((bEquipe1 && (duel.IdCoach1 == _equipe.Equipier1.IdCoach)) || (!bEquipe1 && (duel.IdCoach2 == _equipe.Equipier1.IdCoach)))
                    return new CoachRondeViewModel(_equipe.Equipier1, _iNumeroRonde);
                else if ((bEquipe1 && (duel.IdCoach1 == _equipe.Equipier2.IdCoach)) || (!bEquipe1 && (duel.IdCoach2 == _equipe.Equipier2.IdCoach)))
                    return new CoachRondeViewModel(_equipe.Equipier2, _iNumeroRonde);
            }

            return null;
        }
        #endregion

        #region Ordre des coaches
        public void MonterCoachRang2()
        {
            CoachRondeViewModel coachTmp = _coachRang1;
            _coachRang1 = _coachRang2;
            _coachRang2 = coachTmp;

            if (_rencontre.IdEquipe1 == _equipe.IdEquipe)
            {
                _rencontre.Duel1.IdCoach1 = _coachRang1.IdCoach;
                _rencontre.Duel2.IdCoach1 = _coachRang2.IdCoach;
            }
            else if (_rencontre.IdEquipe2 == _equipe.IdEquipe)
            {
                _rencontre.Duel1.IdCoach2 = _coachRang1.IdCoach;
                _rencontre.Duel2.IdCoach2 = _coachRang2.IdCoach;
            }


            RaisePropertyChanged("CoachRang1");
            RaisePropertyChanged("CoachRang2");
            OnRafraichirControles();
        }

        public void DescendreCoachRang2()
        {
            CoachRondeViewModel coachTmp = _coachRang3;
            _coachRang3 = _coachRang2;
            _coachRang2 = coachTmp;

            if (_rencontre.IdEquipe1 == _equipe.IdEquipe)
            {
                _rencontre.Duel2.IdCoach1 = _coachRang2.IdCoach;
                _rencontre.Duel3.IdCoach1 = _coachRang3.IdCoach;
            }
            else if (_rencontre.IdEquipe2 == _equipe.IdEquipe)
            {
                _rencontre.Duel2.IdCoach2 = _coachRang2.IdCoach;
                _rencontre.Duel3.IdCoach2 = _coachRang3.IdCoach;
            }

            RaisePropertyChanged("CoachRang3");
            RaisePropertyChanged("CoachRang2");
            OnRafraichirControles();
        }

        #endregion

        #region Contrôles sur les coaches
        public void RaisePropertiesControles()
        {
            RaisePropertyChanged("Coach1BackgroundBrush");
            RaisePropertyChanged("Coach2BackgroundBrush");
            RaisePropertyChanged("Coach3BackgroundBrush");
            RaisePropertyChanged("Coach1ControleTooltip");
            RaisePropertyChanged("Coach2ControleTooltip");
            RaisePropertyChanged("Coach3ControleTooltip");
        }

        public Brush Coach1BackgroundBrush
        {
            get { return ControleRencontreCoach.GetBrushSelonControle(Tournoi.GetCoach(CoachRang1.IdCoach), _rencontre, NumeroRonde, TypeRosterJoue); }
        }
        
        public Brush Coach2BackgroundBrush
        {
            get { return ControleRencontreCoach.GetBrushSelonControle(Tournoi.GetCoach(CoachRang2.IdCoach), _rencontre, NumeroRonde, TypeRosterJoue); }
        }

        public Brush Coach3BackgroundBrush
        {
            get { return ControleRencontreCoach.GetBrushSelonControle(Tournoi.GetCoach(CoachRang3.IdCoach), _rencontre, NumeroRonde, TypeRosterJoue); }
        }

        private string GetCoachToolTip(Coach coach)
        {
            if (coach == null)
                return string.Empty;
            string sRetour = ClassementCoachesViewModel.GetClassementBilanAvantRonde(coach, NumeroRonde);
            if (!string.IsNullOrEmpty(sRetour))
                sRetour += "\n";
            sRetour += ControleRencontreCoach.GetTooltipSelonControle(coach, _rencontre, NumeroRonde, TypeRosterJoue);
            return sRetour;
        }

        public string Coach1ControleTooltip
        {
            get { return GetCoachToolTip(Tournoi.GetCoach(CoachRang1.IdCoach)); }
        }

        public string Coach2ControleTooltip
        {
            get { return GetCoachToolTip(Tournoi.GetCoach(CoachRang2.IdCoach)); }
        }

        public string Coach3ControleTooltip
        {
            get { return GetCoachToolTip(Tournoi.GetCoach(CoachRang3.IdCoach)); }
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
