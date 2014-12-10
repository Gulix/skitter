using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels.Rondes
{
    [DataContract]
    public class GenerationFeuilleResultatViewModel
    {
        List<GenerationFeuilleResultatRencontreViewModel> _lsRencontres;
        Coach.eTypeRosterJoue _typRosterJoue;
        int _iNumeroRonde;

        #region DataMembers
        [DataMember]
        public List<GenerationFeuilleResultatRencontreViewModel> Rencontres
        {
            get
            {
                return _lsRencontres;
            }
        }

        [DataMember]
        public string DescriptifRondeCourt
        {
            get
            {
                return string.Empty;
            }
        }

        [DataMember]
        public string DescriptifRondeLong
        {
            get { return string.Empty; }
        }

        #endregion
        
        public Coach.eTypeRosterJoue TypeRosterJoue
        {
            get { return _typRosterJoue; }
        }

        public GenerationFeuilleResultatViewModel(List<Rencontre> lsRencontres, int iNumRonde, Coach.eTypeRosterJoue typRoster)
        {
            _iNumeroRonde = iNumRonde;
            _typRosterJoue = typRoster;
            _lsRencontres = new List<GenerationFeuilleResultatRencontreViewModel>();
            for(int iNumTable = 0; iNumTable < lsRencontres.Count; iNumTable++)
                _lsRencontres.Add(new GenerationFeuilleResultatRencontreViewModel(this, lsRencontres[iNumTable], iNumTable + 1));            
        }
    }

    public class GenerationFeuilleResultatRencontreViewModel
    {
        GenerationFeuilleResultatViewModel _ronde;
        Rencontre _rencontre;
        List<GenerationFeuilleResultatDuelViewModel> _lsDuels;
        int _iNumeroTable;

        #region DataMembers
        [DataMember]
        public List<GenerationFeuilleResultatDuelViewModel> Duels
        {
            get
            {
                return _lsDuels;
            }
        }
        #endregion

        public GenerationFeuilleResultatViewModel RondeVM { get { return _ronde; } }
        public Rencontre Rencontre { get { return _rencontre; } }
        public int NumeroTable { get { return _iNumeroTable; } }

        public GenerationFeuilleResultatRencontreViewModel(GenerationFeuilleResultatViewModel rondeVM, Rencontre rencontre, int numeroTable)
        {
            _ronde = rondeVM;
            _rencontre = rencontre;
            _iNumeroTable = numeroTable;
            // Génération des duels
            _lsDuels = new List<GenerationFeuilleResultatDuelViewModel>();
            _lsDuels.Add(new GenerationFeuilleResultatDuelViewModel(_rencontre.Duel1, this, 1));
            _lsDuels.Add(new GenerationFeuilleResultatDuelViewModel(_rencontre.Duel2, this, 2));
            _lsDuels.Add(new GenerationFeuilleResultatDuelViewModel(_rencontre.Duel3, this, 3));
        }
    }

    public class GenerationFeuilleResultatDuelViewModel
    {
        Duel _duel;
        GenerationFeuilleResultatRencontreViewModel _rencontreVM;
        int _iNumeroMatch;

        #region DataMembers
        [DataMember]
        public string DescriptifRondeLong
        {
            get { return _rencontreVM.RondeVM.DescriptifRondeLong; }
        }

        [DataMember]
        public string NomEquipe1
        {
            get { return _rencontreVM.Rencontre.LibelleEquipe1; }
        }

        [DataMember]
        public string NomEquipe2
        {
            get { return _rencontreVM.Rencontre.LibelleEquipe2; }
        }

        [DataMember]
        public string NomCoach1
        {
            get { return ChaineCoach(_duel.IdCoach1); }
        }

        [DataMember]
        public string NomCoach2
        {
            get { return ChaineCoach(_duel.IdCoach2); }
        }

        [DataMember]
        public string RosterJoue1
        {
            get { return ChaineRosterJoue(_duel.IdCoach1); }
        }

        [DataMember]
        public string RosterJoue2
        {
            get { return ChaineRosterJoue(_duel.IdCoach2); }
        }

        [DataMember]
        public string InformationTable
        {
            get { return string.Format("Table {0}-{1}", _rencontreVM.NumeroTable, _iNumeroMatch); }
        }
        #endregion

        public string ChaineRosterJoue(int idCoach)
        {
            Coach coach = Tournoi.GetCoach(idCoach);
            if (coach == null)
                return string.Empty;

            Coach coachRoster = coach.GetCoachRosterSelonRondeJouee(_rencontreVM.RondeVM.TypeRosterJoue);
            if (coachRoster == null)
                return string.Empty;
            if (coachRoster.IdCoach == idCoach)
                return coachRoster.NomRoster;

            return string.Format("{0} de {1}", coachRoster.NomRoster, coachRoster.NomCoach);
        }

        public string ChaineCoach(int idCoach)
        {
            Coach coach = Tournoi.GetCoach(idCoach);
            if (coach == null)
                return string.Empty;

            if (coach.Equipe.Capitaine.IdCoach == idCoach)
                return coach.NomCoach + " (capitaine)";
            return coach.NomCoach;
        }

        public GenerationFeuilleResultatDuelViewModel(Duel duel, GenerationFeuilleResultatRencontreViewModel rencontreVM, int numeroMatch)
        {
            _duel = duel;
            _rencontreVM = rencontreVM;
            _iNumeroMatch = numeroMatch;
        }
    }
}
