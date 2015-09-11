using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Configuration
{
    /// <summary>
    /// ViewModel class for the configuration of the Rosters' list
    /// </summary>
    public class RostersListViewModel : INotifyPropertyChanged
    {
        #region Variables
        Tournoi _tournoi;
        List<RosterViewModel> _lsRostersVM;
        #endregion

        public RostersListViewModel()
        {
            _tournoi = Tournoi.GetInstance();
        }

        #region List of current rosters
        public List<RosterViewModel> RostersList
        {
            get
            {
                if (_lsRostersVM == null)
                    _lsRostersVM = _tournoi.Configuration.Rosters.Select(r => new RosterViewModel(r)).OrderBy(rVm => rVm.NomRoster).ToList();

                return _lsRostersVM;
            }
        }

        public void SetDefaultList()
        {
            _tournoi.Configuration.Rosters.Clear();
            _tournoi.Configuration.AddNewRoster("Elfes Sylvains", "Wood Elves", 1);
            _tournoi.Configuration.AddNewRoster("Morts-vivants", "Undead", 1);
            _tournoi.Configuration.AddNewRoster("Nains du chaos", "Chaos Dwarves", 1);
            _tournoi.Configuration.AddNewRoster("Skavens", "Skaven", 1);
            _tournoi.Configuration.AddNewRoster("Hommes-lézards", "Lizardmen", 1);
            _tournoi.Configuration.AddNewRoster("Nécromantiques", "Necromantic", 2);
            _tournoi.Configuration.AddNewRoster("Amazones", "Amazons", 2);
            _tournoi.Configuration.AddNewRoster("Nordiques", "Norse", 2);
            _tournoi.Configuration.AddNewRoster("Elfes noirs", "Dark Elves", 2);
            _tournoi.Configuration.AddNewRoster("Nains", "Dwarves", 2);
            _tournoi.Configuration.AddNewRoster("Khemri", "Khemri", 3);
            _tournoi.Configuration.AddNewRoster("Chaos", "Chaos", 3);
            _tournoi.Configuration.AddNewRoster("Elfes pros", "Elves", 3);
            _tournoi.Configuration.AddNewRoster("Humains", "Humans", 3);
            _tournoi.Configuration.AddNewRoster("Orques", "Orc", 3);
            _tournoi.Configuration.AddNewRoster("Khorne", "", 4);
            _tournoi.Configuration.AddNewRoster("Slanns", "Slann", 4);
            _tournoi.Configuration.AddNewRoster("Nurgle", "Nurgle's Rotters", 4);
            _tournoi.Configuration.AddNewRoster("Hauts-Elfes", "High Elves", 4);
            _tournoi.Configuration.AddNewRoster("Pacte du Chaos", "Chaos Pact", 4);
            _tournoi.Configuration.AddNewRoster("Halflings", "Halflings", 5);
            _tournoi.Configuration.AddNewRoster("Gobelins", "Goblins", 5);
            _tournoi.Configuration.AddNewRoster("Ogres", "Ogres", 5);
            _tournoi.Configuration.AddNewRoster("Vampires", "Vampires", 5);
            _tournoi.Configuration.AddNewRoster("Bas-Fonds", "Underworld", 5);
            
            _lsRostersVM = null;
            RaisePropertyChanged("RostersList");
        }

        public void ClearList()
        {
            _tournoi.Configuration.Rosters.Clear();
            
            _lsRostersVM = null;
            RaisePropertyChanged("RostersList");
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string sPropName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sPropName));
        }

        #endregion
    }
}
