/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 14/07/2014
 * Time: 19:22
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
    /// Description of RosterViewModel.
    /// </summary>
    public class RosterViewModel : INotifyPropertyChanged
    {
        #region Variables
        Roster _roster;
        #endregion
        
        #region Accesseurs
        public string NomRoster
        {
            get {return _roster.NomRoster;}
            set { _roster.NomRoster = value; RaisePropertyChanged("NomRoster"); }
        }
        
        public int IdRoster
        {
            get {return _roster.IdRoster;}
        }
        
        public int ValeurRoster
        {
            get {return _roster.ValeurRoster;}
        }

        public string ValeurRosterChaine
        {
            get { return _roster.ValeurRoster.ToString(); }
            set
            {
                int iValeurRoster = -1;
                if (int.TryParse(value, out iValeurRoster))
                {
                    _roster.ValeurRoster = iValeurRoster;
                }
                RaisePropertyChanged("ValeurRosterChaine");
                RaisePropertyChanged("ValeurRoster");
            }
        }

        public bool IsRosterNAF
        {
            get { return _roster.IsNaf; }
            set { _roster.IsNaf = value; RaisePropertyChanged("IsRosterNAF"); }
        }

        public string NomRosterNaf
        {
            get { return _roster.NomNaf; }
            set { _roster.NomNaf = value; RaisePropertyChanged("NomNaf"); }
        }
        #endregion
        
        public RosterViewModel(Roster roster)
        {
            _roster = roster;
        }
        
        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string sPropertyChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sPropertyChanged));
        }
        
        #endregion
        
        #region Générateur de liste
        static List<RosterViewModel> _S_lsRosters;

        public static List<RosterViewModel> GetListeComplete()
        {
            if (_S_lsRosters == null)
                _S_lsRosters = 
                    Tournoi.GetInstance().Configuration.Rosters
                    .Select(r => new RosterViewModel(r))
                    .OrderBy(r => r.NomRoster)
                    .ToList();
            
            return _S_lsRosters;
        }
        #endregion
        
    }
}
