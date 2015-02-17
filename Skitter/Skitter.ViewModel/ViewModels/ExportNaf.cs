using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;
using Skitter.ViewModel.Fonctionnel;

namespace Skitter.ViewModel.ViewModels
{
    public static class ExportNaf
    {
        #region Export Excel

        #region Liste des Coaches avec Rosters joués
        public static void CopierPP_ListeCoachesRosterRonde135()
        {
            CopierPP_ListeCoachesRoster(1);
        }

        public static void CopierPP_ListeCoachesRosterRonde2()
        {
            CopierPP_ListeCoachesRoster(2);
        }

        public static void CopierPP_ListeCoachesRosterRonde4()
        {
            CopierPP_ListeCoachesRoster(4);
        }

        private static void CopierPP_ListeCoachesRoster(int iRonde)
        {
            List<Coach> lsCoaches = Tournoi.ListeCoaches
                .OrderBy(c => IsCoachEtRoster_NAF(c.IdCoach, c.GetRosterSelonRondeJouee(iRonde)) ? 0 : 1)
                .ToList();
            
            string sListe = "Pseudo NAf\tN°NAF\tRoster\tOK NAF ?";
            foreach(Coach c in lsCoaches)
            {
                Roster roster = c.GetRosterSelonRondeJouee(iRonde);

                sListe += string.Format("\n{0}\t{1}\t{2}\t{3}", 
                    c.PseudoNafOuNormal,
                    c.NumeroNAF, 
                    roster.NomNafOuNormal,
                    IsCoachEtRoster_NAF(c.IdCoach, roster) ? "OK" : "PAS OK");
            }

            ExportExcel.CopierDonneesExcelVersPressePapier(sListe);
        }
        #endregion

        #region Liste des rencontres
        public static void CopierPP_ListeRencontres(int iRonde)
        {
            List<Rencontre> lsRencontres = Tournoi.GetRencontresSelonRonde(iRonde);
            List<Duel> lsDuels = lsRencontres.SelectMany(rnc => rnc.ListeDuels).ToList();
            
            lsDuels = lsDuels.OrderBy(d => (IsCoachEtRoster_NAF(d.IdCoach1, Coach.GetRosterSelonRondeJouee(d.IdCoach1, iRonde)) ? 0 : 1)
                                        + (IsCoachEtRoster_NAF(d.IdCoach2, Coach.GetRosterSelonRondeJouee(d.IdCoach2, iRonde)) ? 0 : 1)
                                     )
                             .ToList();

            string sExport = "Type de duel\tPseudo NAf\tTD\tTD\tPseudo NAf";
            foreach(Duel duel in lsDuels)
            {
                bool bCoach1NAF = IsCoachEtRoster_NAF(duel.IdCoach1, Coach.GetRosterSelonRondeJouee(duel.IdCoach1, iRonde));
                bool bCoach2NAF = IsCoachEtRoster_NAF(duel.IdCoach2, Coach.GetRosterSelonRondeJouee(duel.IdCoach2, iRonde));

                string sTypeDuel = string.Empty;
                if (bCoach1NAF && bCoach2NAF)
                    sTypeDuel = "Les 2 NAF";
                else if (bCoach1NAF && !bCoach2NAF)
                    sTypeDuel = "Coach 1 NAF";
                else if (!bCoach1NAF && bCoach2NAF)
                    sTypeDuel = "Coach 2 NAF";
                else
                    sTypeDuel = "Aucun NAF";

                Coach c1 = Tournoi.GetCoach(duel.IdCoach1);
                Coach c2 = Tournoi.GetCoach(duel.IdCoach2);
                sExport += string.Format("\n{4}\t{0}\t{5}\t{2}\t{3}\t{6}\t{1}", 
                    string.IsNullOrEmpty(c1.PseudoNAF) ? c1.NomCoach : c1.PseudoNAF,
                    string.IsNullOrEmpty(c2.PseudoNAF) ? c2.NomCoach : c2.PseudoNAF,
                    duel.ResultatCoach1.NbTD, 
                    duel.ResultatCoach2.NbTD,
                    sTypeDuel,
                    c1.GetRosterSelonRondeJouee(iRonde).NomNafOuNormal, 
                    c2.GetRosterSelonRondeJouee(iRonde).NomNafOuNormal
                    );
            }

            ExportExcel.CopierDonneesExcelVersPressePapier(sExport);
        }
        #endregion

        private static bool IsCoachEtRoster_NAF(int idCoach, Roster roster)
        {
            Coach coach = Tournoi.GetCoach(idCoach);
            
            if ((coach == null) || !coach.NumeroNAF.HasValue || string.IsNullOrEmpty(coach.PseudoNAF)
                || (roster == null) || !roster.IsNaf)
                return false;

            return true;
        }
        #endregion
    }
}
