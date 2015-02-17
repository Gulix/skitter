using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Skitter.ViewModel.ViewModels.Configuration;

namespace Skitter.Wpf.Configuration
{
    /// <summary>
    /// Configuration générale du tournoi
    /// </summary>
    public partial class PageTournoi : Grid, IPage
    {
        ConfigurationTournoiViewModel _viewModel;

        public PageTournoi()
        {
            PageManager.AjouterPage(this);

            InitializeComponent();

            ReinitialiserPage();
            btnValiderConfigurationGenerale.Click += btnValiderConfigurationGenerale_Click;
        }

        void btnValiderConfigurationGenerale_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Valider la configuration générale ?\nIl ne sera plus possible de modifier ces éléments.",
                "Configuration générale",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                string sErreurs = _viewModel.ValiderConfigGenerale();
                if (string.IsNullOrEmpty(sErreurs))
                {
                    PageManager.ReinitialiserToutesPages(true);
                }
            }
        }

        #region IPage Members

        public void ReinitialiserPage()
        {
            _viewModel = new ConfigurationTournoiViewModel();
            DataContext = _viewModel;
        }

        #endregion
    }
}
