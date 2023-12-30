// See https://aka.ms/new-console-template for more information
using IronOcr;
using PokeOCRSV;
using PokeTeamImageTran;
//using IronOcr;.
using System.Drawing;
using System.Text;

string TeamId = "dataset";
StringBuilder stringBuilder = new StringBuilder();
//goto py;
if (!Directory.Exists($"TeamDataTera/{TeamId}"))
{
    Directory.CreateDirectory($"TeamDataTera/{TeamId}");
}
//var files = Directory.GetFiles("TeamImg");
var files = Directory.GetFiles("TeamBug");
int idx = 1;

files.OrderBy(s => int.Parse(s.Split("\\").Last().Split(".")[0]));
foreach (var file in files)
{

    //Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\Fk4XK7TagAED2Xt.jfif");
    Bitmap bitmap = new Bitmap(Environment.CurrentDirectory + "/" + file);
    bitmap = ResizeBitmap(bitmap, 1280, 720);
    //bitmap.Save($"{idx++}.png");
    //    continue;
    //Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\16U0~$[[``NJ359DALLGXS6.jpg");
    //Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\0{G4$JT[HY_RY{XT2M(XS[X.jpg");
    // 40 100, 1236, 52
    //PokeOCRSV.PokeRegion[] pokeRegions = { 
    //    new(new Point(55, 114)), new(new Point(55, 295)), new(new Point(55, 474)),
    //    new(new Point(665, 114)), new(new Point(665, 295)), new(new Point(665, 474)),

    //};

    PokeRegion[] pokeRegions = {
        new(new Point(55, 114)), new(new Point(55, 295)), new(new Point(55, 474)),
        new(new Point(665, 114)), new(new Point(665, 295)), new(new Point(665, 474)),

    };
    //PokeOCRSV.PokeRegion pokeRegion2 = ;
    //PokeOCRSV.PokeRegion pokeRegion3 = ;
    //PokeOCRSV.PokeRegion pokeRegion4 = new(new Point(670, 110));
    //PokeOCRSV.PokeRegion pokeRegion5 = new(new Point(670, 295));
    //PokeOCRSV.PokeRegion pokeRegion6 = new(new Point(670, 470));


    for (int i = 0; i < 6; i++)
    {
        var abitmap = CropImage(bitmap, new Rectangle(pokeRegions[i].BasePoint, new Size(pokeRegions[i].Width, pokeRegions[i].Height)));
        var movemap = CropImage(abitmap, pokeRegions[i].RectangleMove);

        //for (int ji = 0; ji < 4; ji++)
        //{
        //    //CropImage(movemap, new Rectangle(new Point(0, ji * 140 / 4), new Size(200, 140/4))).Save($"TeamData/{TeamId}/test{i}_move{ji}.jpg");
        //    var ad = CropImage(movemap, new Rectangle(new Point(0, ji * 140 / 4), new Size(200, 140 / 4)));
        //    ad.Save($"TeamDataTera/{TeamId}/{idx++:00000}.jpg");
        //}
        var namemap = CropImage(abitmap, pokeRegions[i].RectangleTera);
        //var itemmap = CropImage(abitmap, pokeRegions[i].Item);
        //var abilmap = CropImage(abitmap, pokeRegions[i].Ability);
        //abitmap.Save($"TeamData/{TeamId}/test{i}.jpg");
        //movemap.Save($"TeamData/{TeamId}/test{i}_move.jpg");
        //namemap.Save($"TeamData/{TeamId}/test{i}_name.jpg");
        //itemmap.Save($"TeamData/{TeamId}/test{i}_item.jpg");
        //abilmap.Save($"TeamData/{TeamId}/test{i}_abil.jpg");

        namemap.Save($"TeamDataTera/{TeamId}/{idx++:00000}.jpg"); ;
        //itemmap.Save($"TeamData/{TeamId}/{idx++:00000}.jpg"); ;
        //abilmap.Save($"TeamData/{TeamId}/{idx++:00000}.jpg"); ;

        //return;
    }

}


return;
// 这里要生成数据
//return;
//if (!Directory.Exists("dataset"))
//{
//    Directory.CreateDirectory("dataset");
//}
py:
for (int i = 6; i < 7; i++)
{


    var res = PokeTeamImageTran.PyExtensions.CallPaddleOcr($"TeamData/{TeamId}/{i}", PokeTeamImageTran.LangType.japan);
    int ii = 0;
    foreach (var rr in res)
    {
        string filePath = "";
        if (rr.StartsWith("filePath: "))
        {
            ++ii;
            //if (ii % 200 == 0)
            //{
            //    File.WriteAllText($"TeamData/{TeamId}/rec{(ii - 1) / 200}.txt", stringBuilder.ToString());
            //    stringBuilder.Clear();
            //}
            filePath = rr.Substring(10).Trim();
            stringBuilder.Append("\n");
            stringBuilder.Append(filePath);
            stringBuilder.Append("\t");
        }
        else
        {
            stringBuilder.Append(rr.Trim());
        }

        Console.WriteLine(rr);
    }
    File.WriteAllText($"TeamData/{TeamId}/rec{i}.txt", stringBuilder.ToString());
    stringBuilder.Clear();

}
//var ffs = Directory.GetFiles($"TeamData/{TeamId}");
//for (int i = 0; i < ffs.Length; i++)
//{
//    if (!Directory.Exists($"TeamData/{TeamId}/{i / 200}"))
//    {
//        Directory.CreateDirectory($"TeamData/{TeamId}/{i / 200}");
//    }
//    File.Move(ffs[i], $"TeamData/{TeamId}/{i / 200}/{i:00000}.png");
//}


//bitmap = CropImage(bitmap, new Rectangle(16, 16, 94, 32));
return;
// 编写一下坐标 rect

//bitmap = CropImage(bitmap, new Rectangle(40, 100, 1200, 520));
//bitmap = CropImage(bitmap, new Rectangle(16, 16, 94, 32));
//bitmap.Save("test.jpg");
//// 17 15 110 48
//var Ocr = new IronTesseract() { Language = OcrLanguage.JapaneseBest};
//using (var Input = new OcrInput(bitmap))
//{

//    //Input.Deskew();  // use if image not straight
//    Input.DeNoise(); // use if image contains digital noise
//    var Result = Ocr.Read(Input);
//    Console.WriteLine(Result.Text);
//}


Bitmap CropImage(Bitmap source, Rectangle section)
{

    Bitmap target = new Bitmap(section.Width, section.Height);

    using (Graphics g = Graphics.FromImage(target))
    {
        g.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height),
                         section,
                         GraphicsUnit.Pixel);
    }
    return target;
}

Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
{
    Bitmap result = new Bitmap(width, height);
    using (Graphics g = Graphics.FromImage(result))
    {
        g.DrawImage(bmp, 0, 0, width, height);
    }

    return result;
}