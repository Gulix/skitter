﻿using System;
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
using Skitter.ViewModel.ViewModels.Classements;

namespace Skitter.Wpf.Resultats
{
    public partial class PageResultatsRonde3 : PageResultatsRonde
    {
        public PageResultatsRonde3()
            : base()
        {
            InitializeComponent();
        }

        protected override ResultatsRondeViewModel GetViewModel()
        {
            return new ResultatsRonde3ViewModel();
        }
    }
}
