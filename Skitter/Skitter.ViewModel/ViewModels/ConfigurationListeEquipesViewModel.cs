/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 10/05/2014
 * Time: 15:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;
using Skitter.Object;
using System.Windows;

namespace Skitter.ViewModel.ViewModels
{
	/// <summary>
	/// Génération de la liste des équipes
	/// </summary>
	public class ConfigurationListeEquipesViewModel : INotifyPropertyChanged
    {
        #region Variables
        Tournoi _tournoi;
        EquipeViewModel _equipeSelectionnee;
        List<EquipeViewModel> _lsEquipes;
        #endregion

        public ConfigurationListeEquipesViewModel()
		{
            _tournoi = Tournoi.GetInstance();
		}
		
		#region Liste des équipes
        public List<EquipeViewModel> ListeEquipes
        {
            get
            {
                if (_lsEquipes == null)
                    _lsEquipes = _tournoi.Equipes.Select(e => new EquipeViewModel(e)).ToList();

                return _lsEquipes.OrderBy(vm => vm.NomEquipe).ToList();
            }
        }

        public void AjouterNouvelleEquipe()
        {
            Equipe equipe = new Equipe() { NomEquipe = "Nouvelle équipe" };
            _tournoi.Equipes.Add(equipe);
            _lsEquipes = null;
            RaisePropertyChanged("ListeEquipes");
            EquipeSelectionnee = ListeEquipes.FirstOrDefault(vm => vm.IdEquipe == equipe.IdEquipe);
        }

        public void SupprimerEquipe()
        {
            if (EquipeSelectionnee == null)
                return;

            _tournoi.Equipes.RemoveAll(e => e.IdEquipe == EquipeSelectionnee.IdEquipe);
            _lsEquipes = null;
            RaisePropertyChanged("ListeEquipes");
        }

        public bool IsSuppressionPossible
        {
            get 
            { 
                return (EquipeSelectionnee != null)
                    && (_tournoi.PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration); 
            }
        }

        public bool IsAjoutPossible
        {
            get { return _tournoi.PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration; }
        }

        public Visibility AvertissementModificationVisibility
        {
            get
            {
                return (Tournoi.GetInstance().PhaseEnCours == Tournoi.eTypePhaseTournoi.Configuration) 
                    ? Visibility.Collapsed : Visibility.Visible;
            }
        }
		#endregion

        #region Equipe sélectionnée
        public EquipeViewModel EquipeSelectionnee
        {
            get { return _equipeSelectionnee; }
            set
            {
                _equipeSelectionnee = value;
                RaisePropertyChanged("EquipeSelectionnee");
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
		#endregion
	}
}
