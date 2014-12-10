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
    [TemplatePart(Name = "btnInitAleatoire", Type = typeof(Button))]
    [TemplatePart(Name = "btnInitClassement", Type = typeof(Button))]
    [TemplatePart(Name = "btnAnnulerOrganisation", Type = typeof(Button))]
    [TemplatePart(Name = "btnValiderOrganisation", Type = typeof(Button))]
    [TemplatePart(Name = "btnAnnulerSaisie", Type = typeof(Button))]
    [TemplatePart(Name = "btnValiderSaisie", Type = typeof(Button))]
    /// <summary>
    /// Classe abstraite permettant de gérer l'ensemble des rondes du tournoi
    /// </summary>
    public abstract class PageRonde : UserControl, IPage
    {
        static PageRonde()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageRonde), new FrameworkPropertyMetadata(typeof(PageRonde)));
        }

        RondeViewModel _viewModel;
        bool _bLoaded = false;

        protected abstract RondeViewModel GetViewModel();

        public PageRonde()
        {
            PageManager.AjouterPage(this);

            ReinitialiserPage();

            Loaded += PageRonde_Loaded;
        }

        void PageRonde_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bLoaded)
                return;
            _bLoaded = true;

            Button btnInitAleatoire = (Button)Template.FindName("PART_btnInitAleatoire", this);
            btnInitAleatoire.Click += btnInitAleatoire_Click;
            Button btnInitClassement = (Button)Template.FindName("PART_btnInitClassement", this);
            btnInitClassement.Click += btnInitClassement_Click;
            Button btnAnnulerOrganisation = (Button)Template.FindName("PART_btnAnnulerOrganisation", this);
            btnAnnulerOrganisation.Click += btnAnnulerOrganisation_Click;
            Button btnValiderOrganisation = (Button)Template.FindName("PART_btnValiderOrganisation", this);
            btnValiderOrganisation.Click += btnValiderOrganisation_Click;
            Button btnAnnulerSaisie = (Button)Template.FindName("PART_btnAnnulerSaisie", this);
            btnAnnulerSaisie.Click += btnAnnulerSaisie_Click;
            Button btnValiderSaisie = (Button)Template.FindName("PART_btnValiderSaisie", this);
            btnValiderSaisie.Click += btnValiderSaisie_Click;
            Button btnGenererHTML = (Button)Template.FindName("PART_btnGenererHTML", this);
            btnGenererHTML.Click += btnGenererHTML_Click;
            Button btnGenererHTML2 = (Button)Template.FindName("PART_btnGenererHTML2", this);
            btnGenererHTML2.Click += btnGenererHTML_Click;
            Button btnGenererPresentation = (Button)Template.FindName("PART_btnGenererPresentation", this);
            btnGenererPresentation.Click += btnGenererPresentation_Click;
            Button btnGenererPresentation2 = (Button)Template.FindName("PART_btnGenererPresentation2", this);
            btnGenererPresentation2.Click += btnGenererPresentation_Click;
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

        #region Génération du HTML pour la feuille des résultats
        void btnGenererHTML_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenererHTML();
        }

        void btnGenererPresentation_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenererPresentation();
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
