// See https://aka.ms/new-console-template for more information
using IronOcr;
//using IronOcr;.
using System.Drawing;

string TeamId = "TestKor";

if (!Directory.Exists($"TeamData/{TeamId}"))
{
    Directory.CreateDirectory($"TeamData/{TeamId}");
}

//Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\Fk4XK7TagAED2Xt.jfif");
//Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\16U0~$[[``NJ359DALLGXS6.jpg");
Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\0{G4$JT[HY_RY{XT2M(XS[X.jpg");
// 40 100, 1236, 52
PokeOCRSV.PokeRegion[] pokeRegions = { 
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
    var namemap = CropImage(abitmap, pokeRegions[i].Name);
    var itemmap = CropImage(abitmap, pokeRegions[i].Item);
    var abilmap = CropImage(abitmap, pokeRegions[i].Ability);
    //abitmap.Save($"TeamData/{TeamId}/test{i}.jpg");
    movemap.Save($"TeamData/{TeamId}/test{i}_move.jpg");
    namemap.Save($"TeamData/{TeamId}/test{i}_name.jpg");
    itemmap.Save($"TeamData/{TeamId}/test{i}_item.jpg");
    abilmap.Save($"TeamData/{TeamId}/test{i}_abil.jpg");

    //return;
}
//bitmap = CropImage(bitmap, new Rectangle(16, 16, 94, 32));
return;
// 编写一下坐标 rect

bitmap = CropImage(bitmap, new Rectangle(40, 100, 1200, 520));
bitmap = CropImage(bitmap, new Rectangle(16, 16, 94, 32));
bitmap.Save("test.jpg");
// 17 15 110 48
var Ocr = new IronTesseract() { Language = OcrLanguage.JapaneseBest};
using (var Input = new OcrInput(bitmap))
{

    //Input.Deskew();  // use if image not straight
    Input.DeNoise(); // use if image contains digital noise
    var Result = Ocr.Read(Input);
    Console.WriteLine(Result.Text);
}


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