using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
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

        #region Selection
        RosterViewModel _selectedVM;

        public RosterViewModel SelectedRoster
        {
            get { return _selectedVM; }
            set { _selectedVM = value; RaisePropertyChanged("SelectedRoster"); }
        }
        #endregion

        #region Actions
        public bool IsDeleteEnabled
        {
            get 
            {
                return (_tournoi.PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration)
                    && !_tournoi.Equipes.Any();
            }
        }

        public bool IsAddEnabled
        {
            get { return _tournoi.PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration; }
        }

        public void AddNewRoster()
        {
            Roster roster = _tournoi.Configuration.AddNewRoster();
            RefreshListeRosters();

            SelectedRoster = _lsRostersVM.FirstOrDefault(r => r.IdRoster == roster.IdRoster);
        }

        public void DeleteSelectedRoster()
        {
            if (SelectedRoster != null)
            {
                MessageBoxResult result = MessageBox.Show("Confirmez-vous la suppression de ce roster ?", "Suppression", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _tournoi.Configuration.Rosters.RemoveAll(r => r.IdRoster == SelectedRoster.IdRoster);
                    RefreshListeRosters();
                }
            }
        }

        #endregion

        #region List of current rosters
        private void RefreshListeRosters()
        {
            _lsRostersVM = null;
            RaisePropertyChanged("RostersList");
        }

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

            RefreshListeRosters();
        }

        public void ClearList()
        {
            _tournoi.Configuration.Rosters.Clear();

            RefreshListeRosters();
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
