using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterface
{
    public interface IScoreRepository
    {
        void AddScore(Score aScore);

        ICollection<Score> GetScores();
    }
}
