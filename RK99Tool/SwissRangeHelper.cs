using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RK9Tool
{
    public class SwissRoundInfo
    {
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public int TotalSwissRounds { get; set; }
        public int Day1SwissRounds { get; set; }
        public int? Day2MatchPointsThreshold { get; set; } // 使用 nullable int 类型来处理 "N/A"
        public int Day2SwissRounds { get; set; }
    }

    public static class SwissRangeHelper
    {
        static List<SwissRoundInfo> _swissRoundInfos =
        [
            new() { MinParticipants = 33, MaxParticipants = 64, TotalSwissRounds = 7, Day1SwissRounds = 7, Day2MatchPointsThreshold = null, Day2SwissRounds = 0 },
            new() { MinParticipants = 65, MaxParticipants = 128, TotalSwissRounds = 8, Day1SwissRounds = 6, Day2MatchPointsThreshold = 12, Day2SwissRounds = 2 },
            new() { MinParticipants = 129, MaxParticipants = 256, TotalSwissRounds = 9, Day1SwissRounds = 7, Day2MatchPointsThreshold = 15, Day2SwissRounds = 2 },
            new () { MinParticipants = 257, MaxParticipants = 512, TotalSwissRounds = 10, Day1SwissRounds = 8, Day2MatchPointsThreshold = 18, Day2SwissRounds = 2 },
            new () { MinParticipants = 513, MaxParticipants = 1024, TotalSwissRounds = 11, Day1SwissRounds = 8, Day2MatchPointsThreshold = 18, Day2SwissRounds = 3 },
            new () { MinParticipants = 1025, MaxParticipants = 2048, TotalSwissRounds = 12, Day1SwissRounds = 8, Day2MatchPointsThreshold = 18, Day2SwissRounds = 4 },
            new () { MinParticipants = 2049, MaxParticipants = 4096, TotalSwissRounds = 13, Day1SwissRounds = 9, Day2MatchPointsThreshold = 21, Day2SwissRounds = 4 },
            new () { MinParticipants = 4097, MaxParticipants = 8192, TotalSwissRounds = 14, Day1SwissRounds = 9, Day2MatchPointsThreshold = 21, Day2SwissRounds = 5 }
        ];

        public static SwissRoundInfo GetSwissRoundInfo(int participants)
        {
            return _swissRoundInfos.FirstOrDefault(s => s.MinParticipants <= participants && s.MaxParticipants >= participants) ?? _swissRoundInfos[0];
        }

    }
}
