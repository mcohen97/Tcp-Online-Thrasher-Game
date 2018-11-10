

using System;
using System.Collections.Generic;

namespace UserCRUDServiceContract
{
    [Serializable]
    public class GameReportDto
    {
        public DateTime Date { get; set; }
        public ICollection<PlayerFieldDto> PlayersReports {get;set;}
    }
}
