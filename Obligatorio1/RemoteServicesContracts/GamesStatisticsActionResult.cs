using System;
using System.Collections.Generic;
using UserCRUDServiceContract;

namespace ActionResults
{
    [Serializable]
    public class GamesStatisticsActionResult:ActionResult
    {
        public ICollection<GameReportDto> GamesStatistics { get; set; }
    }
}
