using GamesInfoService;
using System;
using System.Collections.Generic;


namespace ActionResults
{
    [Serializable]
    public class ScoreListActionResult:ActionResult
    {
        public ICollection<ScoreDto> ScoreList { get; set; }
    }
}
