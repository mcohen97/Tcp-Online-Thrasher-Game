using RemoteServicesContracts;
using ScoreService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Properties
{
    public class ScoreService : MarshalByRefObject, IScoreService
    {
        private IScoreRepository scores;
        public ScoreService() {
            scores = ScoresInMemory.instance.Value;
        }
        public ICollection<Score> GetLastScores()
        {
            return scores.GetScores();
        }
    }
}
