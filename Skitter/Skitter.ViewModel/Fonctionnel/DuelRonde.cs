using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.Fonctionnel
{
    public class DuelRonde
    {
        Duel _duel;
        int _iRonde;

        public Duel Duel { get { return _duel; } }
        public int NumeroRonde { get { return _iRonde; } }

        public DuelRonde(Duel duel, int numeroRonde)
        {
            _duel = duel;
            _iRonde = numeroRonde;
        }

        public bool IsDuelIdentique(DuelRonde duel)
        {
            return ((Duel.IdCoach1 == duel.Duel.IdCoach1) && (Duel.IdCoach2 == duel.Duel.IdCoach2))
                || ((Duel.IdCoach1 == duel.Duel.IdCoach2) && (Duel.IdCoach2 == duel.Duel.IdCoach1));
        }

        public static DuelRonde GetDuelCoach(Rencontre rencontre, Coach coach, int iNumeroRonde)
        {
            if ((rencontre.Duel1.IdCoach1 == coach.IdCoach) || (rencontre.Duel1.IdCoach2 == coach.IdCoach))
                return new DuelRonde(rencontre.Duel1, iNumeroRonde);
            else if ((rencontre.Duel2.IdCoach1 == coach.IdCoach) || (rencontre.Duel2.IdCoach2 == coach.IdCoach))
                return new DuelRonde(rencontre.Duel2, iNumeroRonde);
            if ((rencontre.Duel3.IdCoach1 == coach.IdCoach) || (rencontre.Duel3.IdCoach2 == coach.IdCoach))
                return new DuelRonde(rencontre.Duel3, iNumeroRonde);

            return null;
        }

        public static Roster GetRosterCoach(DuelRonde duel, Coach coach)
        {
            return ((duel.Duel.IdCoach1 == coach.IdCoach) ? Tournoi.GetCoach(duel.Duel.IdCoach1) : Tournoi.GetCoach(duel.Duel.IdCoach2))
                .GetRosterSelonRondeJouee(Coach.RosterJoueSelonRonde(duel.NumeroRonde));
        }

        public static Roster GetRosterAdversaire(DuelRonde duel, Coach coach)
        {
            return ((duel.Duel.IdCoach1 == coach.IdCoach) ? Tournoi.GetCoach(duel.Duel.IdCoach2) : Tournoi.GetCoach(duel.Duel.IdCoach1))
                .GetRosterSelonRondeJouee(Coach.RosterJoueSelonRonde(duel.NumeroRonde));
        }
    }
}
