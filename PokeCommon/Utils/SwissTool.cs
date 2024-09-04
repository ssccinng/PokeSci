public static class SwissTool
{
    public static double[] GetSwissTable(int num, int roundCnt)
    {

        // 求出 roundcnt的组合数 C(0, roundcnt) , C(1, roundcnt) , ...  C(roundcnt, roundcnt)

        int[,] c = new int[roundCnt + 1, roundCnt + 1];
        for (int i = 0; i <= roundCnt; i++)
        {
            c[i, 0] = 1;
            c[i, i] = 1;
        }
        for (int i = 1; i <= roundCnt; i++)
        {
            for (int j = 1; j < i; j++)
            {
                c[i, j] = c[i - 1, j - 1] + c[i - 1, j];
            }
        }

        int full = 1 << roundCnt;

        double[] res = new double[roundCnt];

        for (int i = 0; i < roundCnt; i++)
        {
            res[i] = c[roundCnt, i] * num / (double)full;
        }

        return res;




    }
}