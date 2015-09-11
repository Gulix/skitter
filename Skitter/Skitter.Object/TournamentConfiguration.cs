using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Skitter.Object
{
    [Serializable]
    public class TournamentConfiguration
    {
        #region Constructors
        public TournamentConfiguration()
        {
            _lsRosters = new List<Roster>();
        }
        #endregion

        #region List of Rosters
        List<Roster> _lsRosters;

        public List<Roster> Rosters
        {
            get { return _lsRosters; }
            set { _lsRosters = value; }
        }

        [XmlIgnore]
        public int NewRosterID
        {
            get
            {
                if (!_lsRosters.Any())
                    return 1;
                return _lsRosters.Max(r => r.IdRoster) + 1;
            }
        }

        public Roster AddNewRoster()
        {
            Roster roster = new Roster();
            roster.IdRoster = NewRosterID;
            _lsRosters.Add(roster);

            return roster;
        }

        public Roster AddNewRoster(string sName, string sNAF, int iRank)
        {
            Roster roster = new Roster();
            roster.IdRoster = NewRosterID;
            roster.NomRoster = sName;
            roster.NomNaf = sNAF;
            roster.IsNaf = !string.IsNullOrEmpty(roster.NomNaf);
            roster.ValeurRoster = iRank;
            _lsRosters.Add(roster);

            return roster;
        }
        #endregion
    }
}
