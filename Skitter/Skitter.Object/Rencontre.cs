using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Skitter.Object.Interfaces;

namespace Skitter.Object
{
    /// <summary>
    /// Rencontre entre deux IParticipant. Une liste de Duel a lieu, selon le nombre de Coach dans les IParticipant (Solo ou par Equipe).
    /// Une Rencontre a lieu au cours d'une unique ronde, indiquée par son index
    /// </summary>
    [Serializable]
    public class Rencontre
    {
        #region Variables
        int _idParticipant1;
        int _idParticipant2;

        List<Duel> _lsDuels;
        int _iIndexRonde;
        #endregion

        #region Accesseurs sérialisés
        public int IdParticipant1
        {
            get { return _idParticipant1; }
            set { _idParticipant1 = value; }
        }

        public int IdParticipant2
        {
            get { return _idParticipant2; }
            set { _idParticipant2 = value; }
        }

        public List<Duel> ListeDuels
        {
            get { return _lsDuels; }
            set { _lsDuels = value; }
        }

        public int IndexRonde
        {
            get { return _iIndexRonde; }
            set { _iIndexRonde = value; }
        }
        #endregion

        #region Accesseurs non-sérialisés
        [XmlIgnore]
        [JsonIgnore]
        public int NbVictoiresParticipant1
        {
            get { return _lsDuels.Count(d => d.ResultatCoach1.NbTD > d.ResultatCoach2.NbTD); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int NbVictoiresParticipant2
        {
            get { return _lsDuels.Count(d => d.ResultatCoach2.NbTD > d.ResultatCoach1.NbTD); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int TDEquipe1
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach1.NbTD); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int TDEquipe2
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach2.NbTD); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int SortiesEquipe1
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach1.NbSorties); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int SortiesEquipe2
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach2.NbSorties); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int SortiesVicieusesEquipe1
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach1.NbSortiesVicieuses); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public int SortiesVicieusesEquipe2
        {
            get { return _lsDuels.Sum(d => d.ResultatCoach2.NbSortiesVicieuses); }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string NomParticipant1
        {
            get
            {
                IParticipant participant = Tournoi.ListeParticipants.FirstOrDefault(p => p.IdParticipant == IdParticipant1);
                if (participant != null)
                    return participant.NomParticipant;
                return string.Empty;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string NomParticipant2
        {
            get
            {
                IParticipant participant = Tournoi.ListeParticipants.FirstOrDefault(p => p.IdParticipant == IdParticipant2);
                if (participant != null)
                    return participant.NomParticipant;
                return string.Empty;
            }
        }
        #endregion

        public Rencontre()
        {
            _lsDuels = new List<Duel>();
        }
    }
}
