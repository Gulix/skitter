using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Skitter.Object.Interfaces;

namespace Skitter.Object
{
    [Serializable]
    public class Tournoi
    {
        #region Variables
        List<Equipe> _lsEquipes;
        List<Coach> _lsCoaches;
        eTypePhaseTournoi _typPhaseEnCours;
        Dictionary<int, List<Rencontre>> _dicRencontresParRonde;

        string _sFichier;

        ConfigurationTournoi _configurationTournoi;
        #endregion

        #region Enumération "Phase du tournoi"
        public enum eTypePhaseTournoi
        {
            ConfigurationTournoi = 0,
            ConfigurationParticipants = 1,
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

        #region Accesseurs sérialisés
        public List<Equipe> Equipes
        {
            get { return _lsEquipes; }
            set { _lsEquipes = value; }
        }

        public List<Coach> Coaches
        {
            get { return _lsCoaches; }
            set { _lsCoaches = value; }
        }

        public eTypePhaseTournoi PhaseEnCours
        {
            get { return _typPhaseEnCours; }
            set { _typPhaseEnCours = value; }
        }

        public Dictionary<int, List<Rencontre>> RencontresParRonde
        {
            get { return _dicRencontresParRonde; }
            set { _dicRencontresParRonde = value; }
        }

        public ConfigurationTournoi ConfigurationTournoi
        {
            get { return _configurationTournoi; }
            set { _configurationTournoi = value; }
        }
        #endregion

        private Tournoi()
        {
            _lsEquipes = new List<Equipe>();
            _lsCoaches = new List<Coach>();
            _dicRencontresParRonde = new Dictionary<int, List<Rencontre>>();
            _typPhaseEnCours = eTypePhaseTournoi.ConfigurationTournoi;

            _sFichier = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dragonbowl.xml");

            _configurationTournoi = new ConfigurationTournoi();
        }

        #region Génération des ID
        [XmlIgnore]
        [JsonIgnore]
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
        private int GenererNouvelIdCoach()
        {
            if (!_iNextIdCoach.HasValue)
            {
                if (!Coaches.Any())
                    _iNextIdCoach = 1;
                else
                    _iNextIdCoach = Coaches.Max(c => c.IdCoach) + 1;
            }
            _iNextIdCoach++;
            return _iNextIdCoach.Value - 1;                
        }
        #endregion

        #region Accesseurs non sérialisés
        [XmlIgnore]
        [JsonIgnore]
        public string NomFichier
        {
            get { return _sFichier; }
            set { _sFichier = value; }
        }

        /// <summary>
        /// Participants au tournoi (Liste de coaches ou d'équipes, selon la configuration)
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public List<IParticipant> Participants
        {
            get
            {
                if (_configurationTournoi.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Equipe)
                    return Equipes.Cast<IParticipant>().ToList();
                if (_configurationTournoi.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Solo)
                    return Coaches.Cast<IParticipant>().ToList();

                return new List<IParticipant>();
            }
        }
        #endregion

        #region JSON serialization
        public void EnregistrerJSON(string sPath)
        {
            string sOutput = JsonConvert.SerializeObject(this);
            using (StreamWriter writer = new StreamWriter(sPath))
            {
                writer.Write(sOutput);
            }
        }
        #endregion

        #region Static - Singleton
        #region Une seule instance à gérer
        private static Tournoi _instance;

        private static Tournoi GetInstance()
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

        #region Serialisation via JSON
        public static void ChargerJSON(string sPath)
        {
            string sInput = string.Empty;
            using (StreamReader rd = new StreamReader(sPath))
            {
                sInput = rd.ReadToEnd();
            }

            Tournoi tournoi = JsonConvert.DeserializeObject<Tournoi>(sInput);
            if (tournoi == null)
                throw new Exception("Le type de données ne correspond pas.");

            _instance = tournoi;
            _instance.NomFichier = sPath;
        }

        public static void SauvegarderTournoi()
        {
            GetInstance().EnregistrerJSON(Tournoi.FichierSauvegarde);
        }
        #endregion

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

        #region Accesseurs divers
        public static eTypePhaseTournoi TypePhaseTournoi
        {
            get { return GetInstance().PhaseEnCours; }
            set { GetInstance().PhaseEnCours = value; }
        }

