using PokemonDataAccess.Models;
using PokeUI3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PokeMath.CalcUnits;

using PokeMath;

namespace PokeUI3.MVVM.ViewModel
{
    public class BSTToolsViewModel: ObservableObject
    {
        public Pokemon Pokemon { get; set; }
        //public SWSHTools SWSHTools { get; set; } = new SWSHTools();

        public int StatHP => SWSHTools.GetHP(Pokemon.BaseHP, IVHP, EVHP);
        public int PureMaxHP => SWSHTools.GetPureBaseHP((int)(StatHP * HPGain), 31);
        public int PureMinHP => SWSHTools.GetPureBaseHP((int)(StatHP * HPGain), 0);

        private int _evHP;
            
        public int EVHP
        {
            get { return _evHP; }
            set { _evHP = value;
                OnPropertyChanged("StatHP");
                OnPropertyChanged("PureMaxHP");
                OnPropertyChanged("PureMinHP");
                OnPropertyChanged(); }
        }
        private int _ivHP = 31;
        public int IVHP
        {
            get { return _ivHP; }
            set { _ivHP = value; OnPropertyChanged("StatHP"); OnPropertyChanged("StatHP");
                OnPropertyChanged("PureMaxHP");
                OnPropertyChanged("PureMinHP");
                OnPropertyChanged();
            }
        }

        public int StatATK => SWSHTools.GetOtherStat(Pokemon.BaseAtk, IVATK, EVATK);
        public int PureMaxATK => SWSHTools.GetPureBaseOtherStat((int)(StatATK * ATKGain), 31);
        public int PureMinATK => SWSHTools.GetPureBaseOtherStat((int)(StatATK * ATKGain), 0);

        private int _evATK;
        public int EVATK
        {
            get { return _evATK; }
            set { _evATK = value;
                OnPropertyChanged("StatATK");
                OnPropertyChanged("PureMaxATK");
                OnPropertyChanged("PureMinATK");
                OnPropertyChanged(); }
        }
        private int _ivATK = 31;
        public int IVATK
        {
            get { return _ivATK; }
            set { _ivATK = value; OnPropertyChanged("StatATK"); OnPropertyChanged("StatATK");
                OnPropertyChanged("PureMaxATK");
                OnPropertyChanged("PureMinATK");
                OnPropertyChanged();
            }
        }

        public int StatDEF => SWSHTools.GetOtherStat(Pokemon.BaseDef, IVDEF, EVDEF);
        public int PureMaxDEF => SWSHTools.GetPureBaseOtherStat((int)(StatDEF * DEFGain), 31);
        public int PureMinDEF => SWSHTools.GetPureBaseOtherStat((int)(StatDEF * DEFGain), 0);

        private int _evDEF;
        public int EVDEF
        {
            get { return _evDEF; }
            set { _evDEF = value;
                OnPropertyChanged("StatDEF");
                OnPropertyChanged("PureMaxDEF");
                OnPropertyChanged("PureMinDEF");
                OnPropertyChanged(); }
        }
        private int _ivDEF = 31;
        public int IVDEF
        {
            get { return _ivDEF; }
            set { _ivDEF = value; OnPropertyChanged("StatDEF"); OnPropertyChanged("StatDEF");
                OnPropertyChanged("PureMaxDEF");
                OnPropertyChanged("PureMinDEF");
                OnPropertyChanged();
            }
        }

        public int StatSPATK => SWSHTools.GetOtherStat(Pokemon.BaseSpa, IVSPATK, EVSPATK);
        public int PureMaxSPATK => SWSHTools.GetPureBaseOtherStat((int)(StatSPATK * SPATKGain), 31);
        public int PureMinSPATK => SWSHTools.GetPureBaseOtherStat((int)(StatSPATK * SPATKGain), 0);

        private int _evSPATK;
        public int EVSPATK
        {
            get { return _evSPATK; }
            set { _evSPATK = value;
                OnPropertyChanged("StatSPATK");
                OnPropertyChanged("PureMaxSPATK");
                OnPropertyChanged("PureMinSPATK");
                OnPropertyChanged(); }
        }
        private int _ivSPATK = 31;
        public int IVSPATK
        {
            get { return _ivSPATK; }
            set { _ivSPATK = value; OnPropertyChanged("StatSPATK"); OnPropertyChanged("StatSPATK");
                OnPropertyChanged("PureMaxSPATK");
                OnPropertyChanged("PureMinSPATK");
                OnPropertyChanged();
            }
        }

