using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Skitter.Object;
using Skitter.ViewModel.Fonctionnel;
using Skitter.ViewModel.ViewModels.Rondes;

namespace Skitter.ViewModel.ViewModels
{
    public class SaisieDuelViewModel : INotifyPropertyChanged
    {
        #region Variables
        Duel _duel;
        Coach.eTypeRosterJoue _typRosterJoue;
        #endregion

        #region ModificationScore
        public event Action ModificationScore;
        
        private void OnModificationScore()
        {
            if (ModificationScore != null)
                ModificationScore();
        }
        #endregion

        #region Accesseurs
        private Coach Coach1
        {
            get { return Tournoi.GetCoach(_duel.IdCoach1); }
        }

        private Coach Coach2
        {
            get { return Tournoi.GetCoach(_duel.IdCoach2); }
        }

        public string NomCoach1
        {
            get { return Coach1.NomCoach; }
        }

        public string InfosRosterCoach1
        {
            get
            {
                Coach coachRosterJoue = Coach1.GetCoachRosterSelonRondeJouee(_typRosterJoue);
                if (coachRosterJoue == null)
                    return string.Empty;
                if (coachRosterJoue.IdCoach == Coach1.IdCoach)
                    return string.Format("({0})", Coach1.NomRoster);

                return string.Format("({0} de {1})", coachRosterJoue.NomRoster, coachRosterJoue.NomCoach);
            }
        }

        public string NomCoach2
        {
            get { return Coach2.NomCoach; }
        }

        public string InfosRosterCoach2
        {
            get
            {
                Coach coachRosterJoue = Coach2.GetCoachRosterSelonRondeJouee(_typRosterJoue);
                if (coachRosterJoue == null)
                    return string.Empty;
                if (coachRosterJoue.IdCoach == Coach2.IdCoach)
                    return string.Format("({0})", Coach2.NomRoster);

                return string.Format("({0} de {1})", coachRosterJoue.NomRoster, coachRosterJoue.NomCoach);
            }
        }

        public string DescriptionMatch
        {
            get
            {
                return string.Format("{0} ({2} - {3}) {1}",
                    NomCoach1, NomCoach2, TdCoach1, TdCoach2);
            }
        }

        public string TdCoach1
        {
            get { return _duel.ResultatCoach1.NbTD.ToString(); }
            set
            {
                _duel.ResultatCoach1.NbTD = Conv.IntStringVersInt(_duel.ResultatCoach1.NbTD, value);
                RaisePropertyChanged("TdCoach1");
                OnModificationScore();
            }
        }

        public string SortiesCoach1
        {
            get { return _duel.ResultatCoach1.NbSorties.ToString(); }
            set
            {
                _duel.ResultatCoach1.NbSorties = Conv.IntStringVersInt(_duel.ResultatCoach1.NbSorties, value);
                RaisePropertyChanged("SortiesCoach1");
            }
        }

        public string SortiesVicieusesCoach1
        {
            get { return _duel.ResultatCoach1.NbSortiesVicieuses.ToString(); }
            set
            {
                _duel.ResultatCoach1.NbSortiesVicieuses = Conv.IntStringVersInt(_duel.ResultatCoach1.NbSortiesVicieuses, value);
                RaisePropertyChanged("SortiesVicieusesCoach1");
            }
        }

        public string TdCoach2
        {
            get { return _duel.ResultatCoach2.NbTD.ToString(); }
            set
            {
                _duel.ResultatCoach2.NbTD = Conv.IntStringVersInt(_duel.ResultatCoach2.NbTD, value);
                RaisePropertyChanged("TdCoach2");
                OnModificationScore();
            }
        }

        public string SortiesCoach2
        {
            get { return _duel.ResultatCoach2.NbSorties.ToString(); }
            set
            {
                _duel.ResultatCoach2.NbSorties = Conv.IntStringVersInt(_duel.ResultatCoach2.NbSorties, value);
                RaisePropertyChanged("SortiesCoach2");
            }
        }

        public string SortiesVicieusesCoach2
        {
            get { return _duel.ResultatCoach2.NbSortiesVicieuses.ToString(); }
            set
            {
                _duel.ResultatCoach2.NbSortiesVicieuses = Conv.IntStringVersInt(_duel.ResultatCoach2.NbSortiesVicieuses, value);
                RaisePropertyChanged("SortiesVicieusesCoach2");
            }
        }
        #endregion

        #region Constructeurs
        public SaisieDuelViewModel(Duel duel, Action scoreModifie, Coach.eTypeRosterJoue typRosterJoue)
        {
            _typRosterJoue = typRosterJoue;
            _duel = duel;
            ModificationScore += scoreModifie;
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
