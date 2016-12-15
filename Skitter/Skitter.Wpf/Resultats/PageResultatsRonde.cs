using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Skitter.ViewModel.Fonctionnel;
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.Wpf.Resultats
{
    public abstract class PageResultatsRonde : UserControl, IPage
    {
        bool _bLoaded = false;

        static PageResultatsRonde()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageResultatsRonde), new FrameworkPropertyMetadata(typeof(PageResultatsRonde)));
        }

        ResultatsRondeViewModel _viewModel;

        protected abstract ResultatsRondeViewModel GetViewModel();

        public PageResultatsRonde()
        {
            PageManager.AjouterPage(this);

            ReinitialiserPage();

            Loaded += PageResultatsRonde_Loaded;
        }

        void PageResultatsRonde_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bLoaded)
                return;
            _bLoaded = true;

            Button btnTriAttaque = (Button)Template.FindName("PART_btnTriAttaque", this);
            btnTriAttaque.Click += btnTriAttaque_Click;
            Button btnTriBashlord = (Button)Template.FindName("PART_btnTriBashlord", this);
            btnTriBashlord.Click += btnTriBashlord_Click;
            Button btnTriDefense = (Button)Template.FindName("PART_btnTriDefense", this);
            btnTriDefense.Click += btnTriDefense_Click;
            Button btnTriGeneral = (Button)Template.FindName("PART_btnTriGeneral", this);
            btnTriGeneral.Click += btnTriGeneral_Click;
            Button btnTriPaillasson = (Button)Template.FindName("PART_btnTriPaillasson", this);
            btnTriPaillasson.Click += btnTriPaillasson_Click;
            Button btnTriPassoire = (Button)Template.FindName("PART_btnTriPassoire", this);
            btnTriPassoire.Click += btnTriPassoire_Click;
            Button btnTriVicieux = (Button)Template.FindName("PART_btnTriVicieux", this);
            btnTriVicieux.Click += btnTriVicieux_Click;
            Button btnTriPoingsEnMousse = (Button)Template.FindName("PART_btnTriPoingsEnMousse", this);
            btnTriPoingsEnMousse.Click += btnTriPoingsEnMousse_Click;
            Button btnVersExcel = (Button)Template.FindName("PART_btnVersExcel", this);
            btnVersExcel.Click += btnVersExcel_Click;
            Button btnExport = (Button)Template.FindName("PART_btnExport", this);
            btnExport.Click += btnExport_Click;
            Button btnIndividuel = (Button)Template.FindName("PART_btnIndividuel", this);
            btnIndividuel.Click += btnIndividuel_Click;
            Button btnPalmares = (Button)Template.FindName("PART_btnPalmares", this);
            btnPalmares.Click += btnPalmares_Click;
        }

        #region Tris
        void btnTriVicieux_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierVicieux();
        }

        void btnTriPassoire_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierPassoire();
        }

        void btnTriPaillasson_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierPaillasson();
        }

        void btnTriGeneral_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierGeneral();
        }

        void btnTriDefense_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierDefense();
        }

        void btnTriBashlord_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierBashlord();
        }

        void btnTriAttaque_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierAttaque();
        }

        void btnTriPoingsEnMousse_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.TrierPoingsEnMousse();
        }
        #endregion

        #region Copier vers Excel
        void btnVersExcel_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.CopierVersExcel();
                MessageBox.Show("Les résultats ont été placés dans le presse-papiers.");
            }
        }

        void btnIndividuel_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.CopierIndividuelVersExcel();
                MessageBox.Show("Les résultats ont été placés dans le presse-papiers.");
            }
        }
        #endregion

        #region Exports Nustache
        void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // 1 - Choix d'un fichier de template
            MessageBox.Show("Sélectionnez le fichier 'Template' désiré.");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FileAndDirectory.ExeDirectory;
            bool? bResult = openFileDialog.ShowDialog();
            if (!bResult.HasValue || !bResult.Value)
                return;
            string sFichierSource = openFileDialog.FileName;

            // 2 - Choix d'un fichier de destination
            MessageBox.Show("Sélectionnez le fichier dans lequel générer les données.");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = FileAndDirectory.ExeDirectory;
            bResult = saveFileDialog.ShowDialog();
            if (!bResult.HasValue || !bResult.Value)
                return;
            string sFichierDestination = saveFileDialog.FileName;

            // 3 - Génération du fichier destination
            try
            {
                _viewModel.RenderResultats(sFichierSource, sFichierDestination);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Erreur lors de la génération.\n" + exc.Message);
            }
        }

        void btnPalmares_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenererPalmares();
        }
        #endregion

        #region IPage Members

        public void ReinitialiserPage()
        {
            _viewModel = GetViewModel();

            DataContext = _viewModel;
        }

        #endregion
    }
}
