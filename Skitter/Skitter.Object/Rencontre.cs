using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Skitter.Object
{
    /// <summary>
    /// Rencontre entre deux triplettes
    /// </summary>
    [Serializable]
    public class Rencontre
    {
        #region Variables
        int _idEquipe1;
        int _idEquipe2;
        Duel _duel1;
        Duel _duel2;
        Duel _duel3;

        bool _bScoreRegistered;
        #endregion

        #region Accesseurs
        public int IdEquipe1
        {
            get { return _idEquipe1; }
            set { _idEquipe1 = value; }
        }

        public int IdEquipe2
        {
            get { return _idEquipe2; }
            set { _idEquipe2 = value; }
        }

        public Duel Duel1
        {
            get { return _duel1; }
            set { _duel1 = value; }
        }

        public Duel Duel2
        {
            get { return _duel2; }
            set { _duel2 = value; }
        }

        public Duel Duel3
        {
            get { return _duel3; }
            set { _duel3 = value; }
        }

        public bool IsScoreRegistered
        {
            get { return _bScoreRegistered; }
            set { _bScoreRegistered = value; }
        }
        #endregion

        #region Accesseurs non-sérialisés
        [XmlIgnore]
        public int ScoreEquipe1
        {
            get
            {
                return ((Duel1.ResultatCoach1.NbTD > Duel1.ResultatCoach2.NbTD) ? 1 : 0)
                    + ((Duel2.ResultatCoach1.NbTD > Duel2.ResultatCoach2.NbTD) ? 1 : 0)
                    + ((Duel3.ResultatCoach1.NbTD > Duel3.ResultatCoach2.NbTD) ? 1 : 0);
            }
        }

        [XmlIgnore]
        public int ScoreEquipe2
        {
            get
            {
                return ((Duel1.ResultatCoach1.NbTD < Duel1.ResultatCoach2.NbTD) ? 1 : 0)
                    + ((Duel2.ResultatCoach1.NbTD < Duel2.ResultatCoach2.NbTD) ? 1 : 0)
                    + ((Duel3.ResultatCoach1.NbTD < Duel3.ResultatCoach2.NbTD) ? 1 : 0);
            }
        }

        [XmlIgnore]
        public int TDEquipe1
        {
            get { return Duel1.ResultatCoach1.NbTD + Duel2.ResultatCoach1.NbTD + Duel3.ResultatCoach1.NbTD; }
        }

        [XmlIgnore]
        public int TDEquipe2
        {
            get { return Duel1.ResultatCoach2.NbTD + Duel2.ResultatCoach2.NbTD + Duel3.ResultatCoach2.NbTD; }
        }

        [XmlIgnore]
        public int SortiesEquipe1
        {
            get { return Duel1.ResultatCoach1.NbSorties + Duel2.ResultatCoach1.NbSorties + Duel3.ResultatCoach1.NbSorties; }
        }

        [XmlIgnore]
        public int SortiesEquipe2
        {
            get { return Duel1.ResultatCoach2.NbSorties + Duel2.ResultatCoach2.NbSorties + Duel3.ResultatCoach2.NbSorties; }
        }

        [XmlIgnore]
        public int SortiesVicieusesEquipe1
        {
            get { return Duel1.ResultatCoach1.NbSortiesVicieuses + Duel2.ResultatCoach1.NbSortiesVicieuses + Duel3.ResultatCoach1.NbSortiesVicieuses; }
        }

        [XmlIgnore]
        public int SortiesVicieusesEquipe2
        {
            get { return Duel1.ResultatCoach2.NbSortiesVicieuses + Duel2.ResultatCoach2.NbSortiesVicieuses + Duel3.ResultatCoach2.NbSortiesVicieuses; }
        }

        [XmlIgnore]
        public string LibelleEquipe1
        {
            get
            {
                Equipe equipe = Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.IdEquipe == IdEquipe1);
                if (equipe != null)
                    return equipe.NomEquipe;
                return string.Empty;
            }
        }

        [XmlIgnore]
        public string LibelleEquipe2
        {
            get
            {
                Equipe equipe = Tournoi.GetInstance().Equipes.FirstOrDefault(e => e.IdEquipe == IdEquipe2);
                if (equipe != null)
                    return equipe.NomEquipe;
                return string.Empty;
            }
        }
        #endregion

        public Rencontre()
        {

        }
    }
}
