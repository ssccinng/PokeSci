// See https://aka.ms/new-console-template for more information
using IronOcr;
//using IronOcr;
using System.Drawing;


Bitmap bitmap = new Bitmap(@"D:\QQ\1078995020\FileRecv\Fk4XK7TagAED2Xt.jfif");
// 40 100, 1236, 52
PokeOCRSV.PokeRegion pokeRegion = new(new Point(60, 110));

bitmap = CropImage(bitmap, new Rectangle(pokeRegion.BasePoint, new Size(pokeRegion.Width, pokeRegion.Height)));
//bitmap = CropImage(bitmap, new Rectangle(16, 16, 94, 32));
bitmap.Save("test.jpg");
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