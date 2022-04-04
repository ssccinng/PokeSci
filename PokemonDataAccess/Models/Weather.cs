using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonDataAccess.Models
{
    /// <summary>
    /// 天气描述以及效果
    /// </summary>
    public class Weather
    {
        public int WeatherId { get; set; }
        public string WeatherName { get; set;}
        //public int WeatherLevel { get; set;}
        //public string WeatherType { get; set;}
        //public sbyte WeatherWeight { get; set;}
        public string WeatherType { get; set; }
    }
}
