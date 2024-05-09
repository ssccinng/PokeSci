namespace PokeCommon.Models;

public class PokeModel
{
    public int Id
    {
        get; set;
    }
    public string Name_Chs
    {
        get; set;
    }
    public string Name_Cht
    {
        get; set;
    }
    public string Name_Eng
    {
        get; set;
    }
    public string Name_Jpn
    {
        get; set;
    }
    public string Name_Kor
    {
        get; set;
    }
    public string Name_Ita
    {
        get; set;
    }
    public string Name_Fre
    {
        get; set;
    }
    public string Name_Ger
    {
        get; set;
    }
    public string Name_Span
    {
        get; set;
    }

    public string this[int languageId]
    {
        get
        {
            //{ "ja", "us", "fr", "it", "de", "es", "ko", "sc", "tc" }
            switch (languageId)
            {
                case 0:
                    return Name_Jpn;
                case 1:
                    return Name_Eng;
                case 2:
                    return Name_Fre;
                case 3:
                    return Name_Ita;
                case 4:
                    return Name_Ger;
                case 5:
                    return Name_Span;
                case 6:
                    return Name_Kor;
                case 7:
                    return Name_Chs;
                case 8:
                    return Name_Cht;
                default:
                    return Name_Eng;
            }

        }
        set
        {
            switch (languageId)
            {
                case 0:
                    Name_Jpn = value;
                    break;
                case 1:
                    Name_Eng = value;
                    break;
                case 2:
                    Name_Fre = value;
                    break;
                case 3:
                    Name_Ita = value;
                    break;
                case 4:
                    Name_Ger = value;
                    break;
                case 5:
                    Name_Span = value;
                    break;
                case 6:
                    Name_Kor = value;
                    break;
                case 7:
                    Name_Chs = value;
                    break;
                case 8:
                    Name_Cht = value;
                    break;
                default:
                    Name_Eng = value;
                    break;
            }

        }
    }
}

