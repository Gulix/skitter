using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Skitter.Object;
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.ViewModel.ViewModels.Configuration
{
    public class ConfigurationTournoiViewModel : INotifyPropertyChanged
    {
        #region Constructeur
        public ConfigurationTournoiViewModel()
        {

        }
        #endregion

        #region Accesseurs sur l'accessibilité des éléments
        public bool IsConfigurationGeneraleEnabled
        {
            get { return Tournoi.TypePhaseTournoi == Tournoi.eTypePhaseTournoi.ConfigurationTournoi; }
        }

        public Visibility NbCoachesVisibility
        {
            get
            {
                return TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Equipe ?
                    Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region Accesseurs sur les éléments de la configuration générale (à fixer avant d'aller plus loin)
        public ConfigurationTournoi.eTypeParticipantTournoi TypeParticipantTournoi
        {
            get { return Tournoi.Configuration.TypeParticipantTournoi; }
            set 
            { 
                Tournoi.Configuration.TypeParticipantTournoi = value; 
                RaisePropertyChanged("TypeParticipantTournoi");
                RaisePropertyChanged("NbCoachesVisibility");
            }
        }

        public List<ConfigurationTournoi.eTypeParticipantTournoi> ListeTypesParticipant
        {
            get { return Enum.GetValues(typeof(ConfigurationTournoi.eTypeParticipantTournoi)).Cast<ConfigurationTournoi.eTypeParticipantTournoi>().ToList(); }
        }

        public string NbCoachesParEquipe
        {
            get { return Tournoi.Configuration.NbCoachesParEquipe.GetValueOrDefault(0).ToString(); }
            set 
            {
                Tournoi.Configuration.NbCoachesParEquipe = Conv.IntStringVersInt(Tournoi.Configuration.NbCoachesParEquipe.GetValueOrDefault(0), value);
                RaisePropertyChanged("NbCoachesParEquipe"); 
            }
        }

        public string NbRondes
        {
            get { return Tournoi.Configuration.NbRondes.ToString(); }
            set 
            {
                Tournoi.Configuration.NbRondes = Conv.IntStringVersInt(Tournoi.Configuration.NbRondes, value);
                RaisePropertyChanged("NbRondes"); 
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string sPropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sPropertyName));
        }

        #endregion

        public static object TypeParticipantToString(object value)
        {
            if ((value == null) || !(value is ConfigurationTournoi.eTypeParticipantTournoi))
                return string.Empty;

            ConfigurationTournoi.eTypeParticipantTournoi typParticipant = (ConfigurationTournoi.eTypeParticipantTournoi)value;
            switch(typParticipant)
            {
                case ConfigurationTournoi.eTypeParticipantTournoi.Equipe:
                    return "Tournoi par équipe";
                case ConfigurationTournoi.eTypeParticipantTournoi.Solo:
                    return "Tournoi individuel";
            }

            return string.Empty;
        }

        public string ValiderConfigGenerale()
        {
            List<string> lsErreurs = new List<string>();
            if (Tournoi.Configuration.NbRondes <= 0)
                lsErreurs.Add("Un nombre de rondes positif doit être spécifié.");
            if ((Tournoi.Configuration.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Equipe)
                && (Tournoi.Configuration.NbCoachesParEquipe.GetValueOrDefault(0) <= 1))
                lsErreurs.Add("Le nombre de coaches par équipe est incorrect");

            if (!lsErreurs.Any())
            {
                Tournoi.TypePhaseTournoi = Tournoi.eTypePhaseTournoi.ConfigurationParticipants;
                return string.Empty;
            }

            return Conv.ListToString(lsErreurs, "\n");
        }
    }
}
