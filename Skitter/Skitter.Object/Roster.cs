/*
 * Created by SharpDevelop.
 * User: SALON
 * Date: 14/07/2014
 * Time: 19:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;

namespace Skitter.Object
{
    public class Roster
    {
        #region Variables
        int _idRoster;
        string _sNomRoster;
        int _iValeurRoster;
        bool _bIsNaf;
        string _sNomNaf;
        #endregion
        
        #region Propriétés
        public int IdRoster
        {
            get {return _idRoster;}
            set {_idRoster = value;}
        }
        public string NomRoster
        {
            get {return _sNomRoster;}
            set {_sNomRoster = value;}
        }
        public int ValeurRoster
        {
            get {return _iValeurRoster;}
            set {_iValeurRoster = value;}
        }
        public bool IsNaf 
        {
            get { return _bIsNaf; }
            set { _bIsNaf = value; }
        }
        public string NomNaf
        {
            get { return _sNomNaf; }
            set { _sNomNaf = value; }
        }

        public string NomNafOuNormal
        {
            get
            {
                if (string.IsNullOrEmpty(NomNaf))
                    return NomRoster;
                return NomNaf;
            }
        }
        #endregion
        
        
        public Roster()
        {
        }
        
        public static List<Roster> GetListeComplete()
        {
            List<Roster> lsRosters = new List<Roster>();
            
            int i = 1;
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Elfes Sylvains", ValeurRoster = 1, IsNaf = true, NomNaf = "Wood Elves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Morts-vivants", ValeurRoster = 1, IsNaf = true, NomNaf = "Undead" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Nains du chaos", ValeurRoster = 1, IsNaf = true, NomNaf = "Chaos Dwarves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Skavens", ValeurRoster = 1, IsNaf = true, NomNaf = "Skaven" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Hommes-lézards", ValeurRoster = 1, IsNaf = true, NomNaf = "Lizardmen" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Nécromantiques", ValeurRoster = 2, IsNaf = true, NomNaf = "Necromantic" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Amazones", ValeurRoster = 2, IsNaf = true, NomNaf = "Amazons" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Nordiques", ValeurRoster = 2, IsNaf = true, NomNaf = "Norse" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Elfes noirs", ValeurRoster = 2, IsNaf = true, NomNaf = "Dark Elves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Nains", ValeurRoster = 2, IsNaf = true, NomNaf = "Dwarves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Khemri", ValeurRoster = 3, IsNaf = true, NomNaf = "Khemri" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Chaos", ValeurRoster = 3, IsNaf = true, NomNaf = "Chaos" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Elfes pros", ValeurRoster = 3, IsNaf = true, NomNaf = "Elves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Humains", ValeurRoster = 3, IsNaf = true, NomNaf = "Humans" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Orques", ValeurRoster = 3, IsNaf = true, NomNaf = "Orc" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Khorne", ValeurRoster = 4, IsNaf = false });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Slanns", ValeurRoster = 4, IsNaf = true, NomNaf = "Slann" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Nurgle", ValeurRoster = 4, IsNaf = true, NomNaf = "Nurgle's Rotters" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Hauts-Elfes", ValeurRoster = 4, IsNaf = true, NomNaf = "High Elves" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Pacte du Chaos", ValeurRoster = 4, IsNaf = true, NomNaf = "Chaos Pact" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Halflings", ValeurRoster = 5, IsNaf = true, NomNaf = "Halflings" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Gobelins", ValeurRoster = 5, IsNaf = true, NomNaf = "Goblins" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Ogres", ValeurRoster = 5, IsNaf = true, NomNaf = "Ogres" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Vampires", ValeurRoster = 5, IsNaf = true, NomNaf = "Vampires" });
            lsRosters.Add(new Roster() { IdRoster = i++, NomRoster = "Bas-Fonds", ValeurRoster = 5, IsNaf = true, NomNaf = "Underworld" });
        
            return lsRosters;
        }
    }
}
