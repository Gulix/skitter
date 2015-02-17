/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 01/05/2014
 * Time: 18:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using Skitter.Object.Interfaces;

namespace Skitter.Object
{
	/// <summary>
	/// Description d'une équipe, qui est constituée de plusieurs Coach (via ConstitutionEquipe)
	/// </summary>
    [Serializable]
	public class Equipe : IParticipant
	{
		#region Variables
		int _idEquipe;
		string _sNomEquipe;
		string _sLigueAffiliee;
        string _sHymne;

        List<int> _lsIdCoaches;
		#endregion
		
		#region Accesseurs sérialisés
		public int IdEquipe
		{
			get {return _idEquipe;}
			set {_idEquipe =value;}
		}
		public string NomEquipe
		{
			get {return _sNomEquipe;}
			set {_sNomEquipe = value;}
		}
		
        public string LigueAffiliee
		{
			get {return _sLigueAffiliee;}
			set {_sLigueAffiliee = value;}
		}
		
		public List<int> ListeIdCoaches
		{
		    get {return _lsIdCoaches;}
            set { _lsIdCoaches = value; }
		}
		
        public string HymneEquipe
        {
            get { return _sHymne; }
            set { _sHymne = value; }
        }
		#endregion

        #region Accesseurs non-sérialisés
        [XmlIgnore]
        public int ValeurRosterEquipe
        {
            get { return Tournoi.ListeCoaches.Where(c => _lsIdCoaches.Any(id => id == c.IdCoach)).Sum(c => c.ValeurRoster); }
        }
        #endregion
        /// <summary>
        /// Chargement via sérialisation
        /// </summary>
        internal Equipe()
		{
            _lsIdCoaches = new List<int>();
		}

        internal bool ContientIdCoach(int idCoach)
        {
            return _lsIdCoaches.Any(id => id == idCoach);
        }

        #region IParticipant Members

        public string NomParticipant
        {
            get { return NomEquipe; }
            set { NomEquipe = value; }
        }

        public int IdParticipant
        {
            get { return IdEquipe; }
        }

        public List<Coach> ListeCoaches
        {
            get { return Tournoi.GetListeCoach(_lsIdCoaches); }
        }

        public string GetErreurs()
        {
            return string.Empty;
        }

        #endregion
    }
}
