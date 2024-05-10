
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeShitLib
{
    public class MOVE
    {
        public string type = null;
        public string pp = null;
        public string dec;
        public string acc = null;
        public string pow = null;
        public string chiname = null;
        public string engname = null;
        public string jpnname = null;
        public int id = 0;
        public string atktype = null;

        public int color1 = 0, color2 = 0;
        public string _16color1 , _16color2;
        public string aaa = @"一般,0,A8A878,6D6D4E
格斗,1,C03028,7D1F1A
飞行,2,A890F0,6D5E9C
毒,3,A040A0,682A68
地面,4,E0C068,927D44
岩石,5,B8A038,786824
虫,6,A8B820,6D7815
幽灵,7,705898,493963
钢,8,B8B8D0,787887
火,9,F08030,9C531F
水,10,6890F0,445E9C
草,11,78C850,4E8234
电,12,F8D030,A1871F
超能力,13,F85888,A13959
冰,14,98D8D8,638D8D
龙,15,7038F8,4924A1
恶,16,705848,49392F
妖精,17,EE99AC,9B6470";

        public MOVE(string id, string chiname, string jpnname,
            string engname, string type, string atktype, string pow, string acc, string pp)
        {
            this.id = int.Parse(id);
            this.chiname = chiname;
            this.jpnname = jpnname;
            this.engname = engname;
            this.type = type;
            string[] qq = aaa.Split('\n');

            foreach (string fl in qq)
            {
                if (type == fl.Split(',')[0])
                {
                    color1 = Convert.ToInt32("0x" + fl.Split(',')[2], 16);
                    color2 = Convert.ToInt32("0x" + fl.Split(',')[3], 16);

                    _16color1 = fl.Split(',')[2];
                    _16color2 = fl.Split(',')[3];
                    break;
                }
            }
            this.atktype = atktype;
            this.pow = pow;
            this.acc = acc;
            this.pp = pp;
        }
        public MOVE(string[] data)
        {
            {
                //this.id = int.Parse(data[0]);
                this.chiname = data[1];
                this.jpnname = data[2];
                this.engname = data[3];
                this.type = data[4];
                string[] qq = aaa.Split('\n');

                foreach (string fl in qq)
                {
                    if (data[4] == fl.Split(',')[0])
                    {
                        color1 = Convert.ToInt32("0x" + fl.Split(',')[2].Trim(), 16);
                        color2 = Convert.ToInt32("0x" + fl.Split(',')[3].Trim(), 16);
                        _16color1 = fl.Split(',')[2].Trim();
                        _16color2 = fl.Split(',')[3].Trim();
                        break;
                    }
                }
                this.atktype = data[5];
                this.pow = data[6];
                this.acc = data[7];
                this.pp = data[8];
                this.dec = data[9];
            }
        }
    }
}
