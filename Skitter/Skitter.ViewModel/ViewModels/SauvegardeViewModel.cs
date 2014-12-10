using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels
{
    public class SauvegardeViewModel : INotifyPropertyChanged
    {
        #region Instance statique
        static SauvegardeViewModel _instance;
        public static SauvegardeViewModel GetInstance()
        {
            if (_instance == null)
                _instance = new SauvegardeViewModel();
            return _instance;
        }
        #endregion

        #region Variables
        DateTime? _dtLast;
        #endregion

        #region Accesseurs
        public string DerniereSauvegarde
        {
            get
            {
                if (_dtLast.HasValue)
                    return string.Format("Dernière sauvegarde le {0:00}/{1:00}/{2:0000} à {3:00}:{4:00}:{5:00}",
                        _dtLast.Value.Day, _dtLast.Value.Month, _dtLast.Value.Year, _dtLast.Value.Hour, _dtLast.Value.Minute, _dtLast.Value.Second);

                return "Pas encore sauvegardé.";
            }
        }

        public string FichierSauvegarde
        {
            get { return Tournoi.GetInstance().NomFichier; }
            set { Tournoi.GetInstance().NomFichier = value; RaisePropertyChanged("FichierSauvegarde"); }
        }
        #endregion

        private SauvegardeViewModel()
        {
            _dtLast = null;
        }

        public void SauvegarderDonnees()
        {
            if (string.IsNullOrEmpty(Tournoi.GetInstance().NomFichier))
                return;

            try
            {
                Tournoi.GetInstance().EnregistrerXml(Tournoi.GetInstance().NomFichier);
                _dtLast = DateTime.Now;
                RaisePropertyChanged("DerniereSauvegarde");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
