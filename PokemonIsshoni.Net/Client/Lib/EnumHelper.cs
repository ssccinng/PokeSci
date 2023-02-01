
using PokemonIsshoni.Net.Shared.Models;
using MatchType = PokemonIsshoni.Net.Shared.Models.MatchType;

namespace PokemonIsshoni.Net.Client.Lib
{
    public static class EnumHelper
    {
        public static string ToChsString(this MatchType matchType) => matchType switch
        {
            MatchType.Single => "单打",
            MatchType.Double => "双打",
            _ => "日光回旋下苍穹",
        };
        public static string ToChsString(this MatchOnline matchOnline) => matchOnline switch
        {
            MatchOnline.Offline => "线下赛事",
            MatchOnline.Online => "线上大会",
            _ => "月华飞溅落凌霄",
        };

        public static string ToChsString(this RoundType roundType) => roundType switch
        {
            RoundType.Elimination => "淘汰赛",
            RoundType.Swiss => "瑞士轮",
            RoundType.Robin => "循环赛",
            _ => "可爱星星飞天撞",
        };

        public static string ToChsString(this PromotionType promotionType) => promotionType switch
        {
            PromotionType.Topcut => "人数限制",
            PromotionType.Scorecut => "分数限制",
            _ => "海神庄严交响乐",
        };
        public static string ToChsString(this EliminationType eliminationType) => eliminationType switch
        {
            EliminationType.Double => "双败淘汰赛",
            EliminationType.Single => "单败淘汰赛",
            _ => "海神庄严交响乐",
        };
    }
}
