/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 01/05/2014
 * Time: 18:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Skitter.Object.Interfaces;

namespace Skitter.Object
{
	/// <summary>
	/// Informations sur un coach
	/// </summary>
	[Serializable]
	public class Coach : IParticipant
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
        [JsonIgnore]
        private Roster Roster
        {
            get { return Roster.GetListeComplete().FirstOrDefault(r => r.IdRoster == IdRoster); }
        }

        [XmlIgnore]
        [JsonIgnore]
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
        [JsonIgnore]
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
        [JsonIgnore]
        public Equipe Equipe
        {
            get { return Tournoi.ListeEquipes.FirstOrDefault(e => e.ContientIdCoach(this.IdCoach)); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string PseudoNafOuNormal
        {
            get
            {
                if (string.IsNullOrEmpty(PseudoNAF))
                    return NomCoach;
                return PseudoNAF;
            }
        }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Chargement par sérialisation
        /// </summary>
        internal Coach()
		{
            
		}
        #endregion

        #region Roster joué selon la ronde
        public Roster GetRosterSelonRondeJouee(eTypeRosterJoue typRoster)
        {
            Coach rosterCoachJoue = GetCoachRosterSelonRondeJouee(typRoster);
            if (rosterCoachJoue == null)
                return null;
            return rosterCoachJoue.Roster;
        }

        public Roster GetRosterSelonRondeJouee(int iRonde)
        {
            return GetRosterSelonRondeJouee(RosterJoueSelonRonde(iRonde));
        }

        public static Roster GetRosterSelonRondeJouee(int idCoach, int iRonde)
        {
            eTypeRosterJoue typRoster = RosterJoueSelonRonde(iRonde);
            Coach coach = Tournoi.GetCoach(idCoach);
            return coach.GetRosterSelonRondeJouee(typRoster);
        }

        public Coach GetCoachRosterSelonRondeJouee(eTypeRosterJoue typRoster)
        {
            if (typRoster == eTypeRosterJoue.RosterClassique)
                return this;

            if (Equipe == null)
                return null;

            if (typRoster == eTypeRosterJoue.EchangeRonde2)
            {
                for (int iCoach = 0; iCoach < Equipe.ListeIdCoaches.Count; iCoach++)
                {
                    if (Equipe.ListeIdCoaches[iCoach] == IdCoach)
                    {
                        if (iCoach >= Equipe.ListeIdCoaches.Count)
                            return Tournoi.GetCoach(Equipe.ListeIdCoaches[0]);
                        return Tournoi.GetCoach(Equipe.ListeIdCoaches[iCoach + 1]);
                    }
                }
            }

            if (typRoster == eTypeRosterJoue.EchangeRonde4)
            {
                for (int iCoach = 0; iCoach < Equipe.ListeIdCoaches.Count; iCoach++)
                {
                    if (Equipe.ListeIdCoaches[iCoach] == IdCoach)
                    {
                        if (iCoach == 0)
                            return Tournoi.GetCoach(Equipe.ListeIdCoaches.Last());
                        return Tournoi.GetCoach(Equipe.ListeIdCoaches[iCoach - 1]);
                    }
                }
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

        #region IParticipant Members

        [XmlIgnore]
        [JsonIgnore]
        public string NomParticipant
        {
            get { return NomCoach; }
            set { NomCoach = value; }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int IdParticipant
        {
            get { return IdCoach; }
        }

        [XmlIgnore]
        [JsonIgnore]
        public List<Coach> ListeCoaches
        {
            get { return new List<Coach>() { this }; }
        }

        public string GetErreurs()
        {
            if (string.IsNullOrEmpty(NomParticipant))
                return "Le nom du coach est nécessaire.";
            if (Roster == null)
                return "Le roster doit être sélectionné.";

            return string.Empty;
        }

        #endregion
    }
}
