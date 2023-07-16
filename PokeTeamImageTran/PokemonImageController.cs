using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;

namespace PokeTeamImageTran;
[Route("api/[controller]")]
[ApiController]
public class PokemonImageController : ControllerBase
{
    PokeOCRSV.PokeRegion[] _pokeRegions = {
        new(new Point(55, 114)), new(new Point(55, 295)), new(new Point(55, 474)),
        new(new Point(665, 114)), new(new Point(665, 295)), new(new Point(665, 474)),

    };

    [HttpPost("GetImageData1")]
    public async Task<string> Get11()
    {
        //using StreamReader streamReader = new StreamReader(Request.Body);
        //var bytes1 = await streamReader.ReadToEndAsync();
        var bytes = new byte[(int)Request.ContentLength];
        //try
        //{
        //    var aa1a = Request.Body.Read(bytes, 0, (int)Request.ContentLength);

        //}
        //catch (Exception ee)
        //{

        //    throw;
        //}
        await Request.Body.CopyToAsync(new MemoryStream(bytes));

        string path = $"NovalImage/{DateTime.Now.Ticks}.png";

        if (!Directory.Exists("NovalImage"))
        {
            Directory.CreateDirectory("NovalImage");
        }
        System.IO.File.WriteAllBytes(path, bytes);
        return Environment.CurrentDirectory + "/" + path;
    }
        [HttpPost("GetImageData/{lang}")]
     public  async Task<object> Get(string lang)
    {
        //using StreamReader streamReader = new StreamReader(Request.Body);
        //var bytes1 = await streamReader.ReadToEndAsync();
         //Console.WriteLine(lang);
         //Console.WriteLine(Request.ContentLength);
        var bytes = new byte[(int)Request.ContentLength];
        //try
        //{
        //    var aa1a = Request.Body.Read(bytes, 0, (int)Request.ContentLength);

        //}
        //catch (Exception ee)
        //{

        //    throw;
        //}
        await Request.Body.CopyToAsync(new MemoryStream(bytes));
      
        string path = $"TeamImage/{DateTime.Now.Ticks}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //System.IO.File.WriteAllBytes(path + "/Test.png", bytes);
        //File.Writeto;
        
        using var img = Image.Load(bytes);
        if (img == null)
        {
            return "图片为空";
        }

        List<string> teras = new();

        if (img.Width != 1280 || img.Height != 720)
            img.Mutate(s => s.Resize(new Size(1280, 720)));
        //img.Mutate(x => x.Crop(new Rectangle(100,100, 100, 100)));
        //img.SaveAsPng(path + "/Test1.png");
        for (int i = 0; i < 6; i++)
        {
            using var pokeimg = img.CloneAs<Rgba32>();
            pokeimg.Mutate(x => x.Crop(new Rectangle(
                _pokeRegions[i].BasePoint.X, _pokeRegions[i].BasePoint.Y,
                _pokeRegions[i].Width, _pokeRegions[i].Height)));
            using var pokeimg_name = pokeimg.CloneAs<Rgba32>();
            pokeimg_name.Mutate(x => x.Crop(_pokeRegions[i].Name));
            using var pokeimg_ability = pokeimg.CloneAs<Rgba32>();
            pokeimg_ability.Mutate(x => x.Crop(_pokeRegions[i].Ability));
            //try
            //{
            //    var pokeimg_item1 = pokeimg.CloneAs<Rgba32>();
            //    pokeimg_item1.Mutate(x => x.Crop(_pokeRegions[i].Item));
            //}
            //catch (Exception ee)
            //{

            //    throw;
            //}
            using var pokeimg_item = pokeimg.CloneAs<Rgba32>();
            pokeimg_item.Mutate(x => x.Crop(_pokeRegions[i].Item));

            using var pokeimg_move = pokeimg.CloneAs<Rgba32>();
            pokeimg_move.Mutate(x => x.Crop(_pokeRegions[i].RectangleMove));

            using var pokeimg_tera = pokeimg.CloneAs<Rgba32>();
            pokeimg_tera.Mutate(x => x.Crop(_pokeRegions[i].RectangleTera));
            string pathtemp = Path.GetTempFileName();
            await pokeimg_tera.SaveAsPngAsync(pathtemp);
            await pokeimg_name.SaveAsPngAsync(path + $"/Poke{i}_1name.png");
            await pokeimg_ability.SaveAsPngAsync(path + $"/Poke{i}_2ability.png");
            await pokeimg_item.SaveAsPngAsync(path + $"/Poke{i}_3item.png");
            await pokeimg_move.SaveAsPngAsync(path + $"/Poke{i}_4move.png");
            //await pokeimg_tera.SaveAsPngAsync(path + $"/Poke{i}_5tera.png");
            unsafe
            {

                teras.Add(GetTeraType.GetTeraTypeML(System.IO.File.ReadAllBytes(pathtemp)));

            }

            //return;
        }

        var list = PyExtensions.CallPaddleOcr(path, lang);
        StringBuilder sb = new StringBuilder();
        int type = 0;
        int idxx = 0;
        foreach (var item in list)
        {
            if (item.StartsWith("filePath"))
            {
                if (item.EndsWith("name.png"))
                {
                    sb.Append("\n宝可梦名: ");
                    type = 1;
                }
                else if (item.EndsWith("ability.png"))
                {
                    sb.Append("特性: ");
                    type = 2;
                }
                else if (item.EndsWith("item.png"))
                {
                    sb.Append("道具: ");
                    type = 3;
                }
                else if (item.EndsWith("move.png"))
                {
                    sb.AppendLine("招式: ");
                    type = 4;
                }

                
            }
            else
            {
                switch (type)
                {
                    case 1:
                        sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang));
                        sb.AppendLine($"太晶属性: {teras[idxx++]}");
                        break;
                    case 2:
                        sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang, "ability"));
                        break;
                    case 3:
                        sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang, "item"));
                        break;
                    case 4:
                        sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang, "waza"));
                        break;
                    default:
                        break;
                }

            }

        }

        return sb.ToString();
    }
    [HttpPost("GetImageJson/{lang}")]
    public async Task<object> Get1(string lang)
    {
        //using StreamReader streamReader = new StreamReader(Request.Body);
        //var bytes1 = await streamReader.ReadToEndAsync();
        var bytes = new byte[(int)Request.ContentLength];
        //try
        //{
        //    var aa1a = Request.Body.Read(bytes, 0, (int)Request.ContentLength);

        //}
        //catch (Exception ee)
        //{

        //    throw;
        //}
        await Request.Body.CopyToAsync(new MemoryStream(bytes));
        List<string> teras = new();

        string path = $"TeamImage/{DateTime.Now.Ticks}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //System.IO.File.WriteAllBytes(path + "/Test.png", bytes);
        //File.Writeto;

        using var img = Image.Load(bytes);
        if (img == null)
        {
            return "图片为空";
        }
        if (img.Width != 1280 || img.Height != 720)
            img.Mutate(s => s.Resize(new Size(1280, 720)));
        //img.Mutate(x => x.Crop(new Rectangle(100,100, 100, 100)));
        //img.SaveAsPng(path + "/Test1.png");
        for (int i = 0; i < 6; i++)
        {
            using var pokeimg = img.CloneAs<Rgba32>();
            pokeimg.Mutate(x => x.Crop(new Rectangle(
                _pokeRegions[i].BasePoint.X, _pokeRegions[i].BasePoint.Y,
                _pokeRegions[i].Width, _pokeRegions[i].Height)));
            using var pokeimg_name = pokeimg.CloneAs<Rgba32>();
            pokeimg_name.Mutate(x => x.Crop(_pokeRegions[i].Name));
            using var pokeimg_ability = pokeimg.CloneAs<Rgba32>();
            pokeimg_ability.Mutate(x => x.Crop(_pokeRegions[i].Ability));
            //try
            //{
            //    var pokeimg_item1 = pokeimg.CloneAs<Rgba32>();
            //    pokeimg_item1.Mutate(x => x.Crop(_pokeRegions[i].Item));
            //}
            //catch (Exception ee)
            //{

            //    throw;
            //}
            using var pokeimg_item = pokeimg.CloneAs<Rgba32>();
            pokeimg_item.Mutate(x => x.Crop(_pokeRegions[i].Item));

            using var pokeimg_move = pokeimg.CloneAs<Rgba32>();

            using var pokeimg_tera = pokeimg.CloneAs<Rgba32>();
            pokeimg_tera.Mutate(x => x.Crop(_pokeRegions[i].RectangleTera));
            string pathtemp = Path.GetTempFileName();
            await pokeimg_tera.SaveAsPngAsync(pathtemp);

            pokeimg_move.Mutate(x => x.Crop(_pokeRegions[i].RectangleMove));
            await pokeimg_name.SaveAsPngAsync(path + $"/Poke{i}_1name.png");
            await pokeimg_ability.SaveAsPngAsync(path + $"/Poke{i}_2ability.png");
            await pokeimg_item.SaveAsPngAsync(path + $"/Poke{i}_3item.png");
            await pokeimg_move.SaveAsPngAsync(path + $"/Poke{i}_4move.png");

            teras.Add(GetTeraType.GetTeraTypeML(System.IO.File.ReadAllBytes(pathtemp)));


            //return;
        }

        var list = PyExtensions.CallPaddleOcr(path, lang);
        StringBuilder sb = new StringBuilder();
        List<List<string>[]> ress = new ();
        int type = 0;
        int idx = 0;
        foreach (var item in list)
        {
            if (item.StartsWith("filePath"))
            {
                if (item.EndsWith("name.png"))
                {
                    ress.Add(new List<string>[5]);
                    ress.Last()[0] = new();
                    sb.Append("\n宝可梦名: ");
                    ress.Last()[4] = new();

                    type = 1;
                }
                else if (item.EndsWith("ability.png"))
                {
                    ress.Last()[1] = new();

                    sb.Append("特性: ");
                    type = 2;
                }
                else if (item.EndsWith("item.png"))
                {
                    ress.Last()[2] = new();

                    sb.Append("道具: ");
                    type = 3;
                }
                else if (item.EndsWith("move.png"))
                {
                    ress.Last()[3] = new();
                    sb.AppendLine("招式: ");
                    type = 4;
                }


            }
            else
            {
                switch (type)
                {
                    case 1:
                        //sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang));
                        ress.Last()[0].Add(TranslateHelper.TranslateNameToChs(item, lang));
                        ress.Last()[4].Add(teras[idx++]);

                        break;
                    case 2:
                        //sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang, "ability"));
                        ress.Last()[1].Add(TranslateHelper.TranslateNameToChs(item, lang, "ability"));

                        break;
                    case 3:
                        ress.Last()[2].Add(TranslateHelper.TranslateNameToChs(item, lang, "item"));
                        break;
                    case 4:
                        //sb.AppendLine(TranslateHelper.TranslateNameToChs(item, lang, "waza"));
                        ress.Last()[3].Add(TranslateHelper.TranslateNameToChs(item, lang, "waza"));
                        break;
                    default:
                        break;
                }

            }

        }

        return ress;
    }
    //public 
}
