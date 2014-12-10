using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Skitter.Object;

namespace Skitter.ViewModel.Fonctionnel
{
    class ControleRencontreCoach
    {
        public enum eTypeControle
        {
            Erreur,
            OK,
            RosterIdentique,
            RosterDejaAffronteUneFois,
            RosterDejaAffrontePlusUneFois,
            OppositionDejaJoueeUneFois,
            OppositionDejaJoueePlusUneFois,
            DuelDejaJoue
        }

        public static string GetTooltipSelonControle(Coach coach, Rencontre rencontre, int iRondeActuelle, Coach.eTypeRosterJoue typRosterRonde)
        {
            eTypeControle typControle = ControlerCoach(coach, rencontre, iRondeActuelle, typRosterRonde);
            
            if (typControle == eTypeControle.DuelDejaJoue)
                return "Ces 2 adversaires se sont déjà affrontés.";
            if (typControle == eTypeControle.OppositionDejaJoueePlusUneFois)
                return "Ce coach a déjà joué cette opposition plus d'une fois.";
            if (typControle == eTypeControle.OppositionDejaJoueeUneFois)
                return "Ce coach a déjà joué cette oppositon une fois.";
            if (typControle == eTypeControle.RosterDejaAffrontePlusUneFois)
                return "Ce coach a déjà affronté ce roster plus d'une fois.";
            if (typControle == eTypeControle.RosterDejaAffronteUneFois)
                return "Ce coach a déjà affronté ce roster une fois.";
            if (typControle == eTypeControle.RosterIdentique)
                return "Les rosters de ces coaches sont identiques.";
            if (typControle == eTypeControle.Erreur)
                return "Une erreur est survenue dans le contrôle.";

            return null;
        }

        public static SolidColorBrush GetBrushSelonControle(Coach coach, Rencontre rencontre, int iRondeActuelle, Coach.eTypeRosterJoue typRosterRonde)
        {
            eTypeControle typControle = ControlerCoach(coach, rencontre, iRondeActuelle, typRosterRonde);
            // Rouge si adversaire déjà affronté
            if (typControle == eTypeControle.DuelDejaJoue)
                return new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            // Bleu si opposition (roster vs roster) déjà jouée
            if (typControle == eTypeControle.OppositionDejaJoueePlusUneFois)
                return new SolidColorBrush(Color.FromArgb(255, 31, 143, 255));
            if (typControle == eTypeControle.OppositionDejaJoueeUneFois)
                return new SolidColorBrush(Color.FromArgb(255, 153, 204, 255));
            // Orange si roster déjà joué
            if (typControle == eTypeControle.RosterDejaAffronteUneFois)
                return new SolidColorBrush(Color.FromArgb(255, 255, 202, 122));
            if (typControle == eTypeControle.RosterDejaAffrontePlusUneFois)
                return new SolidColorBrush(Color.FromArgb(255, 255, 153, 0));
            // Violet si mêmes rosters
            if (typControle == eTypeControle.RosterIdentique)
                return new SolidColorBrush(Color.FromArgb(255, 204, 153, 204));
            // Vert si erreur
            if (typControle == eTypeControle.Erreur)
                return new SolidColorBrush(Color.FromArgb(255, 153, 255, 51));

            return Brushes.Transparent;
        }

