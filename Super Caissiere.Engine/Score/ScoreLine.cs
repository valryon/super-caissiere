using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCaissiere.Engine.Score
{
    /// <summary>
    /// Score data
    /// </summary>
    [Serializable]
    public class ScoreLine : IComparable<ScoreLine>
    {
        /// <summary>
        /// Player name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Score value
        /// </summary>
        public Int32 Points { get; set; }

        /// <summary>
        /// Score time
        /// </summary>
        public DateTime Date { get; set; }

        public ScoreLine()
            : this("-----", 0)
        {
        }

        public ScoreLine(String name, Int32 points)
        {
            this.Name = name;
            this.Points = points;
            this.Date = DateTime.Now;
        }

        public override string ToString()
        {
            return Name + " - " + Points.ToString("000000000") + " (" + Date.ToString("dd/MM/yyyy") + ")";
        }

#if WINDOWS
        /// <summary>
        /// Unique md5 hash for this score data
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        private static string HashString(string _value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(_value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++) ret += data[i].ToString("x2").ToLower();
            return ret;
        }
#endif

        public int CompareTo(ScoreLine scoreLine)
        {
            if (Points > scoreLine.Points)
            {
                return 1;
            }
            else if (Points == scoreLine.Points)
            {
                if (Date > scoreLine.Date)
                {
                    return 1;
                }

                return 0;
            }

            return -1;
        }
    }
}
