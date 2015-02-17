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
            _lsRencontres = lsRencontres.Where(r => (r.IdParticipant1 == _equipe.IdEquipe) || (r.IdParticipant2 == _equipe.IdEquipe)).ToList();
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

        public string Hymne
        {
            get { return _equipe.HymneEquipe; }
        }

        public int NbVictoires
        {
            get
            {
                return _lsRencontres.Where(r => ((r.IdParticipant1 == _equipe.IdEquipe) && (r.NbVictoiresParticipant1 > r.NbVictoiresParticipant2))
                                             || ((r.IdParticipant2 == _equipe.IdEquipe) && (r.NbVictoiresParticipant1 < r.NbVictoiresParticipant2)))
                                    .Count();
            }
        }

        public int NbNuls
        {
            get { return _lsRencontres.Where(r => r.NbVictoiresParticipant1 == r.NbVictoiresParticipant2).Count(); }
        }

        public int NbDefaites
        {
            get
            {
                return _lsRencontres.Where(r => ((r.IdParticipant1 == _equipe.IdEquipe) && (r.NbVictoiresParticipant1 < r.NbVictoiresParticipant2))
                                             || ((r.IdParticipant2 == _equipe.IdEquipe) && (r.NbVictoiresParticipant1 > r.NbVictoiresParticipant2)))
                                    .Count();
            }
        }

        public int ValeurEquipe
        {
            get { return _equipe.ValeurRosterEquipe; }
        }

        public int DuelsRemportes
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.NbVictoiresParticipant1 : r.NbVictoiresParticipant2); }
        }

        public int ClassementMeilleurJoueur
        {
            get
            {
                return _equipe.ListeIdCoaches.Select(id => _classementCoaches.ObtenirClassementCoach(id)).Min();
            }
        }

        public int TDMarques
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.TDEquipe1 : r.TDEquipe2); }
        }

        public int TDEncaisses
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.TDEquipe2 : r.TDEquipe1); }
        }

        public int SortiesEffectuees
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.SortiesEquipe1 : r.SortiesEquipe2); }
        }

        public int SortiesSubies
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.SortiesEquipe2 : r.SortiesEquipe1); }
        }

        public int SortiesVicieusesEffectuees
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.SortiesVicieusesEquipe1 : r.SortiesVicieusesEquipe2); }
        }

        public int SortiesVicieusesSubies
        {
            get { return _lsRencontres.Sum(r => (r.IdParticipant1 == _equipe.IdEquipe) ? r.SortiesVicieusesEquipe2 : r.SortiesVicieusesEquipe1); }
        }

        public int TotalSortiesSubies
        {
            get { return SortiesSubies + SortiesVicieusesSubies; }
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
                string sRetour = string.Empty;
                foreach(Coach coach in _equipe.ListeCoaches)
                {
                    sRetour += (string.IsNullOrEmpty(sRetour) ? string.Empty : "\n")
                        + string.Format("{0} ({1})", coach.NomCoach, coach.NomRoster);
                }

                return sRetour;
            }
        }
        #endregion

        #region Résultats individuels
        List<ResultatsCoachViewModel> _lsResultatsCoachViewModels;
        void InitResultatsCoachVM()
        {
            _lsResultatsCoachViewModels = new List<ResultatsCoachViewModel>();
            foreach (Coach c in _equipe.ListeCoaches)
                _lsResultatsCoachViewModels.Add(new ResultatsCoachViewModel(c, _lsRencontres));
        }

        List<ResultatsCoachViewModel> ListeResultatsCoachViewModels
        {
            get
            {
                if (_lsResultatsCoachViewModels == null)
                    InitResultatsCoachVM();
                return _lsResultatsCoachViewModels;
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
