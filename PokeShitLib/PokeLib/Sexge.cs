using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeShitLib
{
    public class Sexge
    {
        private string[] aa = { "", "物攻", "物防", "特攻", "特防", "速度" };
        public int rss;

        public int up;

        public int down;

        public string name;

        public Sexge(string name, int up, int down, int rss)
        {
            if (up == 0)
            {
                name += "(无修正)";
            }
            else
            {
                name += string.Format("(+{0} -{1})", aa[up], aa[down]);
            }
            this.name   = name;
            this.up     = up;
            this.down   = down;
            this.rss    = rss;
        }
    }
}
