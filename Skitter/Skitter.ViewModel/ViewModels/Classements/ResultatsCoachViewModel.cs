using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Classements
{
    class ResultatsCoachViewModel
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
                if ((rencontre.Duel1.IdCoach1 == _coach.IdCoach) ||(rencontre.Duel1.IdCoach2 == _coach.IdCoach))
                    _lsDuels.Add(rencontre.Duel1);
                if ((rencontre.Duel2.IdCoach1 == _coach.IdCoach) ||(rencontre.Duel2.IdCoach2 == _coach.IdCoach))
                    _lsDuels.Add(rencontre.Duel2);
                if ((rencontre.Duel3.IdCoach1 == _coach.IdCoach) ||(rencontre.Duel3.IdCoach2 == _coach.IdCoach))
                    _lsDuels.Add(rencontre.Duel3);
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
        #endregion
    }
}
