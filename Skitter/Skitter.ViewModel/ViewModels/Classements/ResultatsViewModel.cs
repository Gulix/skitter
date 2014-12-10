using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Nustache.Core;
using Skitter.Object;
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.ViewModel.ViewModels.Classements
{
    public abstract class ResultatsRondeViewModel : INotifyPropertyChanged
    {
        #region Méthodes abstraites
        protected abstract Tournoi.eTypePhaseTournoi GetPhaseSaisieNecessaire();
        public abstract string MessageBloquant {get;}
        public abstract List<Rencontre> GetListeRencontres();
        protected abstract int NumeroRonde { get; }
        #endregion
        
        
        public enum eTypeTri
        {
            Bashlord, 
            Defense,
            Attaque,
            General,
            Vicieux,
            Paillasson,
            Passoire,
            PoingsEnMousse
        }

        eTypeTri _typeTri = eTypeTri.General;

        public ResultatsRondeViewModel()
        {
            TrierGeneral();
        }

        #region Visibility des éléments
        public Visibility VisibilityMessageBloquant
        {
            get { return (Tournoi.GetInstance().PhaseEnCours <= GetPhaseSaisieNecessaire()) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility VisibilityClassement
        {
            get { return (Tournoi.GetInstance().PhaseEnCours > GetPhaseSaisieNecessaire()) ? Visibility.Visible : Visibility.Collapsed; }
        }
        #endregion

        #region Liste des résultats
        List<ResultatsEquipeViewModel> _lsResultats;
        
        private void GenererListeResultats()
        {
            List<Rencontre> lsRencontres = GetListeRencontres();
            ClassementCoachesViewModel classementCoaches = new ClassementCoachesViewModel(lsRencontres);

            _lsResultats = new List<ResultatsEquipeViewModel>();
            foreach (Equipe equipe in Tournoi.GetInstance().Equipes)
            {
                _lsResultats.Add(new ResultatsEquipeViewModel(equipe, lsRencontres, classementCoaches));
            }
        }

        public List<ResultatsEquipeViewModel> ListeResultatsEquipes
        {
            get
            {
                if (_lsResultats == null)
                    GenererListeResultats();
                return _lsResultats;
            }
        }

        private void ModifierTypeTri(eTypeTri typeTri)
        {
            _typeTri = typeTri;
            ModifierTriResultats();
            AffecterRangEquipe();
            RaisePropertyChanged("ListeResultatsEquipes");
            RaisePropertyChanged("BrushTriGeneral");
            RaisePropertyChanged("BrushTriBashlord");
            RaisePropertyChanged("BrushTriPoingsEnMousse");
            RaisePropertyChanged("BrushTriVicieux");
            RaisePropertyChanged("BrushTriAttaque");
            RaisePropertyChanged("BrushTriDefense");
            RaisePropertyChanged("BrushTriPaillasson");
            RaisePropertyChanged("BrushTriPassoire");
        }

        private void ModifierTriResultats()
        {
            foreach (ResultatsEquipeViewModel resultat in ListeResultatsEquipes)
                resultat.TypeTri = _typeTri;
        }

        public void TrierGeneral()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.NbVictoires)
                                                .ThenByDescending(r => r.NbNuls)
                                                .ThenByDescending(r => r.ValeurEquipe)
                                                .ThenByDescending(r => r.DuelsRemportes)
                                                .ThenBy(r => r.ClassementMeilleurJoueur)
                                                .ToList();
            ModifierTypeTri(eTypeTri.General);
        }

        public void TrierBashlord()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.SortiesEffectuees).ToList();
            ModifierTypeTri(eTypeTri.Bashlord);
        }

        public void TrierPoingsEnMousse()
        {
            _lsResultats = ListeResultatsEquipes.OrderBy(r => r.SortiesEffectuees + r.SortiesVicieusesEffectuees).ToList();
            ModifierTypeTri(eTypeTri.PoingsEnMousse);
        }

        public void TrierVicieux()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.SortiesVicieusesEffectuees).ToList();
            ModifierTypeTri(eTypeTri.Vicieux);
        }

        public void TrierAttaque()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.TDMarques).ToList();
            ModifierTypeTri(eTypeTri.Attaque);
        }

        public void TrierDefense()
        {
            _lsResultats = ListeResultatsEquipes.OrderBy(r => r.TDEncaisses).ToList();
            ModifierTypeTri(eTypeTri.Defense);
        }

        public void TrierPassoire()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.TDEncaisses).ToList();
            ModifierTypeTri(eTypeTri.Passoire);
        }

        public void TrierPaillasson()
        {
            _lsResultats = ListeResultatsEquipes.OrderByDescending(r => r.TotalSortiesSubies).ToList();
            ModifierTypeTri(eTypeTri.Paillasson);
        }

        private void AffecterRangEquipe()
        {
            for (int i = 0; i < ListeResultatsEquipes.Count; i++)
                ListeResultatsEquipes[i].RangEquipe = i + 1;
        }
        #endregion

        #region Copie vers Excel
        /// <summary>
        /// Effectue une copie du classement actuel vers le format CSV
        /// </summary>
        public void CopierVersExcel()
        {
            // Génération de la ligne d'en-tête
            string sAvecVirgule = "Equipe,V,N,D,Coeff.,Duels gagnés,Meill. class.,TD+,TD-,Sorties+,Sorties-,Vicieux+,Vicieux-,Bless. subies,";
            string sAvecTab = sAvecVirgule.Replace(',', '\t');
                                           
            // Génération des lignes des équipes
            foreach (ResultatsEquipeViewModel resVM in ListeResultatsEquipes)
            {
                sAvecVirgule += "\n" + resVM.NomEquipe + "," + resVM.NbVictoires + "," + resVM.NbNuls + "," + resVM.NbDefaites + "," + resVM.ValeurEquipe
                    + "," + resVM.DuelsRemportes + "," + resVM.ClassementMeilleurJoueur + "," + resVM.TDMarques + "," + resVM.TDEncaisses
                    + "," + resVM.SortiesEffectuees + "," + resVM.SortiesSubies + "," + resVM.SortiesVicieusesEffectuees
                    + "," + resVM.SortiesVicieusesSubies + "," + resVM.TotalSortiesSubies + ",";

                sAvecTab += "\n" + resVM.NomEquipe + "\t" + resVM.NbVictoires + "\t" + resVM.NbNuls + "\t" + resVM.NbDefaites + "\t" + resVM.ValeurEquipe
                    + "\t" + resVM.DuelsRemportes + "\t" + resVM.ClassementMeilleurJoueur + "\t" + resVM.TDMarques + "\t" + resVM.TDEncaisses
                    + "\t" + resVM.SortiesEffectuees + "\t" + resVM.SortiesSubies + "\t" + resVM.SortiesVicieusesEffectuees
                    + "\t" + resVM.SortiesVicieusesSubies + "\t" + resVM.TotalSortiesSubies + "\t";
            }

            DataObject dataObject = new System.Windows.DataObject();

            dataObject.SetText(sAvecTab);
            // Convert the CSV text to a UTF-8 byte stream before adding it to the container object.
            var bytes = System.Text.Encoding.UTF8.GetBytes(sAvecVirgule);
            var stream = new System.IO.MemoryStream(bytes);
            dataObject.SetData(System.Windows.DataFormats.CommaSeparatedValue, stream);

            // Copy the container object to the clipboard.
            System.Windows.Clipboard.SetDataObject(dataObject, true);
        }
        #endregion

        #region Couleurs des boutons de tri
        private Brush GetBrushSelonTri(eTypeTri typTri)
        {
            if (typTri == _typeTri)
                return new SolidColorBrush(Color.FromRgb(134, 195, 240));
            else
                return Brushes.White;
        }

        public Brush BrushTriGeneral { get { return GetBrushSelonTri(eTypeTri.General); } }
        public Brush BrushTriBashlord { get { return GetBrushSelonTri(eTypeTri.Bashlord); } }
        public Brush BrushTriPoingsEnMousse { get { return GetBrushSelonTri(eTypeTri.PoingsEnMousse); } }
        public Brush BrushTriVicieux { get { return GetBrushSelonTri(eTypeTri.Vicieux); } }
        public Brush BrushTriAttaque { get { return GetBrushSelonTri(eTypeTri.Attaque); } }
        public Brush BrushTriDefense { get { return GetBrushSelonTri(eTypeTri.Defense); } }
        public Brush BrushTriPassoire { get { return GetBrushSelonTri(eTypeTri.Passoire); } }
        public Brush BrushTriPaillasson { get { return GetBrushSelonTri(eTypeTri.Paillasson); } }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string sProperty)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sProperty));
        }

        #endregion

        #region Accesseurs
        public string DescriptionRonde
        {
            get { return string.Format("Ronde {0}", NumeroRonde); }
        }

        public string DescriptionClassement
        {
            get
            {
                switch (_typeTri)
                {
                    case eTypeTri.Attaque:
                        return "Classement 'Meilleure attaque'";
                    case eTypeTri.Bashlord:
                        return "Classement Bashlord";
                    case eTypeTri.Defense:
                        return "Classement 'Meilleure défense'";
                    case eTypeTri.General:
                        return "Classement général";
                    case eTypeTri.Paillasson:
                        return "Classement Victime";
                    case eTypeTri.Passoire:
                        return "Classement Passoire";
                    case eTypeTri.Vicieux:
                        return "Classement Vicieux";
                    case eTypeTri.PoingsEnMousse:
                        return "Classement Poings en mousse";
                }
                return string.Empty;
            }
        }
        #endregion

        public void RenderResultats(string sFichierSource, string sFichierDestination)
        {
            Render.FileToFile(sFichierSource, this, sFichierDestination);
            Process.Start(sFichierDestination);
        }

        public void GenererPalmares()
        {
            Dictionary<string, ResultatPalmares> dicPalmares = new Dictionary<string, ResultatPalmares>();

            TrierGeneral();
            ResultatsEquipeViewModel res = _lsResultats.Last();
            dicPalmares.Add("Derniers", new ResultatPalmares(string.Format("{0} / {1} / {2}", res.NbVictoires, res.NbNuls, res.NbDefaites), res.Equipe));
            TrierPaillasson();
            res = _lsResultats.First();
            dicPalmares.Add("Victimes", new ResultatPalmares(string.Format("{0} sorties subies", res.TotalSortiesSubies), res.Equipe));
            TrierPassoire();
            res = _lsResultats.First();
            dicPalmares.Add("Passoires", new ResultatPalmares(string.Format("{0} TD encaissés", res.TDEncaisses), res.Equipe));
            TrierPoingsEnMousse();
            res = _lsResultats.First();
            dicPalmares.Add("PoingsMousse", new ResultatPalmares(string.Format("{0} sorties effectuées", res.SortiesEffectuees + res.SortiesVicieusesEffectuees), res.Equipe));
            TrierAttaque();
            res = _lsResultats.First();
            dicPalmares.Add("Attaque", new ResultatPalmares(string.Format("{0} TD marqués", res.TDMarques), res.Equipe));
            TrierDefense();
            res = _lsResultats.First();
            dicPalmares.Add("Defense", new ResultatPalmares(string.Format("{0} TD encaissés", res.TDEncaisses), res.Equipe));
            TrierBashlord();
            res = _lsResultats.First();
            dicPalmares.Add("Bashlord", new ResultatPalmares(string.Format("{0} sorties effectuées", res.SortiesEffectuees), res.Equipe));
            TrierVicieux();
            res = _lsResultats.First();
            dicPalmares.Add("Vicieux", new ResultatPalmares(string.Format("{0} sorties vicieuses effectuées", res.SortiesVicieusesEffectuees), res.Equipe));
            TrierGeneral();
            res = _lsResultats[2];
            dicPalmares.Add("3e", new ResultatPalmares(string.Format("{0} / {1} / {2} ({3} pts d'équipe, {4} duels gagnés)", 
                                                                     res.NbVictoires, res.NbNuls, res.NbDefaites, res.ValeurEquipe, res.DuelsRemportes), res.Equipe));
            res = _lsResultats[1];
            dicPalmares.Add("2e", new ResultatPalmares(string.Format("{0} / {1} / {2} ({3} pts d'équipe, {4} duels gagnés)",
                                                                     res.NbVictoires, res.NbNuls, res.NbDefaites, res.ValeurEquipe, res.DuelsRemportes), res.Equipe));
            res = _lsResultats[0];
            dicPalmares.Add("1er", new ResultatPalmares(string.Format("{0} / {1} / {2} ({3} pts d'équipe, {4} duels gagnés)",
                                                                     res.NbVictoires, res.NbNuls, res.NbDefaites, res.ValeurEquipe, res.DuelsRemportes), res.Equipe));


            string sDestination = Path.Combine(FileAndDirectory.ExeDirectory, "Strut", "Palmares.htm");
            Render.FileToFile(Path.Combine(FileAndDirectory.ExeDirectory, "Strut", "Palmares_template.htm"),
                dicPalmares,
                sDestination);
            Process.Start(sDestination);
        }
    }
}
