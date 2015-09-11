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
    }
}
