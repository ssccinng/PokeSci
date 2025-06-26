namespace Showdown
{
    public record SVChooseData : ChooseData
    {
        public int MoveId { get; set; } = 0;
        public int Target { get; set; } = 999;
        public bool Terastallize { get; set; } = false; // 0: 不使用，1: 使用，2: 强制使用
        public override string ToString()
        {
            if (IsPass)
            {
                return "pass";
            }

            if (Target == 999)
            {
                if (Terastallize)
                {
                    return $"move {MoveId} terastallize";
                }
                else
                {
                    return $"move {MoveId}";
                }
            }
            else
            {
                if (Terastallize)
                {
                    return $"move {MoveId} terastallize {Target} ";
                }
                else
                {
                    return $"move {MoveId} {Target}";
                }
            }
        }
    }
    public record MoveChooseData : ChooseData
    {

        public int MoveId { get; set; } = 0;
        public int Target { get; set; } = 999;
        public bool Dmax { get; set; } = false;
        public MoveChooseData(int moveId, int target = 999, bool dmax = false)
        {
            MoveId = moveId;
            Target = target;
            Dmax = dmax;
        }

        public override string ToString()
        {
            if (IsPass)
            {
                return "pass";
            }

            if (Target == 999)
            {
                if (Dmax)
                {
                    return $"move {MoveId} dynamax";
                }
                else
                {
                    return $"move {MoveId}";
                }
            }
            else
            {
                if (Dmax)
                {
                    return $"move {MoveId} {Target} dynamax";
                }
                else
                {
                    return $"move {MoveId} {Target}";
                }
            }
        }
    }
}
