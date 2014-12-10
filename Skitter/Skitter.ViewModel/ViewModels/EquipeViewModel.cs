/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 14/07/2014
 * Time: 19:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Skitter.Object;
using System.Windows;

namespace Skitter.ViewModel.ViewModels
{
    /// <summary>
    /// Description of EquipeViewModel.
    /// </summary>
    public class EquipeViewModel : INotifyPropertyChanged
    {
        #region Variables
        Equipe _equipe;
        
        CoachViewModel _capitaineVM;
        CoachViewModel _equipier1VM;
        CoachViewModel _equipier2VM;
        #endregion
        
        #region Accesseurs
        public string NomEquipe
        {
            get {return _equipe.NomEquipe;}
            set {
                _equipe.NomEquipe = value;
                RaisePropertyChanged("NomEquipe");
                RaisePropertyChanged("Erreur");
            }
        }

        public string HymneEquipe
        {
            get { return _equipe.HymneEquipe; }
            set
            {
                _equipe.HymneEquipe = value;
                RaisePropertyChanged("HymneEquipe");
            }
        }
        
        public CoachViewModel Capitaine
        {
            get {
                if (_capitaineVM == null)
                    _capitaineVM = new CoachViewModel(_equipe.Capitaine, OnRosterModifie);
                return _capitaineVM;
            }
        }
        
        public CoachViewModel Equipier1
        {
            get {
                if (_equipier1VM == null)
                    _equipier1VM = new CoachViewModel(_equipe.Equipier1, OnRosterModifie);
                return _equipier1VM;
            }
        }
        
        public CoachViewModel Equipier2
        {
            get {
                if (_equipier2VM == null)
                    _equipier2VM = new CoachViewModel(_equipe.Equipier2, OnRosterModifie);
                return _equipier2VM;
            }
        }

        public int ValeurEquipe
        {
            get
            {
                return Capitaine.ValeurRoster + Equipier1.ValeurRoster + Equipier2.ValeurRoster;
            }
        }

        public int IdEquipe
        {
            get { return _equipe.IdEquipe; }
        }

        public string Erreur
        {
            get
            {
                if (string.IsNullOrEmpty(NomEquipe))
                    return "Un nom doit être donné à l'équipe.";
                if (Capitaine.ValeurRoster == 0)
                    return "Le roster du capitaine doit être sélectionné.";
                if (Equipier1.ValeurRoster == 0)
                    return "Le roster de l'équipier 1 doit être sélectionné.";
                if (Equipier2.ValeurRoster == 0)
                    return "Le roster de l'équipier 2 doit être sélectionné.";

                if ((Capitaine.Roster.IdRoster == Equipier1.Roster.IdRoster)
                    || (Capitaine.Roster.IdRoster == Equipier2.Roster.IdRoster)
                    || (Equipier1.Roster.IdRoster == Equipier2.Roster.IdRoster))
                    return "Les rosters de chaque membre de l'équipe doivent être différents.";


                return string.Empty;
            }
        }

        public Visibility ErreurVisibility
        {
            get
            {
                return string.IsNullOrEmpty(Erreur) ? Visibility.Hidden : Visibility.Visible;
            }
        }
        #endregion

        #region Liste des hymnes possibles
        public List<string> ListeHymnes
        {
            get { return HymneViewModel.GetListeHymnesDisponibles(); }
        }
        #endregion

        public EquipeViewModel(Equipe equipe)
        {
            _equipe = equipe;
        }

        public void OnRosterModifie()
        {
            RaisePropertyChanged("ValeurEquipe");
            RaisePropertyChanged("Erreur");
            RaisePropertyChanged("ErreurVisibility");
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