        public int StatSPDEF => SWSHTools.GetOtherStat(Pokemon.BaseSpd, IVSPDEF, EVSPDEF);
        public int PureMaxSPDEF => SWSHTools.GetPureBaseOtherStat((int)(StatSPDEF * SPDEFOtherGain), 31);
        public int PureMinSPDEF => SWSHTools.GetPureBaseOtherStat((int)(StatSPDEF * SPDEFOtherGain), 0);

        private int _evSPDEF;
        public int EVSPDEF
        {
            get { return _evSPDEF; }
            set { _evSPDEF = value;
                OnPropertyChanged("StatSPDEF");
                OnPropertyChanged("PureMaxSPDEF");
                OnPropertyChanged("PureMinSPDEF");
                OnPropertyChanged(); }
        }
        private int _ivSPDEF = 31;
        public int IVSPDEF
        {
            get { return _ivSPDEF; }
            set { _ivSPDEF = value; OnPropertyChanged("StatSPDEF"); OnPropertyChanged("StatSPDEF");
                OnPropertyChanged("PureMaxSPDEF");
                OnPropertyChanged("PureMinSPDEF");
                OnPropertyChanged();
            }
        }

        public int StatSPE => SWSHTools.GetOtherStat(Pokemon.BaseSpe, IVSPE, EVSPE);
        public int PureMaxSPE => SWSHTools.GetPureBaseOtherStat((int)(StatSPE * SPEGain), 31);
        public int PureMinSPE => SWSHTools.GetPureBaseOtherStat((int)(StatSPE * SPEGain), 0);

        private int _evSPE;
        public int EVSPE
        {
            get { return _evSPE; }
            set { _evSPE = value;
                OnPropertyChanged("StatSPE");
                OnPropertyChanged("PureMaxSPE");
                OnPropertyChanged("PureMinSPE");
                OnPropertyChanged(); }
        }
        private int _ivSPE = 31;
        public int IVSPE
        {
            get { return _ivSPE; }
            set { _ivSPE = value; OnPropertyChanged("StatSPE"); OnPropertyChanged("StatSPE");
                OnPropertyChanged("PureMaxSPE");
                OnPropertyChanged("PureMinSPE");
                OnPropertyChanged();
            }
        }
        public double HPGain => HPOtherGain;
        public double HPOtherGain {
            get {
                return _hpOtherGain;
            }
            set {
                _hpOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxHP");
                OnPropertyChanged("PureMinHP");
                OnPropertyChanged("StatHP");
            }
        }
        public double _hpOtherGain = 1;
        public double ATKGain => ATKOtherGain;
        public double ATKOtherGain
        {
            get
            {
                return _atkOtherGain;
            }
            set
            {
                _atkOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxATK");
                OnPropertyChanged("PureMinATK");
                OnPropertyChanged("StatATK");

            }
        }
        public double _atkOtherGain = 1;

        public double DEFGain => DEFOtherGain;
        public double DEFOtherGain
        {
            get
            {
                return _defOtherGain;
            }
            set
            {
                _defOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxDEF");
                OnPropertyChanged("PureMinDEF");
                OnPropertyChanged("StatDEF");

            }
        }
        public double _defOtherGain = 1;

        public double SPATKGain => SPATKOtherGain;
        public double SPATKOtherGain
        {
            get
            {
                return _spatkOtherGain;
            }
            set
            {
                _spatkOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxSPATK");
                OnPropertyChanged("PureMinSPATK");
                OnPropertyChanged("StatSPATK");

            }
        }
        public double _spatkOtherGain = 1;

        public double SPDEFGain => SPDEFOtherGain;
        public double SPDEFOtherGain
        {
            get
            {
                return _spdefOtherGain;
            }
            set
            {
                _spdefOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxSPDEF");
                OnPropertyChanged("PureMinSPDEF");
                OnPropertyChanged("StatSPDEF");

            }
        }
        public double _spdefOtherGain = 1;

        public double SPEGain => SPEOtherGain;
        public double SPEOtherGain
        {
            get
            {
                return _speOtherGain;
            }
            set
            {
                _speOtherGain = value;
                OnPropertyChanged();
                OnPropertyChanged("PureMaxSPE");
                OnPropertyChanged("PureMinSPE");
                OnPropertyChanged("StatSPE");

            }
        }

        public double _speOtherGain = 1;



        public BSTToolsViewModel(Pokemon pokemon)
        {
            Pokemon = pokemon;
        }



    }
}
