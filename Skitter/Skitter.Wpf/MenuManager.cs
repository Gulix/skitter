using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Presentation;
using Skitter.ViewModel.ViewModels;

namespace Skitter.Wpf
{
    public class MenuManager : INotifyPropertyChanged
    {
        List<LinkGroup> _lsMenusPrincipaux;

        const string _MENU_SYSTEME = "Système";
        const string _MENU_SYSTEME_INITIALISATION = "Initialisation";
        const string _MENU_SYSTEME_SAUVEGARDE = "Sauvegarde";
        const string _MENU_SYSTEME_APROPOS = "A Propos";
        const string _MENU_CONFIGURATION = "Configuration";
        const string _MENU_CONFIGURATION_TOURNOI = "Tournoi";
        const string _MENU_CONFIGURATION_PARTICIPANTS = "Participants";
        
        public MenuManager()
        {
            _lsMenusPrincipaux = new List<LinkGroup>();
        
            // On construit l'ensemble des menus
            // L'accesseur s'occupera de les rendre visibles, ou non

            // Système
            LinkGroup lngSysteme = new LinkGroup() { DisplayName = _MENU_SYSTEME };
            lngSysteme.Links.Add(new Link() { DisplayName = _MENU_SYSTEME_INITIALISATION, 
                                              Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Systeme/PageInitialisation.xaml"), UriKind.Relative) });
            lngSysteme.Links.Add(new Link() { DisplayName = _MENU_SYSTEME_SAUVEGARDE, 
                                              Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Systeme/PageSauvegarde.xaml"), UriKind.Relative) });
            lngSysteme.Links.Add(new Link() { DisplayName = _MENU_SYSTEME_APROPOS, 
                                              Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Systeme/PageApropos.xaml"), UriKind.Relative) });
            _lsMenusPrincipaux.Add(lngSysteme);

            // Configuration
            LinkGroup lngConfiguration = new LinkGroup() { DisplayName = _MENU_CONFIGURATION };
            lngConfiguration.Links.Add(new Link() { DisplayName = _MENU_CONFIGURATION_TOURNOI,
                                                    Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Configuration/PageTournoi.xaml"), UriKind.Relative) });
            lngConfiguration.Links.Add(new Link() { DisplayName = _MENU_CONFIGURATION_PARTICIPANTS, 
                                                    Source = new Uri(string.Format(CultureInfo.InvariantCulture, "/Configuration/PageListeParticipants.xaml"), UriKind.Relative) });
            _lsMenusPrincipaux.Add(lngConfiguration);
        }

        public LinkGroupCollection ListeMenus
        {
            get
            {
                LinkGroupCollection lngCollection = new LinkGroupCollection();
                lngCollection.Add(_lsMenusPrincipaux[0]);
                
                // Configuration
                LinkGroup lngConfig = new LinkGroup() { DisplayName = _MENU_CONFIGURATION };
                lngConfig.Links.Add(_lsMenusPrincipaux[1].Links[0]);
                if (MenuManagerViewModel.IsMenuConfigurationParticipantsVisible)
                    lngConfig.Links.Add(_lsMenusPrincipaux[1].Links[1]);
                if (lngConfig.Links.Any())
                    lngCollection.Add(lngConfig);

                return lngCollection;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string sProperty)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sProperty));
        }

        #endregion
    }
}
