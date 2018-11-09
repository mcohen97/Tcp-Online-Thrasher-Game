

using System;
using System.Collections.Generic;

namespace UserCRUDServiceContract
{
    public class GameReportDto
    {
        public DateTime Date { get; set; }
        public ICollection<PlayerFieldDto> PlayersReports {get;set;}
    }
}
