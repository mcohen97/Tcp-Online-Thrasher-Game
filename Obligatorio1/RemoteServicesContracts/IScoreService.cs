using ScoreService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteServicesContracts
{
    public interface IScoreService
    {
        ICollection<ScoreDto> GetLastScores();
    }
}
