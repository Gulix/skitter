using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skitter.Object;

namespace Skitter.ViewModel.ViewModels
{
    public static class MenuManagerViewModel
    {
        public static bool IsMenuConfigurationParticipantsVisible
        {
            get { return Tournoi.TypePhaseTournoi >= Tournoi.eTypePhaseTournoi.ConfigurationParticipants; }
        }
    }
}