        public static eTypeControle ControlerCoach(Coach coach, Rencontre rencontre, int iRondeActuelle, Coach.eTypeRosterJoue typRosterRonde)
        {
            // On récupère les rencontres précédentes
            int idEquipe = coach.Equipe.IdEquipe;
            List<List<Rencontre>> lsRencontresParRonde = new List<List<Rencontre>>();
            if (iRondeActuelle > 1)
                lsRencontresParRonde.Add(Tournoi.GetInstance().RencontresRonde1.Where(r => (r.IdEquipe1 == idEquipe) || (r.IdEquipe2 == idEquipe)).ToList());
            if (iRondeActuelle > 2)
                lsRencontresParRonde.Add(Tournoi.GetInstance().RencontresRonde2.Where(r => (r.IdEquipe1 == idEquipe) || (r.IdEquipe2 == idEquipe)).ToList());
            if (iRondeActuelle > 3)
                lsRencontresParRonde.Add(Tournoi.GetInstance().RencontresRonde3.Where(r => (r.IdEquipe1 == idEquipe) || (r.IdEquipe2 == idEquipe)).ToList());
            if (iRondeActuelle > 4)
                lsRencontresParRonde.Add(Tournoi.GetInstance().RencontresRonde4.Where(r => (r.IdEquipe1 == idEquipe) || (r.IdEquipe2 == idEquipe)).ToList());

            // On récupère les duels auxquels à participé le coach
            List<DuelRonde> lsDuelsParRonde = new List<DuelRonde>();
            for (int iRonde = 0; iRonde < lsRencontresParRonde.Count; iRonde++)
            {
                foreach (Rencontre rnc in lsRencontresParRonde[iRonde])
                {
                    lsDuelsParRonde.Add(DuelRonde.GetDuelCoach(rnc, coach, iRonde + 1));
                }
            }

            // On analyse les duels par rapport au duel actuel
            DuelRonde duelActuel = DuelRonde.GetDuelCoach(rencontre, coach, iRondeActuelle);
            if (duelActuel == null)
            {
                Debug.WriteLine("Duel actuel introuvable dans la rencontre");
                return eTypeControle.Erreur;
            }

            // Duel déjà joué ?
            if (lsDuelsParRonde.Any(d => d.IsDuelIdentique(duelActuel)))
                return eTypeControle.DuelDejaJoue;

            // Opposition (Roster vs Roster) déjà jouée ?
            // Roster déjà affronté (pas forcément en jouant le même roster)
            int iCountOpposition = NbDuelsMemeOpposition(coach, duelActuel, lsDuelsParRonde);
            int iCountAffrontement = NbMatchesContreRoster(coach, duelActuel, lsDuelsParRonde);
            
            if (iCountOpposition > 1)
                return eTypeControle.OppositionDejaJoueePlusUneFois;
            else if (iCountAffrontement > 1)
                return eTypeControle.RosterDejaAffrontePlusUneFois;
            else if (iCountOpposition == 1)
                return eTypeControle.OppositionDejaJoueeUneFois;
            else if (iCountAffrontement == 1)
                return eTypeControle.RosterDejaAffronteUneFois;

            // Mêmes rosters
            Roster rosterCoach = DuelRonde.GetRosterCoach(duelActuel, coach);
            Roster rosterAdversaire = DuelRonde.GetRosterAdversaire(duelActuel, coach);
            if (rosterCoach.IdRoster == rosterAdversaire.IdRoster)
                return eTypeControle.RosterIdentique;

            // Contrôle OK
            return eTypeControle.OK;
        }

        /// <summary>
        /// Calcule le nombre de fois que le coach a affronté le même roster que celui du match contrôlé
        /// </summary>
        /// <param name="coach">Coach contrôlé</param>
        /// <param name="duelCourant">Duel de la ronde contrôlée</param>
        /// <param name="lsDuelsJoues">Liste des duels déjà joués</param>
        /// <returns>Nombre d'oppositions contre le même roster</returns>
        private static int NbMatchesContreRoster(Coach coach, DuelRonde duelCourant, List<DuelRonde> lsDuelsJoues)
        {
            Roster rosterAdversaire = DuelRonde.GetRosterAdversaire(duelCourant, coach);
            int precedentsAffrontements = 0;
            foreach (DuelRonde duel in lsDuelsJoues)
            {
                Roster rosterAdversaireDuel = DuelRonde.GetRosterAdversaire(duel, coach);
                if (rosterAdversaire.IdRoster == rosterAdversaireDuel.IdRoster)
                    precedentsAffrontements++;
            }

            return precedentsAffrontements;
        }

        /// <summary>
        /// Calcule le nombre d'oppositions identiques (Même Roster joué vs Même roster affronté) qu'a déjà joué le Coach
        /// </summary>
        /// <param name="coach">Coach contrôlé</param>
        /// <param name="duelCourant">Duel de la ronde contrôlée</param>
        /// <param name="lsDuelsJoues">Liste des duels déjà joués</param>
        /// <returns>Nombre d'oppositions identiques trouvées</returns>
        private static int NbDuelsMemeOpposition(Coach coach, DuelRonde duelCourant, List<DuelRonde> lsDuelsJoues)
        {
            // Rosters joués pour le duel courant
            Roster rosterCoach = DuelRonde.GetRosterCoach(duelCourant, coach);
            Roster rosterAdversaire = DuelRonde.GetRosterAdversaire(duelCourant, coach);

            int iMemeOpposition = 0;
            foreach (DuelRonde duel in lsDuelsJoues)
            {
                Roster rosterCoachDuel = DuelRonde.GetRosterCoach(duel, coach);
                Roster rosterAdversaireDuel = DuelRonde.GetRosterAdversaire(duel, coach);

                if ((rosterCoachDuel.IdRoster == rosterCoach.IdRoster) && (rosterAdversaireDuel.IdRoster == rosterAdversaire.IdRoster))
                    iMemeOpposition++;
            }

            return iMemeOpposition;
        }
    }
}
