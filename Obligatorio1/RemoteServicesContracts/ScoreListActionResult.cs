using GamesInfoService;
using System.Collections.Generic;

namespace ActionResults
{
    public class ScoreListActionResult:ActionResult
    {
        public ICollection<ScoreDto> ScoreList { get; set; }
    }
}
