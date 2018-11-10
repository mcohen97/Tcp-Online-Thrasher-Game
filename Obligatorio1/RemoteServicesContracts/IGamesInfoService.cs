using ActionResults;
using GamesInfoService;


namespace RemoteServicesContracts
{
    public interface IGamesInfoService
    {
        ScoreListActionResult GetTopScores();

        GamesStatisticsActionResult GetLastGamesStatistics();
    }
}
