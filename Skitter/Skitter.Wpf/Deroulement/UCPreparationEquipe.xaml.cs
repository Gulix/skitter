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
    /// Interaction logic for UCPreparationEquipe.xaml
    /// </summary>
    public partial class UCPreparationEquipe : UserControl
    {
        public UCPreparationEquipe()
        {
            InitializeComponent();

            btnDescendreRang2.Click += btnDescendreRang2_Click;
            btnMonterRang2.Click += btnMonterRang2_Click;
        }

        void btnMonterRang2_Click(object sender, RoutedEventArgs e)
        {
            PreparationEquipeViewModel equipeVM = DataContext as PreparationEquipeViewModel;
            if (equipeVM == null)
                return;

            equipeVM.MonterCoachRang2();
        }

        void btnDescendreRang2_Click(object sender, RoutedEventArgs e)
        {
            PreparationEquipeViewModel equipeVM = DataContext as PreparationEquipeViewModel;
            if (equipeVM == null)
                return;

            equipeVM.DescendreCoachRang2();
        }
    }
}
