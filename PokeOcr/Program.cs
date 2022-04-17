//// See https://aka.ms/new-console-template for more information
//using IronOcr;
//using System.Drawing;
//using OcrLiteLib;
////var Ocr = new IronTesseract();
////Ocr.Language = OcrLanguage.ChineseSimplifiedBest;
////string[] moveRegion = { "test2.jpg" };
////using (var input = new OcrInput("14.png"))
////{
////    input.DeNoise();
////    var Result = Ocr.Read(input).Text;
////    Console.WriteLine(Result);
////}
////return;

//OcrLite OcrLite = new();
////OcrLite.isDebugImg = false;
//OcrLite.InitModels("models/dbnet.onnx", "models/angle_net.onnx", "models/crnn_lite_lstm.onnx", "models/keys.txt", 12);
////OcrLite.InitModels("models/dbnet.tnnmodel", "models/angle_net.tnnmodel", "models/crnn_lite_lstm.tnnmodel", "models/keys1.txt", 12);
//string[] moveRegion = { "7.png", "8.png", "9.png", "10.png", "11.png", "12.png", "17.png" };

//List<string> resAll = new();
//foreach (var reg in moveRegion)
//{
//    var res = OcrLite.Detect(reg, 50, 0, 0.618f, 0.3f, 2, true, true);
//    //var res = OcrLite.Detect(reg, 50, 0, 0.618f, 0.3f, 2, true, true);
//    //var res = OcrLite.Detect(reg, 50, 1024, 0.618f, 0.3f, 2, true, true);
//    resAll.Add(res.StrRes);
//}

//foreach (var item in resAll)
//{
//    Console.Write(item.Trim());
//}
//return;
////Ocr.UseCustomTesseractLanguageFile
////using (var Input = new OcrInput())
////{
////    // Input.Deskew();  // use if image not straight
////    // Input.DeNoise(); // use if image contains digital noise
////    //Input.Contrast();
////    Input.AddImage("6.jpg", new Rectangle(395, 25, 147, 40));
////    Input.AddImage("6.jpg", new Rectangle(395, 65, 147, 40));
////    Input.AddImage("6.jpg", new Rectangle(395, 110, 147, 40));
////    Input.AddImage("6.jpg", new Rectangle(395, 145, 147, 40));



////    Input.AddImage("6.jpg", new Rectangle(390, 30 + 180, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 75 + 180, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 110 + 180, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 150 + 180, 105, 40));


////    Input.AddImage("6.jpg", new Rectangle(390, 400, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 75 + 360, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 110 + 360, 105, 40));
////    Input.AddImage("6.jpg", new Rectangle(390, 150 + 360, 105, 40));
////    var Result = Ocr.Read(Input);
////    Console.WriteLine(Result.Text);



////}