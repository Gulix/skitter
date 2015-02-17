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
using Skitter.Object.Interfaces;

namespace Skitter.ViewModel.ViewModels
{
    /// <summary>
    /// ViewModel permettant d'agir sur un IParticipant (à savoir un coach ou une équipe)
    /// </summary>
    public class ParticipantViewModel : INotifyPropertyChanged
    {
        #region Variables
        IParticipant _participant;

        List<CoachViewModel> _lsCoachesVM;
        #endregion
        
        #region Accesseurs
        public string NomParticipant
        {
            get {return _participant.NomParticipant;}
            set {
                _participant.NomParticipant = value;
                RaisePropertyChanged("NomParticipant");
                RaisePropertyChanged("Erreur");
                _lsCoachesVM.ForEach(vm => vm.RafraichirNom());
            }
        }

        // TODO : passer l'hymne au niveau des participants, avec un stockage à part
        //public string HymneEquipe
        //{
        //    get { return _equipe.HymneEquipe; }
        //    set
        //    {
        //        _equipe.HymneEquipe = value;
        //        RaisePropertyChanged("HymneEquipe");
        //    }
        //}

        public int ValeurRosters
        {
            get { return _participant.ListeCoaches.Sum(c => c.ValeurRoster); }
        }

        public int IdParticipant
        {
            get { return _participant.IdParticipant; }
        }

        public string Erreur
        {
            get
            {
                // TODO - Gérer les erreurs au niveau des IParticipants ?
                //if (string.IsNullOrEmpty(NomEquipe))
                //    return "Un nom doit être donné à l'équipe.";
                //if (Capitaine.ValeurRoster == 0)
                //    return "Le roster du capitaine doit être sélectionné.";
                //if (Equipier1.ValeurRoster == 0)
                //    return "Le roster de l'équipier 1 doit être sélectionné.";
                //if (Equipier2.ValeurRoster == 0)
                //    return "Le roster de l'équipier 2 doit être sélectionné.";

                //if ((Capitaine.Roster.IdRoster == Equipier1.Roster.IdRoster)
                //    || (Capitaine.Roster.IdRoster == Equipier2.Roster.IdRoster)
                //    || (Equipier1.Roster.IdRoster == Equipier2.Roster.IdRoster))
                //    return "Les rosters de chaque membre de l'équipe doivent être différents.";


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

        #region VM des coaches
        private void InitCoachesViewModel()
        {
            _lsCoachesVM = new List<CoachViewModel>();

            foreach(Coach c in _participant.ListeCoaches)
                this._lsCoachesVM.Add(new CoachViewModel(c, OnRosterModifie, OnNomModifie));
        }

        public List<CoachViewModel> CoachesViewModels
        {
            get
            {
                if (_lsCoachesVM == null)
                    InitCoachesViewModel();
                return _lsCoachesVM;
            }
        }
        #endregion

        #region Liste des hymnes possibles
        public List<string> ListeHymnes
        {
            get { return HymneViewModel.GetListeHymnesDisponibles(); }
        }
        #endregion

        public ParticipantViewModel(IParticipant participant)
        {
            _participant = participant;
        }

        public void OnRosterModifie()
        {
            RaisePropertyChanged("ValeurEquipe");
            RaisePropertyChanged("Erreur");
            RaisePropertyChanged("ErreurVisibility");
        }

        public void OnNomModifie()
        {
            RaisePropertyChanged("NomParticipant");
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
