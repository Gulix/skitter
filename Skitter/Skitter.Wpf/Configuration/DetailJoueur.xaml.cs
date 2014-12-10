/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 14/07/2014
 * Time: 18:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Skitter.ViewModel.ViewModels;

namespace Skitter.Wpf.Configuration
{
	/// <summary>
	/// Interaction logic for DetailJoueur.xaml
	/// </summary>
	public partial class DetailJoueur : Grid
	{
		public DetailJoueur()
		{
			InitializeComponent();

            btnLienNAF.Click += btnLienNAF_Click;
		}

        void btnLienNAF_Click(object sender, RoutedEventArgs e)
        {
            CoachViewModel viewModel = DataContext as CoachViewModel;
            if ((viewModel == null) || string.IsNullOrEmpty(viewModel.LienProfilNAF))
                return;

            Process.Start(viewModel.LienProfilNAF);
        }
	}
}