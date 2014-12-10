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
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BoutonLireHymne.xaml
    /// </summary>
    public partial class BoutonLireHymne : Button
    {
        public BoutonLireHymne()
        {
            InitializeComponent();

            this.Click += BoutonLireHymne_Click;
        }

        void BoutonLireHymne_Click(object sender, RoutedEventArgs e)
        {
            string sHymne = DataContext as string;
            if (string.IsNullOrEmpty(sHymne))
                return;

            Mp3Player.Play(sHymne);
        }
    }
}
