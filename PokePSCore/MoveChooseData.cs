namespace PokePSCore
{
    public class MoveChooseData : ChooseData
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
                return "Pass";
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
