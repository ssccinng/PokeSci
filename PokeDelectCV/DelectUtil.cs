using OpenCvSharp;

namespace PokeDelectCV
{
    public static class DelectUtil
    {
        public static List<Rect> Delect(byte[] img)
        {

            //var image1 = Cv2.ImRead(@"C:\Users\scixing\Pictures\$SKLFUB3S}CJ{H$7HA5P[Y2.jpg");
            //var image1 = Cv2.ImRead(@"C:\Users\scixing\Pictures\PYO2HZ~L@NU8DUXD22XK3QD.png");
            var image = Cv2.ImDecode(img, ImreadModes.Color);
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2HSV);
            var lower = InputArray.Create([0, 0, 163]);
            var upper = InputArray.Create([179, 255, 255]);
            Cv2.InRange(image, lower, upper, image);

            Cv2.ImWrite("test.jpg", image);

            Cv2.Threshold(image, image, 127, 255, ThresholdTypes.BinaryInv);
            Cv2.FindContours(image, out Point[][] point, out var h, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            List<Rect> rects = [];
            foreach (var p in point)
            {
                var rect = Cv2.BoundingRect(p);
                var ration = rect.Width / (double)rect.Height;
                if (ration > 3 && ration < 5)
                { 
                    rects.Add(rect);
                }
            } 
            rects = rects.OrderByDescending(s => s.Height / 10).Take(6).ToList();
            return rects;
            // Delect
        }
    }
}
