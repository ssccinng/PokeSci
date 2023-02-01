using System.Text.Json;

namespace PokeTeamImageTran;

public static class TranslateHelper
{

    public static List<PokeModel> PokeModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("pokenameall.json"));
    public static List<PokeModel> WazaModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("wazanameall.json"));
    public static List<PokeModel> ItemModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("itemnameall.json"));
    public static List<PokeModel> AbilityModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("tokuseinameall.json"));

    public static string TranslateNameToChs(string text, string langType, string type = "name")
    {

        var models = type switch
        {
            "waza" => WazaModels,
            "item" => ItemModels,
            "ability" => AbilityModels,
            _ => PokeModels,
        };
        text= text.Trim();
        IOrderedEnumerable<PokeModel> res;
        switch (langType)
        {
            case "japan":
                res = models.OrderByDescending(x => findSimilarity(text, x.Name_Jpn));
                return res.First().Name_Chs;
            case "ch":
                break;
            case "en":
                res = models.OrderByDescending(x => findSimilarity(text, x.Name_Eng));
                return res.First().Name_Chs;
                break;
            case "korean":
                return models.OrderByDescending(x => findSimilarity(text, x.Name_Kor)).First().Name_Chs;
                break;
            case "chinese_cht":
                return models.OrderByDescending(x => findSimilarity(text, x.Name_Cht)).First().Name_Chs;
                break;
            case "italian":
                res = models.OrderByDescending(x => findSimilarity(text, x.Name_Ita)); 
                return res.First().Name_Chs;  
            case "span":
                res = models.OrderByDescending(x => findSimilarity(text, x.Name_Span)); 
                return res.First().Name_Chs; 
            case "german":
                //return models.OrderByDescending(x => x.Name_Ger.GetSimilarityWith(text)).First().Name_Chs;
                return models.OrderByDescending(x => findSimilarity(text, x.Name_Ger)).First().Name_Chs;
            case "french":
                return models.OrderByDescending(x => findSimilarity(text, x.Name_Fre)).First().Name_Chs;
            case "ta":
                break;
            case "te":
                break;
            case "latin": // 这个危！
                break;
            case "arabic":
                break;
            case "cyrillic":
                break;
            case "devanagari":
                break;
        }
        return "";
    }

    /// <summary>
    /// 获取两个字符串的相似度
    /// </summary>
    /// <param name=”sourceString”>第一个字符串</param>
    /// <param name=”str”>第二个字符串</param>
    /// <returns></returns>
    public static decimal GetSimilarityWith(this string sourceString, string str)
    {

        decimal Kq = 2;
        decimal Kr = 1;
        decimal Ks = 1;

        char[] ss = sourceString.ToCharArray();
        char[] st = str.ToCharArray();

        //获取交集数量
        int q = ss.Intersect(st).Count();
        int s = ss.Length - q;
        int r = st.Length - q;

        return Kq * q / (Kq * q + Kr * r + Ks * s);
    }

    public static int getEditDistance(string X, string Y)
    {
        int m = X.Length;
        int n = Y.Length;

        int[][] T = new int[m + 1][];
        for (int i = 0; i < m + 1; ++i)
        {
            T[i] = new int[n + 1];
        }

        for (int i = 1; i <= m; i++)
        {
            T[i][0] = i;
        }
        for (int j = 1; j <= n; j++)
        {
            T[0][j] = j;
        }

        int cost;
        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                cost = X[i - 1] == Y[j - 1] ? 0 : 1;
                T[i][j] = Math.Min(Math.Min(T[i - 1][j] + 1, T[i][j - 1] + 1),
                        T[i - 1][j - 1] + cost);
            }
        }

        return T[m][n];
    }

    public static double findSimilarity(string x, string y)
    {
        if (x == null || y == null)
        {
            throw new ArgumentException("Strings must not be null");
        }

        double maxLength = Math.Max(x.Length, y.Length);
        if (maxLength > 0)
        {
            // optionally ignore case if needed
            return (maxLength - getEditDistance(x, y)) / maxLength;
        }
        return 1.0;
    }
    //private static string (pr)

}
