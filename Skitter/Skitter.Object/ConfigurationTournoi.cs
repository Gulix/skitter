using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.Object
{
    [Serializable]
    public class ConfigurationTournoi
    {
        #region Variables
        eTypeParticipantTournoi _typParticipantTournoi;
        int? _iNbCoachesParEquipe;
        int _iNbRondes;
        #endregion

        #region Type de participants au tournoi
        public enum eTypeParticipantTournoi
        {
            Solo,
            Equipe
        }

        public eTypeParticipantTournoi TypeParticipantTournoi
        {
            get { return _typParticipantTournoi; }
            set { _typParticipantTournoi = value; }
        }
        #endregion

        #region Divers accesseurs
        public int? NbCoachesParEquipe
        {
            get { return _iNbCoachesParEquipe; }
            set { _iNbCoachesParEquipe = value; }
        }

        public int NbRondes
        {
            get { return _iNbRondes; }
            set { _iNbRondes = value; }
        }
        #endregion

        #region Constructeur
        public ConfigurationTournoi()
        {
            _typParticipantTournoi = eTypeParticipantTournoi.Equipe;
            _iNbCoachesParEquipe = 3;
            _iNbRondes = 5;
        }
        #endregion

        
    }
}
