using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Skitter.Object
{
    [Serializable]
    public class Tournoi
    {
        #region Variables
        List<Equipe> _lsEquipes;
        eTypePhaseTournoi _typPhaseEnCours;
        List<Rencontre> _lsRencontresRonde1;
        List<Rencontre> _lsRencontresRonde2;
        List<Rencontre> _lsRencontresRonde3;
        List<Rencontre> _lsRencontresRonde4;
        List<Rencontre> _lsRencontresRonde5;

        string _sFichier;
        #endregion

        #region Enumération "Phase du tournoi"
        public enum eTypePhaseTournoi
        {
            Configuration = 1,
            GenerationRonde1 = 2,
            SaisieRonde1 = 3,
            GenerationRonde2 = 4,
            SaisieRonde2 = 5,
            GenerationRonde3 = 6,
            SaisieRonde3 = 7,
            GenerationRonde4 = 8,
            SaisieRonde4 = 9,
            GenerationRonde5 = 10,
            SaisieRonde5 = 11,
            Termine = 12 
        }
        #endregion

        #region Accesseurs
        public List<Equipe> Equipes
        {
            get { return _lsEquipes; }
            set { _lsEquipes = value; }
        }

        public eTypePhaseTournoi PhaseEnCours
        {
            get { return _typPhaseEnCours; }
            set { _typPhaseEnCours = value; }
        }

        public List<Rencontre> RencontresRonde1
        {
            get { return _lsRencontresRonde1; }
            set { _lsRencontresRonde1 = value; }
        }

        public List<Rencontre> RencontresRonde2
        {
            get { return _lsRencontresRonde2; }
            set { _lsRencontresRonde2 = value; }
        }

        public List<Rencontre> RencontresRonde3
        {
            get { return _lsRencontresRonde3; }
            set { _lsRencontresRonde3 = value; }
        }
        
        public List<Rencontre> RencontresRonde4
        {
            get { return _lsRencontresRonde4; }
            set { _lsRencontresRonde4 = value; }
        }

        public List<Rencontre> RencontresRonde5
        {
            get { return _lsRencontresRonde5; }
            set { _lsRencontresRonde5 = value; }
        }
        #endregion

        private Tournoi()
        {
            _lsEquipes = new List<Equipe>();
            _lsRencontresRonde1 = new List<Rencontre>();
            _lsRencontresRonde2 = new List<Rencontre>();
            _lsRencontresRonde3 = new List<Rencontre>();
            _lsRencontresRonde4 = new List<Rencontre>();
            _lsRencontresRonde5 = new List<Rencontre>();
            _typPhaseEnCours = eTypePhaseTournoi.Configuration;

            _sFichier = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dragonbowl.xml");
        }

        #region Génération des ID
        [XmlIgnore]
        public int NouvelIdEquipe 
        {
            get
            {
                if (!_lsEquipes.Any())
                    return 1;
                return _lsEquipes.Max(e => e.IdEquipe) + 1;
            }
        }

        int? _iNextIdCoach = null;

        [XmlIgnore]
        public int NouvelIdCoach
        {
            get
            {
                if (!_iNextIdCoach.HasValue)
                {
                    if (!_lsEquipes.Any())
                        _iNextIdCoach = 1;
                    else 
                        _iNextIdCoach = _lsEquipes.Max(e => Math.Max(e.Capitaine.IdCoach, Math.Max(e.Equipier1.IdCoach, e.Equipier2.IdCoach))) + 1;
                }
                _iNextIdCoach++;
                return _iNextIdCoach.Value - 1;                
            }
        }
        #endregion

        #region Accesseurs non sérialisés
        [XmlIgnore]
        public List<Coach> Coaches
        {
            get { return _lsEquipes.SelectMany(e => new List<Coach>() { e.Capitaine, e.Equipier1, e.Equipier2 }).ToList(); }
        }

        [XmlIgnore]
        public string NomFichier
        {
            get { return _sFichier; }
            set { _sFichier = value; }
        }
        #endregion

        #region XML serialization
        public void EnregistrerXml(string sPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Tournoi));
            using (StreamWriter wr = new StreamWriter(sPath))
            {
                xs.Serialize(wr, this);
            }
        }
        #endregion

        #region Static
        #region Une seule instance à gérer
        private static Tournoi _instance;

        public static Tournoi GetInstance()
        {
            if (_instance == null)
                _instance = new Tournoi();

            return _instance;
        }
        #endregion

        
        public static void ReinitialiserInstance()
        {
            _instance = new Tournoi();
        }

        public static void ChargerXml(string sPath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Tournoi));
            using (StreamReader rd = new StreamReader(sPath))
            {
                Tournoi tournoi = xs.Deserialize(rd) as Tournoi;

                if (tournoi == null)
                    throw new Exception("Le type de données ne correspond pas.");

                _instance = tournoi;
                _instance.NomFichier = sPath;
            }
        }

        #region Récupération des libellés
        public static string GetNomCoach(int idCoach)
        {
            Coach c = GetCoach(idCoach);
            if (c == null)
                return string.Empty;
            return c.NomCoach;
        }

        public static string GetNomEquipe(int idEquipe)
        {
            Equipe e = GetInstance().Equipes.FirstOrDefault(equipe => equipe.IdEquipe == idEquipe);
            if (e == null)
                return string.Empty;
            return e.NomEquipe;
        }
        #endregion

        #region Récupération des éléments selon ID
        public static Coach GetCoach(int idCoach)
        {
            return GetInstance().Coaches.FirstOrDefault(c => c.IdCoach == idCoach);
        }

        public static Roster GetRoster(int idRoster)
        {
            return Roster.GetListeComplete().FirstOrDefault(r => r.IdRoster == idRoster);
        }
        #endregion

        #region Rencontres
        public static List<Rencontre> GetRencontresAvant(int iNumeroRonde)
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            if (iNumeroRonde > 1)
                lsRencontres.AddRange(GetInstance().RencontresRonde1);
            if (iNumeroRonde > 2)
                lsRencontres.AddRange(GetInstance().RencontresRonde2);
            if (iNumeroRonde > 3)
                lsRencontres.AddRange(GetInstance().RencontresRonde3);
            if (iNumeroRonde > 4)
                lsRencontres.AddRange(GetInstance().RencontresRonde4);

            return lsRencontres;
        }

        public static List<Rencontre> GetRencontresApres(int iNumeroRonde)
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            if (iNumeroRonde >= 1)
                lsRencontres.AddRange(GetInstance().RencontresRonde1);
            if (iNumeroRonde >= 2)
                lsRencontres.AddRange(GetInstance().RencontresRonde2);
            if (iNumeroRonde >= 3)
                lsRencontres.AddRange(GetInstance().RencontresRonde3);
            if (iNumeroRonde >= 4)
                lsRencontres.AddRange(GetInstance().RencontresRonde4);
            if (iNumeroRonde >= 5)
                lsRencontres.AddRange(GetInstance().RencontresRonde5);

            return lsRencontres;
        }
        #endregion
        #endregion

        public static List<Rencontre> GetRencontresSelonRonde(int i)
        {
            if (i == 1)
                return Tournoi.GetInstance().RencontresRonde1;
            if (i == 2)
                return Tournoi.GetInstance().RencontresRonde2;
            if (i == 3)
                return Tournoi.GetInstance().RencontresRonde3;
            if (i == 4)
                return Tournoi.GetInstance().RencontresRonde4;
            if (i == 5)
                return Tournoi.GetInstance().RencontresRonde5;

            return new List<Rencontre>();
        }
    }
}
