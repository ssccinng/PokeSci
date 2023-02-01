using System.Diagnostics;
using Microsoft.OpenApi.Any;

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

#if DEBUG
    public static string PythonPath { get; } = "F:/anaconda3/envs/PaddleOCR/python.exe";
    public static string PyScriptPath { get; } = " f:/VSProject/PaddleOCR/paddleocr.py";
    public static string VitPythonPath { get; } = "F:/anaconda3/envs/vit/python.exe";
    public static string VitPyScriptPath { get; } = "F:/VSProject/so-vits-svc/inference_main.py";
#else
    public static string PyScriptPath { get; } = "/home/paddle/PaddleOCR/paddleocr.py";
    public static string PythonPath { get; } = "/root/anaconda3/envs/PaddleOcr/bin/python3.10";

    public static string VitPythonPath { get; } = "/root/anaconda3/envs/vit/bin/python3.10";
        public static string VitPyScriptPath { get; } = "/home/vit/so-vits-svc/inference_main.py";

#endif

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
    public static string CallVit(string wavPath, string target)
    {
        List<string> list = new List<string>();

        ProcessStartInfo ProcessStartInfo = new ProcessStartInfo
        {
            FileName = VitPythonPath,
        };
        ProcessStartInfo.UseShellExecute = false;
        ProcessStartInfo.RedirectStandardOutput = true;
        ProcessStartInfo.Arguments = string.Format("{0} {1} {2}", VitPyScriptPath, wavPath, target);
        using Process process = Process.Start(ProcessStartInfo);

        using (StreamReader myStreamReader = process.StandardOutput)
        {
            string outputString;
            while ((outputString = myStreamReader.ReadLine()) != null)
            {
                //return outputString;
                //list.Add(outputString);
                Console.WriteLine(outputString);
                if (outputString.StartsWith("voiceRes: "))
                {
                    return outputString[10..];
                }
            }
            ;
            //process.WaitForExit();
        }
        return "";
    }
}
