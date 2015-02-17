using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Skitter.Object;
using System.Windows;

namespace Skitter.ViewModel.ViewModels
{
	/// <summary>
	/// Génération de la liste des participants au tournoi
	/// </summary>
	public class ConfigurationListeParticipantsViewModel : INotifyPropertyChanged
    {
        #region Variables
        ParticipantViewModel _participantSelectionne;
        List<ParticipantViewModel> _lsParticipants;
        #endregion

        public ConfigurationListeParticipantsViewModel()
		{

		}
		
		#region Liste des participants au tournoi
        #region Accesseurs utilisés dans le Xaml
        public List<ParticipantViewModel> ListeParticipants
        {
            get
            {
                if (_lsParticipants == null)
                    _lsParticipants = Tournoi.ListeParticipants.Select(p => new ParticipantViewModel(p)).ToList();

                return _lsParticipants.OrderBy(vm => vm.NomParticipant).ToList();
            }
        }

        public string NbParticipants
        {
            get
            {
                return string.Format("{0} participant{1} enregistré{1} pour le tournoi",
                    Tournoi.ListeParticipants.Count,
                    (Tournoi.ListeParticipants.Count > 1) ? "s" : string.Empty);
            }
        }
        #endregion

        public void AjouterNouveauParticipant()
        {
            int idParticipant = Tournoi.GenererNouveauParticipant();
            RefreshListeParticipants();
            ParticipantSelectionne = ListeParticipants.FirstOrDefault(vm => vm.IdParticipant == idParticipant);
        }

        public void SupprimerParticipantSelectionne()
        {
            if (ParticipantSelectionne == null)
                return;

            Tournoi.SupprimerParticipant(ParticipantSelectionne.IdParticipant);
            ParticipantSelectionne = null;
            RefreshListeParticipants();
        }

        public bool IsSuppressionPossible
        {
            get 
            {
                return (ParticipantSelectionne != null)
                    && (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.ConfigurationParticipants); 
            }
        }

        public bool IsAjoutPossible
        {
            get { return Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.ConfigurationParticipants; }
        }

        public Visibility AvertissementModificationVisibility
        {
            get
            {
                return (Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.ConfigurationParticipants) 
                    ? Visibility.Collapsed : Visibility.Visible;
            }
        }
		#endregion

        #region Equipe sélectionnée
        public ParticipantViewModel ParticipantSelectionne
        {
            get { return _participantSelectionne; }
            set
            {
                _participantSelectionne = value;
                RaisePropertyChanged("ParticipantSelectionne");
                RaisePropertyChanged("IsSuppressionPossible");
            }
        }
        #endregion

        #region INotifyPropertyChanged implémentation
        public event PropertyChangedEventHandler PropertyChanged;
		
		protected void RaisePropertyChanged(string sPropName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(sPropName));
		}

        void RefreshListeParticipants()
        {
            _lsParticipants = null;
            RaisePropertyChanged("ListeParticipants");
            RaisePropertyChanged("NbParticipants");
        }
		#endregion
	}
}
