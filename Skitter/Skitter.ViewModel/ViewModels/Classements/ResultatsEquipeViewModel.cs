using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Classements
{
    public class ResultatsEquipeViewModel : INotifyPropertyChanged
    {
        #region Variables
        Equipe _equipe;
        List<Rencontre> _lsRencontres;
        ClassementCoachesViewModel _classementCoaches;

        int _iRang;
        ResultatsRondeViewModel.eTypeTri _typeTri = ResultatsRondeViewModel.eTypeTri.General;
        #endregion

        #region Constructeur
        public ResultatsEquipeViewModel(Equipe equipe, List<Rencontre> lsRencontres, ClassementCoachesViewModel classementCoach)
        {
            _equipe = equipe;
            _lsRencontres = lsRencontres.Where(r => (r.IdEquipe1 == _equipe.IdEquipe) || (r.IdEquipe2 == _equipe.IdEquipe)).ToList();
            _classementCoaches = classementCoach;
        }
        #endregion

        #region Accesseurs
        public int IdEquipe
        {
            get { return _equipe.IdEquipe; }
        }
        
        public string NomEquipe
        {
            get { return _equipe.NomEquipe; }
        }

        public Equipe Equipe
        {
            get { return _equipe; }
        }

        public int NbVictoires
        {
            get
            {
                return _lsRencontres.Where(r => ((r.IdEquipe1 == _equipe.IdEquipe) && (r.ScoreEquipe1 > r.ScoreEquipe2))
                                             || ((r.IdEquipe2 == _equipe.IdEquipe) && (r.ScoreEquipe1 < r.ScoreEquipe2)))
                                    .Count();
            }
        }

        public int NbNuls
        {
            get { return _lsRencontres.Where(r => r.ScoreEquipe1 == r.ScoreEquipe2).Count(); }
        }

        public int NbDefaites
        {
            get
            {
                return _lsRencontres.Where(r => ((r.IdEquipe1 == _equipe.IdEquipe) && (r.ScoreEquipe1 < r.ScoreEquipe2))
                                             || ((r.IdEquipe2 == _equipe.IdEquipe) && (r.ScoreEquipe1 > r.ScoreEquipe2)))
                                    .Count();
            }
        }

        public int ValeurEquipe
        {
            get { return _equipe.ValeurRosterEquipe; }
        }

        public int DuelsRemportes
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.ScoreEquipe1 : r.ScoreEquipe2); }
        }

        public int ClassementMeilleurJoueur
        {
            get
            {
                return Math.Min(_classementCoaches.ObtenirClassementCoach(_equipe.Capitaine.IdCoach),
                                Math.Min(_classementCoaches.ObtenirClassementCoach(_equipe.Equipier1.IdCoach),
                                         _classementCoaches.ObtenirClassementCoach(_equipe.Equipier2.IdCoach)
                                        )
                               );
            }
        }

        public int TDMarques
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.TDEquipe1 : r.TDEquipe2); }
        }

        public int TDEncaisses
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.TDEquipe2 : r.TDEquipe1); }
        }

        public int SortiesEffectuees
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.SortiesEquipe1 : r.SortiesEquipe2); }
        }

        public int SortiesSubies
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.SortiesEquipe2 : r.SortiesEquipe1); }
        }

        public int SortiesVicieusesEffectuees
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.SortiesVicieusesEquipe1 : r.SortiesVicieusesEquipe2); }
        }

        public int SortiesVicieusesSubies
        {
            get { return _lsRencontres.Sum(r => (r.IdEquipe1 == _equipe.IdEquipe) ? r.SortiesVicieusesEquipe2 : r.SortiesVicieusesEquipe1); }
        }

        public int TotalSortiesSubies
        {
            get { return SortiesSubies + SortiesVicieusesSubies; }
        }

        public int DifferentielSortiesTD
        {
            get
            {
                return (SortiesEffectuees + SortiesVicieusesEffectuees + TDMarques) - (TotalSortiesSubies + TDEncaisses);
            }
        }

        public int RangEquipe
        {
            get { return _iRang; }
            set { _iRang = value; RaisePropertyChanged("RangEquipe"); }
        }

        public ResultatsRondeViewModel.eTypeTri TypeTri
        {
            get { return _typeTri; }
            set
            {
                _typeTri = value;
                RaiseFontWeightProperties();
            }
        }

        public string TooltipEquipe
        {
            get
            {
                return string.Format("{0} ({1})\n{2} ({3})\n{4} ({5})",
                    _equipe.Capitaine.NomCoach, _equipe.Capitaine.NomRoster,
                    _equipe.Equipier1.NomCoach, _equipe.Equipier1.NomRoster,
                    _equipe.Equipier2.NomCoach, _equipe.Equipier2.NomRoster);
            }
        }
        #endregion

        #region Anthem
        /// <summary>
        /// Anthem to be played for the Team
        /// </summary>
        public string Anthem
        {
            get { return _equipe.HymneEquipe; }
        }

        /// <summary>
        /// Is the Anthem button visible ?
        /// </summary>
        public Visibility AnthemVisibility
        {
            get { return string.IsNullOrEmpty(Anthem) ? Visibility.Hidden : Visibility.Visible; }
        }
        #endregion

        #region Résultats individuels
        ResultatsCoachViewModel _resultatsCapitaine;
        ResultatsCoachViewModel _resultatsEquipier1;
        ResultatsCoachViewModel _resultatsEquipier2;
        
        public ResultatsCoachViewModel ResultatsCapitaine
        {
            get
            {
                if (_resultatsCapitaine == null)
                    _resultatsCapitaine = new ResultatsCoachViewModel(_equipe.Capitaine, _lsRencontres);
                return _resultatsCapitaine;
            }
        }

        public ResultatsCoachViewModel ResultatsEquipier1
        {
            get
            {
                if (_resultatsEquipier1 == null)
                    _resultatsEquipier1 = new ResultatsCoachViewModel(_equipe.Equipier1, _lsRencontres);
                return _resultatsEquipier1;
            }
        }

        public ResultatsCoachViewModel ResultatsEquipier2
        {
            get
            {
                if (_resultatsEquipier2 == null)
                    _resultatsEquipier2 = new ResultatsCoachViewModel(_equipe.Equipier2, _lsRencontres);
                return _resultatsEquipier2;
            }
        }
        #endregion

        #region Affichage du classement selon le tri
        private void RaiseFontWeightProperties()
        {
            RaisePropertyChanged("NbVictoiresFontWeight");
            RaisePropertyChanged("NbNulsFontWeight");
            RaisePropertyChanged("ValeurEquipeFontWeight");
            RaisePropertyChanged("DuelsRemportesFontWeight");
            RaisePropertyChanged("ClassementMeilleurJoueurFontWeight");
            RaisePropertyChanged("TDMarquesFontWeight");
            RaisePropertyChanged("TDEncaissesFontWeight");
            RaisePropertyChanged("SortiesEffectueesFontWeight");
            RaisePropertyChanged("SortiesVicieusesEffectueesFontWeight");
            RaisePropertyChanged("TotalSortiesSubiesFontWeight");
        }

        public FontWeight NbVictoiresFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.General) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight NbNulsFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.General) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight ValeurEquipeFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.General) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight DuelsRemportesFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.General) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight ClassementMeilleurJoueurFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.General) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight TDMarquesFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.Attaque) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight TDEncaissesFontWeight
        {
            get
            {
                return ((_typeTri == ResultatsRondeViewModel.eTypeTri.Passoire) || (_typeTri == ResultatsRondeViewModel.eTypeTri.Defense)) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight SortiesEffectueesFontWeight
        {
            get
            {
                return ((_typeTri == ResultatsRondeViewModel.eTypeTri.Bashlord) || (_typeTri == ResultatsRondeViewModel.eTypeTri.PoingsEnMousse)) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight SortiesVicieusesEffectueesFontWeight
        {
            get
            {
                return ((_typeTri == ResultatsRondeViewModel.eTypeTri.Vicieux) || (_typeTri == ResultatsRondeViewModel.eTypeTri.PoingsEnMousse)) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }

        public FontWeight TotalSortiesSubiesFontWeight
        {
            get
            {
                return (_typeTri == ResultatsRondeViewModel.eTypeTri.Paillasson) ?
                    FontWeights.Bold : FontWeights.Normal;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string sProperty)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(sProperty));
        }

        #endregion
    }
}
