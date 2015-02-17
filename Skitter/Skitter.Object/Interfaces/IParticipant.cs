using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skitter.Object.Interfaces
{
    /// <summary>
    /// Un IParticipant est une entité participant à un tournoi, et apparaissant dans le classement.
    /// Globalement, il s'agit de pourvoir une classe parente pour gérer les participants à un tournoi solo ou par équipe.
    /// </summary>
    public interface IParticipant
    {
        string NomParticipant { get; set; }
        int IdParticipant { get; }
        List<Coach> ListeCoaches { get; }
        string GetErreurs();
    }
}
