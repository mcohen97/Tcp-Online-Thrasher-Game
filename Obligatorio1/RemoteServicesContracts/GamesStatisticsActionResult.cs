using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCRUDServiceContract;

namespace ActionResults
{
    public class GamesStatisticsActionResult:ActionResult
    {
        public ICollection<GameReportDto> GamesStatistics { get; set; }
    }
}
