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
using Microsoft.Win32;
using Skitter.ViewModel.ViewModels;

namespace Skitter.Wpf.Systeme
{
    /// <summary>
    /// Interaction logic for PageSauvegarde.xaml
    /// </summary>
    public partial class PageSauvegarde : UserControl, IPage
    {
        SauvegardeViewModel _viewModel;

        public PageSauvegarde()
        {
            PageManager.AjouterPage(this);

            InitializeComponent();

            ReinitialiserPage();

            btnSauvegarder.Click += btnSauvegarder_Click;
            btnParcourir.Click += btnParcourir_Click;
        }

        void btnParcourir_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "Fichier xml (*.xml)|*.xml";
            saveFileDialog.Filter = "Fichier xml (*.xml)|*.xml";
            saveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(_viewModel.FichierSauvegarde);
            saveFileDialog.FileName = System.IO.Path.GetFileName(_viewModel.FichierSauvegarde);
            bool? bResult = saveFileDialog.ShowDialog();
            if (!bResult.HasValue || !bResult.Value)
                return;

            _viewModel.FichierSauvegarde = saveFileDialog.FileName;
        }

        void btnSauvegarder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("Confirmez-vous la sauvegarde ?", "Confirmation", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;


            try
            {
                _viewModel.SauvegarderDonnees();
                MessageBox.Show("Sauvegarde effectuée.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region IPage Members

        public void ReinitialiserPage()
        {
            _viewModel = SauvegardeViewModel.GetInstance();
            DataContext = null;
            DataContext = _viewModel;
        }

        #endregion
    }
}
