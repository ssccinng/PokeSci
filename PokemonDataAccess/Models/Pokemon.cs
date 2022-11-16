using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PokemonDataAccess.Models
{
    public class Pokemon
    {
        public int Id
        {
            get; set;
        }

        [Comment("全国图鉴编号")]
        public int DexId
        {
            get; set;
        }
        // public PokeDex PokeDex { get; set; }


        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string NameChs
        {
            get; set;
        }

        [Comment("英文名")]
        [Column(TypeName = "varchar(40)")]
        public string NameEng
        {
            get; set;
        }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string NameJpn
        {
            get; set;
        }
        //[Column(TypeName = "varchar(40)")]
        //public string PSName { get; set; }
        #endregion


        [Comment("形态编号")]
        public int PokeFormId
        {
            get; set;
        }


        [Comment("形态名")]
        [Column(TypeName = "nvarchar(30)")]
        public string FormNameChs
        {
            get; set;
        }
        [Column(TypeName = "varchar(40)")]
        public string FormNameEng
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(30)")]
        public string FormNameJpn
        {
            get; set;
        }

        [Comment("全名")]
        [Column(TypeName = "nvarchar(30)")]
        public string FullNameChs
        {
            get; set;
        }
        [Column(TypeName = "varchar(60)")]
        public string FullNameEng
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(30)")]
        public string FullNameJpn
        {
            get; set;
        }
        // [Comment("PS名字")]
        // [Column(TypeName = "varchar(30)")]
        // public string PSName { get; set; }

        [Comment("意味無し")]
        public int Stage
        {
            get; set;
        }

        [Comment("能否极巨化")]
        public bool DMax
        {
            get; set;
        }


        [NotMapped]
        public int[] Base_Value
        {
            get
            {
                return new int[] { BaseHP, BaseAtk, BaseDef, BaseSpa, BaseSpd, BaseSpe };
            }
            private set
            {
                BaseHP = value[0];
                BaseAtk = value[1];
                BaseDef = value[2];
                BaseSpa = value[3];
                BaseSpd = value[4];
                BaseSpe = value[5];

            }
        }// 考虑还是分开后储存

        public int BaseHP
        {
            get; set;
        }
        public int BaseAtk
        {
            get; set;
        }
        public int BaseDef
        {
            get; set;
        }
        public int BaseSpa
        {
            get; set;
        }
        public int BaseSpd
        {
            get; set;
        }
        public int BaseSpe
        {
            get; set;
        }

        // [Column(TypeName = "varchar(25)")]
        [NotMapped]
        public int[] Get_EV
        {
            get
            {
                return new int[] { EVHP, EVAtk, EVDef, EVSpa, EVSpd, EVSpe };
            }
            private set
            {
                EVHP = value[0];
                EVAtk = value[1];
                EVDef = value[2];
                EVSpa = value[3];
                EVSpd = value[4];
                EVSpe = value[5];
                // 这里可以加上赋值
            }
        }// 考虑还是分开后储存
        public int EVHP
        {
            get; set;
        }
        public int EVAtk
        {
            get; set;
        }
        public int EVDef
        {
            get; set;
        }
        public int EVSpa
        {
            get; set;
        }
        public int EVSpd
        {
            get; set;
        }
        public int EVSpe
        {
            get; set;
        }


        [Comment("♀/(♂+♀)=(GenderRatio+1)/(255+1),254雌性,255无性别")]
        public int GenderRatio
        {
            get; set;
        }


        public int CatchRate
        {
            get; set;
        }


        public Ability Ability1
        {
            get; set;
        }
        public Ability Ability2
        {
            get; set;
        }
        public Ability AbilityH
        {
            get; set;
        }


        public PokeType Type1
        {
            get; set;
        }
        public PokeType Type2
        {
            get; set;
        }


        [Comment("1:Erratic,2:Fast,3:Medium Fast,4:Medium Slow,5:Slow")]
        public int EXPGroup
        {
            get; set;
        }


        public EggGroup? EggGroup1
        {
            get; set;
        }
        public EggGroup? EggGroup2
        {
            get; set;
        }
        [Comment("孵化周期")]
        public int HatchCycles
        {
            get; set;
        }


        [Column(TypeName = "decimal(5, 2)")]
        public decimal Height
        {
            get; set;
        }

        [Column(TypeName = "decimal(4, 1)")]
        public decimal Weight
        {
            get; set;
        }

        [Comment("0-9:红色,蓝色,绿色,黄色,紫色,粉红色,褐色,黑色,灰色,白色")]
        public int Color
        {
            get; set;
        }

        // public Pokemon BeforeBattle {get; set;}
        // public Pokemon BattleData {get; set;}
        // public Pokemon IconData {get; set;}


    }
}