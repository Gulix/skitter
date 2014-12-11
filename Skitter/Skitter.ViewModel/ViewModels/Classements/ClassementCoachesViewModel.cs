using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Classements
{
    public class ClassementCoachesViewModel
    {
        #region Variables
        List<ResultatsCoachViewModel> _lsResultatsCoaches;
        #endregion

        #region Constructeur
        public ClassementCoachesViewModel(List<Rencontre> lsRencontres)
        {
            _lsResultatsCoaches = new List<ResultatsCoachViewModel>();
            
            foreach (Coach coach in Tournoi.GetInstance().Coaches)
            {
                _lsResultatsCoaches.Add(new ResultatsCoachViewModel(coach, lsRencontres));
            }

            _lsResultatsCoaches = _lsResultatsCoaches.OrderByDescending(r => r.NbVictoires)
                                                     .ThenByDescending(r => r.NbNuls)
                                                     .ThenByDescending(r => r.ValeurRoster)
                                                     .ThenByDescending(r => r.DifferentielSortiesTD)
                                                     .ToList();
        }
        #endregion

        public int ObtenirClassementCoach(int idCoach)
        {
            return _lsResultatsCoaches.FindIndex(r => r.IdCoach == idCoach) + 1;
        }

        /// <summary>
        /// Retourne le classement (1 à X) d'un coach, et son bilan VND, avant les rencontres de la ronde indiquée
        /// </summary>
        internal static string GetClassementBilanAvantRonde(Coach coach, int iNumeroRonde)
        {
            if (coach == null)
                return string.Empty;

            ClassementCoachesViewModel vm = new ClassementCoachesViewModel(Tournoi.GetRencontresAvant(iNumeroRonde));
            int iClassement = vm.ObtenirClassementCoach(coach.IdCoach);
            ResultatsCoachViewModel resultatsCoach = vm._lsResultatsCoaches.FirstOrDefault(r => r.IdCoach == coach.IdCoach);
            if (resultatsCoach == null)
                return string.Empty;

            return string.Format("{0}{1} en individuel : {2}/{3}/{4}",
                iClassement, (iClassement > 1) ? "e" : "er", resultatsCoach.NbVictoires, resultatsCoach.NbNuls, resultatsCoach.NbDefaites);
        }

        internal static int GetClassementFinalCoach(Coach coach)
        {
            if (coach == null)
                return 0;

            ClassementCoachesViewModel vm = new ClassementCoachesViewModel(Tournoi.GetRencontresApres(5));
            return vm.ObtenirClassementCoach(coach.IdCoach);
        }
    }
}
