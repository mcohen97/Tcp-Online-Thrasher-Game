using DataAccessInterface;
using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class GamesInfoInMemory: IGamesInfoRepository
    {
        public static readonly Lazy<GamesInfoInMemory> instance = new Lazy<GamesInfoInMemory>(() => new GamesInfoInMemory(10,10));
        private Score[] topScores;
        private int nTopScores;
        private GameReport[] reportsQueue;
        private int nLastGames;
        private int queueFront;

        public GamesInfoInMemory(int relevantTopScores, int relevantLastGames) {
            nTopScores = relevantTopScores;
            topScores = new Score[nTopScores];
            nLastGames = relevantLastGames;
            reportsQueue = new GameReport[relevantLastGames];
            queueFront = 0;
        }

        public void AddScore(Score aScore)
        {
            lock (topScores) {
                AddScoreWithLock(aScore);
            }
        }

        public void AddScoreWithLock(Score aScore) {
            bool placed = false;
            for (int i = 0; i < topScores.Length && !placed; i++)
            {
                if (topScores[i] == null)
                {
                    topScores[i] = aScore;
                    placed = true;
                }
                else if (aScore.Points >= topScores[i].Points)
                {
                    PlaceScore(i, aScore);
                    placed = true;
                }
            }
        }

        private void PlaceScore(int position, Score aScore)
        {
            for (int i = topScores.Length - 1; i > position; i--) {
                topScores[i] = topScores[i - 1];
            }
            topScores[position] = aScore;
        }

        public ICollection<Score> GetScores()
        {
            ICollection<Score> topScores;
            lock (this.topScores) {
                topScores = GetScoresWithLock();
            }
            return topScores;
        }

        private ICollection<Score> GetScoresWithLock()
        {
            List<Score> scores = new List<Score>();
            for (int i = 0; i < topScores.Length && topScores[i] != null; i++)
            {
                scores.Add(topScores[i]);
            }
            return scores;
        }

        public void AddGameReport(GameReport report) {
            lock (reportsQueue) {
                AddGameReportWithLock(report);
            }
        }

        private void AddGameReportWithLock(GameReport report)
        {
            reportsQueue[queueFront] = report;
            queueFront = ++queueFront % nLastGames;
        }

        public ICollection<GameReport> GetGameReports() {
            ICollection<GameReport> reports = new List<GameReport>();
            lock (reportsQueue) {
                reports = GetGameReportsWithLock();
            }
            return reports;
        }

        private ICollection<GameReport> GetGameReportsWithLock()
        {
            ICollection<GameReport> games = new List<GameReport>();
            int pos = queueFront-1;
            for (int i = 0; (i < nLastGames) && (reportsQueue[pos] != null); i++)
            {
                games.Add(reportsQueue[pos]);
                pos = (nLastGames + --pos) % nLastGames;
            }
            return games;
        }
    }
}
