/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 10/05/2014
 * Time: 15:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Skitter.ViewModel.ViewModels;

namespace Skitter.Wpf.Configuration
{
	/// <summary>
	/// Interaction logic for PageListeEquipes.xaml
	/// </summary>
	public partial class PageListeEquipes : Grid, IPage
	{
        ConfigurationListeEquipesViewModel _viewModel;

		public PageListeEquipes()
		{
            PageManager.AjouterPage(this);

			InitializeComponent();

            ReinitialiserPage();

            btnAjouterEquipe.Click += btnAjouterEquipe_Click;
            btnSupprimerEquipe.Click += btnSupprimerEquipe_Click;
		}

        private void btnAjouterEquipe_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AjouterNouvelleEquipe();
            PageManager.ReinitialiserToutesPages(this);
        }

        private void btnSupprimerEquipe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("Confirmez-vous la suppression de l'équipe sélectionnée ?", "Confirmation", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            
            _viewModel.SupprimerEquipe();
            PageManager.ReinitialiserToutesPages(this);
        }

        #region IPage Members

        public void ReinitialiserPage()
        {
            _viewModel = new ConfigurationListeEquipesViewModel();
            DataContext = _viewModel;
        }

        #endregion
    }
}