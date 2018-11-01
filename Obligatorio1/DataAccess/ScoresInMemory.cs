using DataAccessInterface;
using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class ScoresInMemory: IScoreRepository
    {
        public static readonly Lazy<ScoresInMemory> instance = new Lazy<ScoresInMemory>(() => new ScoresInMemory(10));
        private Score[] scoresQueue;
        private int nLastScores;

        public ScoresInMemory(int reelevantLastScores) {
            nLastScores = reelevantLastScores;
            scoresQueue = new Score[nLastScores];
        }

        public void AddScore(Score aScore)
        {
            bool placed = false;
            for (int i = 0; i < scoresQueue.Length && !placed; i++) {
                if (scoresQueue[i] == null)
                {
                    scoresQueue[i] = aScore;
                    placed = true;
                }
                else if (aScore.Points >= scoresQueue[i].Points){
                    PlaceScore(i, aScore);
                    placed = true;
                }
            }
        }

        private void PlaceScore(int position, Score aScore)
        {
            for (int i = scoresQueue.Length - 1; i > position; i--) {
                scoresQueue[i] = scoresQueue[i - 1];
            }
            scoresQueue[position] = aScore;
        }

        public ICollection<Score> GetScores()
        {
            return scoresQueue.ToList();
        }
    }
}
