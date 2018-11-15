using DataAccessInterface;
using GameLogic;
using RemoteServicesContracts;
using GamesInfoService;
using System;
using System.Collections.Generic;
using System.Linq;
using ActionResults;
using UserCRUDServiceContract;

namespace DataAccess
{
    public class GameInfoService : MarshalByRefObject, IGamesInfoService
    {
        private IGamesInfoRepository scores;
        public GameInfoService() {
            scores = GamesInfoInMemory.instance.Value;
        }

        public ScoreListActionResult GetTopScores()
        {
            ICollection<Score> topScores = scores.GetScores();
            ICollection<ScoreDto> dtos = topScores.Select(s => BuildScoreDto(s)).ToList();
            ScoreListActionResult result = new ScoreListActionResult()
            {
                Success = true,
                Message = "Top scores list",
                ScoreList =dtos
            };
            return result;
        }

        private ScoreDto BuildScoreDto(Score s)
        {
            return new ScoreDto()
            {
                UserNickname = s.PlayerName,
                RolePlayed = s.PlayerRole.ToString(),
                Date = s.Date,
                Points = s.Points
            };
        }

        public GamesStatisticsActionResult GetLastGamesStatistics()
        {
            ICollection<GameReport> statistics = scores.GetGameReports();
            ICollection<GameReportDto> dtos = statistics.Select(gr => BuildStatisticDto(gr)).ToList();
            GamesStatisticsActionResult result = new GamesStatisticsActionResult()
            {
                Success = true,
                Message = "Last 10 games statistics",
                GamesStatistics = dtos
            };
            return result;
        }

        private GameReportDto BuildStatisticDto(GameReport report)
        {
            ICollection<PlayerFieldDto> fieldsDtos = report.registers
                .Select(r => BuildPlayerStatisticDto(r)).ToList();
            GameReportDto reportDto = new GameReportDto()
            {
                Date = report.Date,
                PlayersReports = fieldsDtos
            };
            return reportDto;
        }

        private PlayerFieldDto BuildPlayerStatisticDto(PlayerReportField field)
        {
            PlayerFieldDto dto = new PlayerFieldDto()
            {
                PlayerName = field.PlayerName,
                RolePlayed = field.RolePlayed.ToString(),
                Won = field.Won
            };
            return dto;
        }
    }
}
