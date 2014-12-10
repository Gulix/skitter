using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels
{
    public class SaisieRencontreViewModel : INotifyPropertyChanged
    {
        #region Variables
        Rencontre _rencontre;

        SaisieDuelViewModel _duel1VM;
        SaisieDuelViewModel _duel2VM;
        SaisieDuelViewModel _duel3VM;
        #endregion

        #region Accesseurs
        public string NomEquipe1
        {
            get { return Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.IdEquipe == _rencontre.IdEquipe1).NomEquipe; }
        }

        public string NomEquipe2
        {
            get { return Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.IdEquipe == _rencontre.IdEquipe2).NomEquipe; }
        }

        public string ScoreEquipe1
        {
            get { return _rencontre.ScoreEquipe1.ToString(); }
        }

        public string ScoreEquipe2
        {
            get { return _rencontre.ScoreEquipe2.ToString(); }
        }

        public SaisieDuelViewModel Duel1
        {
            get { return _duel1VM; }
        }

        public SaisieDuelViewModel Duel2
        {
            get { return _duel2VM; }
        }

        public SaisieDuelViewModel Duel3
        {
            get { return _duel3VM; }
        }
        #endregion

        private void OnScoreModifie()
        {
            RaisePropertyChanged("ScoreEquipe1");
            RaisePropertyChanged("ScoreEquipe2");
        }

        public SaisieRencontreViewModel(Rencontre rencontre, Coach.eTypeRosterJoue typRosterJoue)
        {
            _rencontre = rencontre;

            _duel1VM = new SaisieDuelViewModel(_rencontre.Duel1, OnScoreModifie, typRosterJoue);
            _duel2VM = new SaisieDuelViewModel(_rencontre.Duel2, OnScoreModifie, typRosterJoue);
            _duel3VM = new SaisieDuelViewModel(_rencontre.Duel3, OnScoreModifie, typRosterJoue);
        }

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
