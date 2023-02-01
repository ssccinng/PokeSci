using System.Diagnostics;

namespace PokeTeamImageTran;

public enum LangType
{
    japan,
    ch,
    en,
    korean,
    chinese_cht,
    ta,
    te,
    latin,
    arabic,
    cyrillic,
    devanagari,
    //italian

}

public static class PyExtensions
{
    public static string PythonPath { get; } = "/root/anaconda3/envs/PaddleOcr/bin/python3.10";
    //public static string PythonPath { get; } = "F:/anaconda3/envs/PaddleOCR/python.exe";
    public static string PyScriptPath { get; } = "/home/paddle/PaddleOCR/paddleocr.py";
    //public static string PyScriptPath { get; } = " f:/VSProject/PaddleOCR/paddleocr.py";

    public static List<string> CallPaddleOcr(string imgPath, LangType langType)
    {
        return CallPaddleOcr(imgPath, langType.ToString());
    }
    public static List<string> CallPaddleOcr(string imgPath, string langType)
    {
        List<string> list = new List<string>();

        ProcessStartInfo ProcessStartInfo = new ProcessStartInfo
        {
            FileName = PythonPath,
        };
        ProcessStartInfo.UseShellExecute = false;
        ProcessStartInfo.RedirectStandardOutput = true;
        ProcessStartInfo.Arguments = string.Format("{0}  --image_dir {1} --use_angle_cls true --lang {2} ",
            string.Format(@"{0}", PyScriptPath), imgPath, langType switch
            {
                "italian" or "german" or "span" or "french" => "latin",
                _ => langType
            }); ;
        using Process process = Process.Start(ProcessStartInfo);

        using (StreamReader myStreamReader = process.StandardOutput)
        {
            string outputString;
            while ((outputString = myStreamReader.ReadLine()) != null)
            {
                list.Add(outputString);
                Console.WriteLine(outputString);
            }
            ;
            //process.WaitForExit();
        }
        return list;
    }

}
