using DataAccessInterface;
using GameLogic;
using RemoteServicesContracts;
using ScoreService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class ScoreService : MarshalByRefObject, IScoreService
    {
        private IScoreRepository scores;
        public ScoreService() {
            scores = ScoresInMemory.instance.Value;
        }
        public ICollection<ScoreDto> GetTopScores()
        {
            ICollection<Score> topScores = scores.GetScores();
            return topScores.Select(s => BuildScoreDto(s)).ToList();
        }

        private ScoreDto BuildScoreDto(Score s)
        {
            return new ScoreDto() { UserNickname = s.PlayerName, RolePlayed = s.PlayerRole,
                Date = s.Date, Points = s.Points };
        }
    }
}
