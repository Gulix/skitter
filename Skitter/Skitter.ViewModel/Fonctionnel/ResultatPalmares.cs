using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.Fonctionnel
{
    public class ResultatPalmares
    {
        string _sBilan;
        Equipe _equipe;

        public Equipe Equipe
        {
            get { return _equipe; }
        }

        public string Bilan
        {
            get { return _sBilan; }
        }

        public ResultatPalmares(string sScoreBilan, Equipe equipe)
        {
            _equipe = equipe;
            _sBilan = sScoreBilan;
        }
    }
}