        public static ConfigurationTournoi Configuration
        {
            get { return GetInstance().ConfigurationTournoi; }
        }

        public static string FichierSauvegarde
        {
            get { return GetInstance().NomFichier; }
            set { GetInstance().NomFichier = value; }
        }
        #endregion

        #region Rosters
        public static Roster GetRoster(int idRoster)
        {
            return Roster.GetListeComplete().FirstOrDefault(r => r.IdRoster == idRoster);
        }
        #endregion

        #region Rencontres
        public static List<Rencontre> GetRencontresAvant(int iNumeroRonde)
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            for(int iRonde = 1; iRonde < iNumeroRonde; iRonde++)
                lsRencontres.AddRange(GetInstance().RencontresParRonde[iRonde]);

            return lsRencontres;
        }

        public static List<Rencontre> GetRencontresApres(int iNumeroRonde)
        {
            List<Rencontre> lsRencontres = new List<Rencontre>();
            for (int iRonde = iNumeroRonde; iRonde <= Configuration.NbRondes; iRonde++)
                lsRencontres.AddRange(GetInstance().RencontresParRonde[iRonde]);
            
            return lsRencontres;
        }

        public static List<Rencontre> GetRencontresSelonRonde(int i)
        {
            return GetInstance().RencontresParRonde[i];
        }
        #endregion

        #region Participants
        public static List<IParticipant> ListeParticipants
        {
            get { return GetInstance().Participants; }
        }

        public static IParticipant GetParticipant(int idParticipant)
        {
            return ListeParticipants.FirstOrDefault(p => p.IdParticipant == idParticipant);
        }

        public static string GetNomParticipant(int idParticipant)
        {
            IParticipant participant = GetParticipant(idParticipant);
            if (participant == null)
                return string.Empty;

            return participant.NomParticipant;
        }

        public static int GenererNouveauParticipant()
        {
            if (Configuration.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Equipe)
                return Tournoi.GenererNouvelleEquipe();
            if (Configuration.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Solo)
                return Tournoi.GenererNouveauCoach();

            return 0;
        }

        public static void SupprimerParticipant(int idParticipant)
        {
            if (Configuration.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Equipe)
                GetInstance()._lsEquipes.RemoveAll(e => e.IdEquipe == idParticipant);
            if (Configuration.TypeParticipantTournoi == ConfigurationTournoi.eTypeParticipantTournoi.Solo)
                GetInstance()._lsCoaches.RemoveAll(c => c.IdCoach == idParticipant);
        }
        #endregion

        #region Coaches
        public static List<Coach> ListeCoaches
        {
            get { return GetInstance().Coaches; }
        }

        public static Coach GetCoach(int idCoach)
        {
            return ListeCoaches.FirstOrDefault(c => c.IdCoach == idCoach);
        }

        internal static List<Coach> GetListeCoach(List<int> _lsIdCoaches)
        {
            return GetInstance().Coaches.Where(c => _lsIdCoaches.Any(id => id == c.IdCoach)).ToList();
        }

        internal static int NouvelIdCoach
        {
            get { return GetInstance().GenererNouvelIdCoach(); }
        }

        internal static int GenererNouveauCoach()
        {
            Coach coach = new Coach();
            coach.IdCoach = NouvelIdCoach;
            
            Tournoi.ListeCoaches.Add(coach);

            return coach.IdCoach;
        }
        #endregion

        #region Equipes
        public static List<Equipe> ListeEquipes
        {
            get { return GetInstance()._lsEquipes; }
        }

        public static int GenererNouvelleEquipe()
        {
            Equipe equipe = new Equipe();
            equipe.IdEquipe = Tournoi.GetInstance().NouvelIdEquipe;
            equipe.NomEquipe = "Nouvelle équipe";

            for (int iCoach = 0; iCoach < Tournoi.Configuration.NbCoachesParEquipe; iCoach++)
            {
                int iIDCoach = Tournoi.GenererNouveauCoach();
                equipe.ListeIdCoaches.Add(iIDCoach);                
            }
            GetInstance()._lsEquipes.Add(equipe);

            return equipe.IdEquipe;
        }
        #endregion

        #endregion
    }
}
