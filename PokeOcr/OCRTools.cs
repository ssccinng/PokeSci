using System.Drawing;
using OcrLiteLib;
//using System.Drawing;
namespace PokeCommon.PokeOCR
{
    /// <summary>
    /// 因为不跨平台，考虑不集成在这里
    /// </summary>
    public static class OCRTools
    {

        private static OcrLite _ocrLite = InitOcr();
        private static OcrLite InitOcr()
        {
            OcrLite ocrLite = new OcrLite();
            ocrLite.InitModels("models/dbnet.onnx", "models/angle_net.onnx", "models/crnn_lite_lstm.onnx", "models/keys.txt", 12);
            return ocrLite;
        }
        private static int _idx = 0;
        //OcrLite.isDebugImg = false;
        public static List<(string PokeNameImgPath, string MoveImgPath)> SplitSWSHTeamPage(string imgPath)
        {
            List<(string PokeNameImgPath, string MoveImgPath)> result = new();
            Bitmap bitmap = new(imgPath);
            bitmap = ResizeBitmap(bitmap, 1280, 720);
            List<Rectangle> rects = new List<Rectangle>
            {
                //new Rectangle(398, 23, 120, 170),

                new Rectangle(398, 26, 137, 32),
                new Rectangle(398, 70, 137, 32),
                new Rectangle(398, 114, 137, 32),
                new Rectangle(398, 158, 137, 32),



                //new Rectangle(398, 209, 120, 170),

                new Rectangle(398, 26 + 185, 137, 32),
                new Rectangle(398, 70 + 185, 137, 32),
                new Rectangle(398, 114 + 185, 137, 32),
                new Rectangle(398, 158 + 185, 137, 32),

                //new Rectangle(398, 396, 120, 170),

                new Rectangle(398, 26 + 374, 137, 32),
                new Rectangle(398, 70 + 374, 137, 32),
                new Rectangle(398, 114 + 374, 137, 32),
                new Rectangle(398, 158 + 374, 137, 32),

                //new Rectangle(984, 23, 120, 170),

                new Rectangle(984, 26, 137, 32),
                new Rectangle(984, 70, 137, 32),
                new Rectangle(984, 114, 137, 32),
                new Rectangle(984, 158, 137, 32),

                //new Rectangle(984, 209, 120, 170),

                new Rectangle(984, 26 + 185, 137, 32),
                new Rectangle(984, 70 + 185, 137, 32),
                new Rectangle(984, 114 + 185, 137, 32),
                new Rectangle(984, 158 + 185, 137, 32),

                //new Rectangle(984, 396, 120, 170),

                new Rectangle(984, 26 + 374, 137, 32),
                new Rectangle(984, 70 + 374, 137, 32),
                new Rectangle(984, 114 + 374, 137, 32),
                new Rectangle(984, 158 + 374, 137, 32),


            };
            List<Rectangle> rects1 = new List<Rectangle>
            {
                new Rectangle(80, 90, 256, 100),
                new Rectangle(80, 277, 256, 100),
                new Rectangle(80, 464, 256, 100),

                new Rectangle(668, 90, 256, 100),
                new Rectangle(668, 277, 256, 100),
                new Rectangle(668, 464, 256, 100),

            };
            for (int i = 0; i < rects.Count; i++)
            {
                CropImage(bitmap, rects[i]).Save($"{_idx}.png");
                //CropImage(bitmap, rects1[i]).Save($"{_idx + 1}.png");
                result.Add((null, $"{_idx++}.png"));
                //result.Add(($"{_idx + 1}.png", $"{_idx++}.png"));
                //_idx++;
            }
            return result;

        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
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
        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        public static string GetText(string imgPath)
        {
            //return _ocrLite.Detect(imgPath, 70, 0, 0.618f, 0.3f, 2, true, true).StrRes;
            return _ocrLite.Detect(imgPath, 60, 512, 0.618f, 0.3f, 2, true, true).StrRes;
        }
    }
}
