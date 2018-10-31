using System;
using System.Collections.Generic;
using System.Linq;
using ScoreService;

namespace DataAccess
{
    public class ScoresInMemory: IScoreRepository
    {
        public static readonly Lazy<ScoresInMemory> instance = new Lazy<ScoresInMemory>(() => new ScoresInMemory(10));
        private Score[] scoresQueue;
        private int nLastScores;
        private int frontPos;

        public ScoresInMemory(int reelevantLastScores) {
            nLastScores = reelevantLastScores;
            scoresQueue = new Score[nLastScores];
            frontPos = 0;
        }

        public void AddScore(Score aScore)
        {
            int nextPos = ++frontPos % nLastScores;
            scoresQueue[nextPos] = aScore;
        }

        public ICollection<Score> GetScores()
        {
            ICollection<Score> sorted = new List<Score>();
            int auxPos = frontPos;
            for (int i = 0; i < scoresQueue.Length; i++) {
                sorted.Add(scoresQueue[auxPos]);
                //adding nLastScores transform position -1 into the last one (circularity)
                auxPos = (nLastScores + (--auxPos)) % nLastScores;
            }
           return sorted.Reverse().ToList();
        }
    }
}
