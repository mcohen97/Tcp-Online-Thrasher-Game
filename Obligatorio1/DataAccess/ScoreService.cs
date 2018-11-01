using GameLogic;
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
        public ICollection<ScoreModel> GetLastScores()
        {
            ICollection<Score> topScores = scores.GetScores();
            return topScores.Select(s => BuildScoreDto(s)).ToList();
        }

        private ScoreModel BuildScoreDto(Score s)
        {
            return new ScoreModel() { UserNickname = s.PlayerName, RolePlayed = s.PlayerRole,
                Date = s.Date, Points = s.Points };
        }
    }
}
