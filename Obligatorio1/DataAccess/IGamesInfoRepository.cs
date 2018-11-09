using GameLogic;
using System.Collections.Generic;

namespace DataAccessInterface
{
    public interface IGamesInfoRepository
    {
        void AddScore(Score aScore);

        ICollection<Score> GetScores();

        void AddGameReport(GameReport report);

        ICollection<GameReport> GetGameReports();
    }
}
