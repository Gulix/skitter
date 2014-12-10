/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 01/05/2014
 * Time: 18:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Skitter.Object
{
	/// <summary>
	/// Informations sur un coach
	/// </summary>
	[Serializable]
	public class Coach
	{
		#region Variables
		int _idCoach;
		string _sNomCoach;
		string _sNomEquipe;
		int? _iNumeroNAF;
        string _sPseudoNAF;
		int _idRoster;
        #endregion
		
		#region Accesseurs
		public int IdCoach
		{
			get {return _idCoach;}
			set {_idCoach = value;}
		}
		public string NomCoach
		{
			get {return _sNomCoach;}
			set {_sNomCoach = value;}
		}
		public string NomEquipe
		{
			get {return _sNomEquipe;}
			set {_sNomEquipe = value;}
		}
		public int? NumeroNAF
		{
			get {return _iNumeroNAF;}
			set {_iNumeroNAF = value;}
		}
        public string PseudoNAF
        {
            get { return _sPseudoNAF; }
            set { _sPseudoNAF = value; }
        }
		public int IdRoster
		{
			get {return _idRoster;}
			set {_idRoster=value;}
		}
		#endregion

        #region Accesseurs non-sérialisés
        [XmlIgnore]
        private Roster Roster
        {
            get { return Roster.GetListeComplete().FirstOrDefault(r => r.IdRoster == IdRoster); }
        }

        [XmlIgnore]
        public int ValeurRoster
        {
            get
            {
                if (Roster == null)
                    return 0;
                return Roster.ValeurRoster;
            }
        }

        [XmlIgnore]
        public string NomRoster
        {
            get
            {
                if (Roster == null)
                    return string.Empty;
                return Roster.NomRoster;
            }
        }

        [XmlIgnore]
        public Equipe Equipe
        {
            get { return Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.ContientIdCoach(this.IdCoach)); }
        }
        #endregion

        public Coach()
		{
            _idCoach = Tournoi.GetInstance().NouvelIdCoach;
		}

        #region Roster joué selon la ronde
        public Roster GetRosterSelonRondeJouee(eTypeRosterJoue typRoster)
        {
            Coach rosterCoachJoue = GetCoachRosterSelonRondeJouee(typRoster);
            if (rosterCoachJoue == null)
                return null;
            return rosterCoachJoue.Roster;
        }

        public Coach GetCoachRosterSelonRondeJouee(eTypeRosterJoue typRoster)
        {
            if (typRoster == eTypeRosterJoue.RosterClassique)
                return this;

            if (Equipe == null)
                return null;

            if (typRoster == eTypeRosterJoue.EchangeRonde2)
            {
                if (Equipe.Capitaine.IdCoach == IdCoach)
                    return Equipe.Equipier1;
                if (Equipe.Equipier1.IdCoach == IdCoach)
                    return Equipe.Equipier2;
                if (Equipe.Equipier2.IdCoach == IdCoach)
                    return Equipe.Capitaine;
            }

            if (typRoster == eTypeRosterJoue.EchangeRonde4)
            {
                if (Equipe.Capitaine.IdCoach == IdCoach)
                    return Equipe.Equipier2;
                if (Equipe.Equipier1.IdCoach == IdCoach)
                    return Equipe.Capitaine;
                if (Equipe.Equipier2.IdCoach == IdCoach)
                    return Equipe.Equipier1;
            }

            return null;
        }

        public enum eTypeRosterJoue
        {
            RosterClassique,
            EchangeRonde2,
            EchangeRonde4
        }

        public static eTypeRosterJoue RosterJoueSelonRonde(int iNumeroRonde)
        {
            if (iNumeroRonde == 2)
                return eTypeRosterJoue.EchangeRonde2;
            if (iNumeroRonde == 4)
                return eTypeRosterJoue.EchangeRonde4;

            return eTypeRosterJoue.RosterClassique;
        }
        #endregion
    }
}
