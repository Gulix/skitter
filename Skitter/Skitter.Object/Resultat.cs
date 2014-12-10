using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.Object
{
    /// <summary>
    /// Résultats obtenus pour un match, par un coach
    /// </summary>
    [Serializable]
    public class Resultat
    {
        #region Variables
        int _iTD;
        int _iSorties;
        int _iSortiesVicieuses;
        #endregion

        #region Accesseurs
        public int NbTD
        {
            get { return _iTD; }
            set { _iTD = value; }
        }

        public int NbSorties
        {
            get { return _iSorties; }
            set { _iSorties = value; }
        }

        public int NbSortiesVicieuses
        {
            get { return _iSortiesVicieuses; }
            set { _iSortiesVicieuses = value; }
        }
        #endregion

        public Resultat()
        {

        }
    }
}
