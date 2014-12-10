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
using Skitter.ViewModel.ViewModels.Rondes;

namespace Skitter.Wpf.Deroulement
{
    public partial class PageRonde3 : PageRonde
    {
        public PageRonde3()
            : base()
        {
            InitializeComponent();
        }

        protected override ViewModel.ViewModels.Rondes.RondeViewModel GetViewModel()
        {
            return new Ronde3ViewModel();
        }
    }
}
