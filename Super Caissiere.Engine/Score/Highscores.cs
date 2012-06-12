using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCaissiere.Engine.Score
{
    /// <summary>
    /// Highscores system manager
    /// </summary>
    [Serializable]
    public class Highscores
    {
        /// <summary>
        /// Score lines
        /// </summary>
        public ScoreLine[] ScoreLines { get; set; }

        /// <summary>
        /// Serializable constructor
        /// </summary>
        public Highscores()
            : this(0)
        {
            
        }

        public Highscores(int scoreLinesNumer)
        {
            ScoreLines = new ScoreLine[scoreLinesNumer];

            for (int i = 0; i < ScoreLines.Length; i++)
            {
                ScoreLines[i] = new ScoreLine();
            }
        }

        /// <summary>
        /// Add a new score, wich will be sort in the array
        /// </summary>
        /// <param name="score"></param>
        /// <returns>New rank</returns>
        public int AddScore(ScoreLine newScoreLine)
        {
            int rank = ScoreLines.Length + 1;

            ScoreLine[] scoresForThisLevel = ScoreLines;

            for (int scoreLineIndex = 0; scoreLineIndex < scoresForThisLevel.Length; scoreLineIndex++)
            {
                ScoreLine currentLine = scoresForThisLevel[scoreLineIndex];

                // Beat it!
                if (newScoreLine.CompareTo(currentLine) >= 0)
                {
                    rank = scoreLineIndex;

                    ScoreLine last = null;
                    ScoreLine toReplace = newScoreLine;

                    // Move down the other lines
                    for (int i = scoreLineIndex; i < scoresForThisLevel.Length ; i++)
                    {
                        last = scoresForThisLevel[i];
                        scoresForThisLevel[i] = toReplace;

                        toReplace = last;
                    }

                    break;
                }
            }

            return rank;
        }
    }
}
