using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels
{
    public class CoachRondeViewModel
    {
        #region Variables
        Coach _coach;
        int _iRonde;
        #endregion

        #region Constructeur
        public CoachRondeViewModel(Coach coach, int iNumeroRonde)
        {
            _coach = coach;
            _iRonde = iNumeroRonde;
        }
        #endregion

        #region Accesseurs
        public string NomCoachAvecRoster
        {
            get
            {
                Coach.eTypeRosterJoue typRoster = Coach.RosterJoueSelonRonde(_iRonde);
                if (typRoster == Coach.eTypeRosterJoue.RosterClassique)
                    return string.Format("{0} ({1})", _coach.NomCoach, _coach.NomRoster);

                Coach coachPreteur = _coach.GetCoachRosterSelonRondeJouee(typRoster);
                return string.Format("{0} ({1} de {2})", _coach.NomCoach, coachPreteur.NomRoster, coachPreteur.NomCoach);
            }
        }

        public int IdCoach
        {
            get { return _coach.IdCoach; }
        }
        #endregion
    }
}
