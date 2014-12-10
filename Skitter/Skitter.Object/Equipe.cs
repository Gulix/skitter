/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 01/05/2014
 * Time: 18:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml.Serialization;

namespace Skitter.Object
{
	/// <summary>
	/// Description d'une équipe, qui est constituée de plusieurs Coach (via ConstitutionEquipe)
	/// </summary>
    [Serializable]
	public class Equipe
	{
		#region Variables
		int _idEquipe;
		string _sNomEquipe;
		string _sLigueAffiliee;
        string _sHymne;
		
		Coach _capitaine;
		Coach _equipier1;
		Coach _equipier2;
		#endregion
		
		#region Accesseurs
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
		
		public Coach Capitaine
		{
		    get {return _capitaine;}
		    set {_capitaine = value;}
		}
		public Coach Equipier1
		{
		    get {return _equipier1;}
		    set {_equipier1 = value;}
		}
		public Coach Equipier2
		{
		    get {return _equipier2;}
		    set {_equipier2 = value;}
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
            get { return _capitaine.ValeurRoster + _equipier1.ValeurRoster + _equipier2.ValeurRoster; }
        }
        #endregion

        public Equipe()
		{
            _idEquipe = Tournoi.GetInstance().NouvelIdEquipe;

            _capitaine = new Coach();
            _equipier1 = new Coach();
            _equipier2 = new Coach();
		}

        internal bool ContientIdCoach(int idCoach)
        {
            return (_capitaine.IdCoach == idCoach) || (_equipier1.IdCoach == idCoach) || (_equipier2.IdCoach == idCoach);
        }
    }
}
