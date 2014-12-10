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

namespace Skitter.Wpf.Deroulement
{
    /// <summary>
    /// Interaction logic for UCPreparationRencontre.xaml
    /// </summary>
    public partial class UCPreparationRencontre : UserControl
    {
        public UCPreparationRencontre()
        {
            InitializeComponent();

            btnSelectionEquipe1.Click += btnSelectionEquipe1_Click;
            btnSelectionEquipe2.Click += btnSelectionEquipe2_Click;
        }

        void btnSelectionEquipe2_Click(object sender, RoutedEventArgs e)
        {
            PreparationRencontreViewModel rencontreVM = DataContext as PreparationRencontreViewModel;
            if (rencontreVM == null)
                return;

            rencontreVM.Equipe2Selectionnee = !rencontreVM.Equipe2Selectionnee;
        }

        void btnSelectionEquipe1_Click(object sender, RoutedEventArgs e)
        {
            PreparationRencontreViewModel rencontreVM = DataContext as PreparationRencontreViewModel;
            if (rencontreVM == null)
                return;

            rencontreVM.Equipe1Selectionnee = !rencontreVM.Equipe1Selectionnee;
        }
    }
}
