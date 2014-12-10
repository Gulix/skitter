using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Skitter.ViewModel.ViewModels;

namespace Skitter.Wpf.Systeme
{
    /// <summary>
    /// Interaction logic for PageInitialisation.xaml
    /// </summary>
    public partial class PageInitialisation : System.Windows.Controls.UserControl
    {
        public PageInitialisation()
        {
            InitializeComponent();

            btnNouveauTournoi.Click += btnNouveauTournoi_Click;
            btnChargementTournoi.Click += btnChargementTournoi_Click;
        }

        void btnNouveauTournoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                System.Windows.MessageBox.Show("Confirmez-vous la réinitialisation de l'ensemble du tournoi ?", "Confirmation", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;

            InitialisationViewModel.InitialiserNouveauTournoi();

            PageManager.ReinitialiserToutesPages();
        }

        void btnChargementTournoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                System.Windows.MessageBox.Show("Confirmez-vous le chargement du tournoi depuis un fichier externe ?", "Confirmation", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "Fichier xml (*.xml)|*.xml";
            openFileDialog.Filter = "Fichier xml (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.FileName = "dragonbowl.xml";
            DialogResult dialResult = openFileDialog.ShowDialog();
            if (dialResult != DialogResult.OK)
                return;

            try
            {
                InitialisationViewModel.ChargerTournoiExistant(openFileDialog.FileName);
                PageManager.ReinitialiserToutesPages();
                System.Windows.MessageBox.Show("Chargement terminé !");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
