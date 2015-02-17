using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Classements
{
    public class ResultatsCoachViewModel
    {
        #region Variables
        Coach _coach;
        List<Duel> _lsDuels;
        #endregion

        #region Constructeur
        public ResultatsCoachViewModel(Coach coach, List<Rencontre> lsRencontres)
        {
            _coach = coach;
            _lsDuels = new List<Duel>();
            foreach(Rencontre rencontre in lsRencontres)
            {
                _lsDuels.AddRange(rencontre.ListeDuels.Where(d => d.APrisPartAuDuel(coach)));
            }
        }
        #endregion

        #region Accesseurs
        public int IdCoach { get { return _coach.IdCoach; } }

        public int NbVictoires
        {
            get
            {
                return _lsDuels.Count(d => ((d.IdCoach1 == _coach.IdCoach) && (d.ResultatCoach1.NbTD > d.ResultatCoach2.NbTD))
                                        || ((d.IdCoach2 == _coach.IdCoach) && (d.ResultatCoach1.NbTD < d.ResultatCoach2.NbTD)));
            }
        }

        public int NbNuls
        {
            get { return _lsDuels.Count(d => d.ResultatCoach1.NbTD == d.ResultatCoach2.NbTD); }
        }

        public int NbDefaites
        {
            get
            {
                return _lsDuels.Count(d => ((d.IdCoach1 == _coach.IdCoach) && (d.ResultatCoach1.NbTD < d.ResultatCoach2.NbTD))
                                        || ((d.IdCoach2 == _coach.IdCoach) && (d.ResultatCoach1.NbTD > d.ResultatCoach2.NbTD)));
            }
        }

        public int ValeurRoster
        {
            get { return _coach.ValeurRoster; }
        }

        public int DifferentielSortiesTD
        {
            get
            {
                return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach)
                    ? (d.ResultatCoach1.NbTD + d.ResultatCoach1.NbSorties + d.ResultatCoach1.NbSortiesVicieuses
                        - d.ResultatCoach2.NbTD - d.ResultatCoach2.NbSorties - d.ResultatCoach2.NbSortiesVicieuses)
                    : (d.ResultatCoach2.NbTD + d.ResultatCoach2.NbSorties + d.ResultatCoach2.NbSortiesVicieuses
                        - d.ResultatCoach1.NbTD - d.ResultatCoach1.NbSorties - d.ResultatCoach1.NbSortiesVicieuses));

            }
        }

        public int TDMarques
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach1.NbTD : d.ResultatCoach2.NbTD); }
        }

        public int TDEncaisses
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach2.NbTD : d.ResultatCoach1.NbTD); }
        }

        public int SortiesEffectuees
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach1.NbSorties : d.ResultatCoach2.NbSorties); }
        }

        public int SortiesSubies
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach2.NbSorties : d.ResultatCoach1.NbSorties); }
        }

        public int SortiesVicieusesEffectuees
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach1.NbSortiesVicieuses : d.ResultatCoach2.NbSortiesVicieuses); }
        }

        public int SortiesVicieusesSubies
        {
            get { return _lsDuels.Sum(d => (d.IdCoach1 == _coach.IdCoach) ? d.ResultatCoach2.NbSortiesVicieuses : d.ResultatCoach1.NbSortiesVicieuses); }
        }

        public int TotalSortiesSubies
        {
            get { return SortiesSubies + SortiesVicieusesSubies; }
        }

        public int ClassementIndiv
        {
            get { return ClassementCoachesViewModel.GetClassementFinalCoach(_coach); }
        }
        #endregion
    }
}
