/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 14/07/2014
 * Time: 19:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.ComponentModel;
using Skitter.Object;
using System.Collections.Generic;

namespace Skitter.ViewModel.ViewModels
{
    /// <summary>
    /// Description of CoachViewModel.
    /// </summary>
    public class CoachViewModel : INotifyPropertyChanged
    {
        #region Variables
        Coach _coach;

        Action _onRosterModifie;
        #endregion
        
        #region Accesseurs
        public string NomCoach
        {
            get {return _coach.NomCoach;}
            set 
            {
                _coach.NomCoach = value;
                RaisePropertyChanged("NomCoach");
            }
        }

        public int IdCoach
        {
            get { return _coach.IdCoach; }
        }
        
        public string NumeroNAF
        {
            get {return _coach.NumeroNAF.HasValue ? _coach.NumeroNAF.ToString() : string.Empty;}
            set 
            {
                int iValue = -1;
                if (int.TryParse(value, out iValue))
                    _coach.NumeroNAF = iValue;
                else
                {
                    _coach.NumeroNAF = null;
                    _coach.PseudoNAF = string.Empty;
                }
                RaisePropertyChanged("NumeroNAF");
            }
        }

        public string LienProfilNAF
        {
            get
            {
                if (string.IsNullOrEmpty(NumeroNAF))
                    return string.Empty;

                return "http://member.thenaf.net/index.php?module=NAF&type=coachpage&coach=" + NumeroNAF;
            }
        }

        public string PseudoNAF
        {
            get { return _coach.PseudoNAF; }
            set
            {
                _coach.PseudoNAF = value;
                RaisePropertyChanged("PseudoNAF");
            }
        }

        public string NomRoster
        {
            get { return _coach.NomRoster; } // TODO : nom du roster selon la ronde
        }

        public string CoachAvecRosterJoue
        {
            get { return string.Format("{0} ({1})", NomCoach, NomRoster); } // TODO : nom du roster selon la ronde
        }
        
        public RosterViewModel Roster
        {
            get { 
                return RosterViewModel.GetListeComplete()
                    .FirstOrDefault(rvm => rvm.IdRoster == _coach.IdRoster);
            }
            set {
                if (value != null)
                    _coach.IdRoster = value.IdRoster;
                RaisePropertyChanged("Roster");
                OnRosterModifie();
            }
        }

        public int ValeurRoster
        {
            get { return (Roster != null) ? Roster.ValeurRoster : 0; }
        }
        #endregion

        #region Liste des rosters
        public List<RosterViewModel> ListeRosters
        {
            get
            {
                return RosterViewModel.GetListeComplete();
            }
        }
        #endregion

        public CoachViewModel(Coach coach)
            : this(coach, null)
        {

        }

        public CoachViewModel(Coach coach, Action onRosterModifie)
        {
            _coach = coach;
            _onRosterModifie = onRosterModifie;
        }
        
        private void OnRosterModifie()
        {
            RaisePropertyChanged("ValeurRoster");
            if (_onRosterModifie != null)
                _onRosterModifie();
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
