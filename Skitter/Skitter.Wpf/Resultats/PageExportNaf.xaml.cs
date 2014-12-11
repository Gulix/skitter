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

namespace Skitter.Wpf.Resultats
{
    public partial class PageExportNaf : UserControl
    {
        public PageExportNaf()
        {
            InitializeComponent();

            btnExcel.Click += btnExcel_Click;
            btnXml.Click += btnXml_Click;
        }

        void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("L'export va se faire dans le presse-papier.\nA chaque étape, copier le résultat de l'export dans un nouvel onglet de votre fichier Excel.");

            ExportNaf.CopierPP_ListeCoachesRosterRonde135();
            MessageBox.Show("Rosters des rondes 1, 3 et 5 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeCoachesRosterRonde2();
            MessageBox.Show("Rosters de la ronde 2 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeCoachesRosterRonde4();
            MessageBox.Show("Rosters des rondes 4 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeRencontres(1);
            MessageBox.Show("Résultats de la ronde 1 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeRencontres(2);
            MessageBox.Show("Résultats de la ronde 2 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeRencontres(3);
            MessageBox.Show("Résultats de la ronde 3 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeRencontres(4);
            MessageBox.Show("Résultats de la ronde 4 copiés. Validez pour passer à la page suivante.");

            ExportNaf.CopierPP_ListeRencontres(5);
            MessageBox.Show("Résultats de la ronde 5 copiés. Validez pour passer à la page suivante.");

        }

        void btnXml_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
