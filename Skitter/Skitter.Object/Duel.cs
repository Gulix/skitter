using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.Object
{
    /// <summary>
    /// Un Duel est une rencontre entre deux coaches
    /// </summary>
    [Serializable]
    public class Duel
    {
        #region Variables
        public int _idCoach1;
        public int _idCoach2;
        public Resultat _resultatCoach1;
        public Resultat _resultatCoach2;
        #endregion

        #region Accesseurs
        public int IdCoach1
        {
            get { return _idCoach1; }
            set { _idCoach1 = value; }
        }

        public int IdCoach2
        {
            get { return _idCoach2; }
            set { _idCoach2 = value; }
        }

        public Resultat ResultatCoach1
        {
            get { return _resultatCoach1; }
            set { _resultatCoach1 = value; }
        }

        public Resultat ResultatCoach2
        {
            get { return _resultatCoach2; }
            set { _resultatCoach2 = value; }
        }
        #endregion

        public Duel()
        {
            _resultatCoach1 = new Resultat();
            _resultatCoach2 = new Resultat();
        }

        /// <summary>
        /// Le coach indiqué a-t-il pris part à ce Duel ?
        /// </summary>
        public bool APrisPartAuDuel(Coach coach)
        {
            if (coach == null)
                return false;

            return (coach.IdCoach == IdCoach1) || (coach.IdCoach == IdCoach2);
        }
    }
}
