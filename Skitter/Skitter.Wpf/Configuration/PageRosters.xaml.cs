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
    /// Rosters' configuration page
    /// </summary>
    public partial class PageRosters : UserControl, IPage
    {
        RostersListViewModel _pageViewModel;
        
        public PageRosters()
        {
            PageManager.AjouterPage(this);

            InitializeComponent();

            ReinitialiserPage();

            btnDefaultRosters.Click += btnDefaultRosters_Click;
        }

        void btnDefaultRosters_Click(object sender, RoutedEventArgs e)
        {
            _pageViewModel.SetDefaultList();
        }

        #region IPage Members

        public void ReinitialiserPage()
        {
            _pageViewModel = new RostersListViewModel();
            DataContext = _pageViewModel;
        }

        #endregion
    }
}
