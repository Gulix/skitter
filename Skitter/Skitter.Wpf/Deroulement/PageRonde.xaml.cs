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
using Skitter.ViewModel.ViewModels;
using Skitter.ViewModel.ViewModels.Rondes;

namespace Skitter.Wpf.Deroulement
{
    /// <summary>
    /// Classe abstraite permettant de gérer l'ensemble des rondes du tournoi
    /// </summary>
    public abstract partial class PageRonde : UserControl, IPage
    {
        static PageRonde()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageRonde), new FrameworkPropertyMetadata(typeof(PageRonde)));
        }

        RondeViewModel _viewModel;

        protected abstract RondeViewModel GetViewModel();

        public PageRonde()
        {
            PageManager.AjouterPage(this);

            InitializeComponent();

            ReinitialiserPage();

            btnInitAleatoire.Click += btnInitAleatoire_Click;
            btnInitClassement.Click += btnInitClassement_Click;
            btnAnnulerOrganisation.Click += btnAnnulerOrganisation_Click;
            btnValiderOrganisation.Click += btnValiderOrganisation_Click;
            btnAnnulerSaisie.Click += btnAnnulerSaisie_Click;
            btnValiderSaisie.Click += btnValiderSaisie_Click;
        }

        #region Etat "Choix initialisation"
        void btnInitClassement_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.InitialiserSelonClassement();
                PageManager.ReinitialiserToutesPages(this);
            }
        }

        void btnInitAleatoire_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.InitialiserSelonAleatoire();
                PageManager.ReinitialiserToutesPages(this);
            }
        }
        #endregion

        #region Etat "Organisation des matches
        void btnAnnulerOrganisation_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AnnulerOrganisation();
                PageManager.ReinitialiserToutesPages(this);
            }
        }

        void btnValiderOrganisation_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.ValiderOrganisation();
                PageManager.ReinitialiserToutesPages(this);
            }
        }
        #endregion

        #region Etat "Saisie des résultats"
        void btnValiderSaisie_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.ValiderSaisie();
                PageManager.ReinitialiserToutesPages(this);
            }
        }

        void btnAnnulerSaisie_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AnnulerSaisie();
                PageManager.ReinitialiserToutesPages(this);
            }
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
