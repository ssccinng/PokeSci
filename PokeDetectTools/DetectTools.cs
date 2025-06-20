using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeDelectCV;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace PokeDetectTools
{
    public static class DetectTools
    {
        static PokeRegion[] _pokeRegions1 = {
            new(new Point(55 + 12, 114 + 12)), new(new Point(55 + 12, 295 + 12)), new(new Point(55 + 12, 474 + 12)),
            new(new Point(665 + 12, 114 + 12)), new(new Point(665 + 12, 295 + 12)), new(new Point(665 + 12, 474 + 12)),
        };

        /// <summary>
        /// 图片识别为文字（OCR），返回识别结果列表
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="langType">语言类型</param>
        /// <returns>识别出的文本列表</returns>
        public static List<string> ImageToText(string imgPath, LangType langType)
        {
            return PyExtensions.CallPaddleOcr(imgPath, langType);
        }

        /// <summary>
        /// 图片识别为文字（OCR），返回识别结果（合并为单个字符串）
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="langType">语言类型</param>
        /// <returns>识别出的文本</returns>
        public static string ImageToTextSingle(string imgPath, LangType langType)
        {
            var lines = PyExtensions.CallPaddleOcr(imgPath, langType);
            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// 识别宝可梦队伍图片，返回识别结果字符串
        /// </summary>
        /// <param name="imageBytes">图片字节流</param>
        /// <param name="lang">语言</param>
        /// <param name="pokeRegions">分割区域定义</param>
        /// <returns>识别结果</returns>
        public static async Task<string> DetectTeamImageAsync(
            byte[] imageBytes,
            string lang,
            Func<byte[], string> getTeraTypeFunc,
            Func<string, string, string?, string> translateHelperFunc)
        {
            var rect = DelectUtil.Delect(imageBytes);
            rect.Reverse();

            string path = $"TeamImage/{DateTime.Now.Ticks}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<string> teras = new();
            using var img = Image.Load(imageBytes);

            for (int i = 0; i < rect.Count; i++)
            {
                using var pokeimg = img.CloneAs<Rgba32>();
                pokeimg.Mutate(x => x.Crop(new Rectangle(rect[i].X, rect[i].Y, rect[i].Width, rect[i].Height)));
                pokeimg.Mutate(x => x.Resize(new Size(586, 160)));
                pokeimg.Mutate(x => x.Crop(new Rectangle(14, 14, 572, 146)));

                using var pokeimg_name = pokeimg.CloneAs<Rgba32>();
                pokeimg_name.Mutate(x => x.Crop(_pokeRegions1[i].Name));
                using var pokeimg_ability = pokeimg.CloneAs<Rgba32>();
                pokeimg_ability.Mutate(x => x.Crop(_pokeRegions1[i].Ability));
                using var pokeimg_item = pokeimg.CloneAs<Rgba32>();
                pokeimg_item.Mutate(x => x.Crop(_pokeRegions1[i].Item));
                using var pokeimg_move = pokeimg.CloneAs<Rgba32>();
                pokeimg_move.Mutate(x => x.Crop(_pokeRegions1[i].RectangleMove));
                using var pokeimg_tera = pokeimg.CloneAs<Rgba32>();
                pokeimg_tera.Mutate(x => x.Crop(_pokeRegions1[i].RectangleTera));

                string pathtemp = Path.GetTempFileName();
                await pokeimg_tera.SaveAsPngAsync(pathtemp);
                await pokeimg_name.SaveAsPngAsync(path + $"/Poke{i}_1name.png");
                await pokeimg_ability.SaveAsPngAsync(path + $"/Poke{i}_2ability.png");
                await pokeimg_item.SaveAsPngAsync(path + $"/Poke{i}_3item.png");
                await pokeimg_move.SaveAsPngAsync(path + $"/Poke{i}_4move.png");

                teras.Add(getTeraTypeFunc(File.ReadAllBytes(pathtemp)));
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
                            sb.AppendLine(translateHelperFunc(item, lang, null));
                            sb.AppendLine($"太晶属性: {teras[idxx++]}");
                            break;
                        case 2:
                            sb.AppendLine(translateHelperFunc(item, lang, "ability"));
                            break;
                        case 3:
                            sb.AppendLine(translateHelperFunc(item, lang, "item"));
                            break;
                        case 4:
                            sb.AppendLine(translateHelperFunc(item, lang, "waza"));
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        public class PokeDetectResult
        {
            public List<string> Names { get; set; } = new();
            public List<string> Abilities { get; set; } = new();
            public List<string> Items { get; set; } = new();
            public List<string> Moves { get; set; } = new();
            public List<string> TeraTypes { get; set; } = new();
        }

        /// <summary>
        /// 识别宝可梦队伍图片，返回结构化JSON风格数据
        /// </summary>
        /// <param name="imageBytes">图片字节流</param>
        /// <param name="lang">语言</param>
        /// <returns>结构化识别结果</returns>
        public static async Task<List<PokeDetectResult>> DetectTeamImageJsonAsync(byte[] imageBytes, string lang)
        {
            var rect = DelectUtil.Delect(imageBytes);

            string path = $"TeamImage/{DateTime.Now.Ticks}";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<string> teras = new();
            using var img = Image.Load(imageBytes);

            for (int i = 0; i < rect.Count; i++)
            {
                using var pokeimg = img.CloneAs<Rgba32>();
                pokeimg.Mutate(x => x.Crop(new Rectangle(rect[i].X, rect[i].Y, rect[i].Width, rect[i].Height)));
                pokeimg.Mutate(x => x.Resize(new Size(586, 160)));
                pokeimg.Mutate(x => x.Crop(new Rectangle(14, 14, 572, 146)));

                using var pokeimg_name = pokeimg.CloneAs<Rgba32>();
                pokeimg_name.Mutate(x => x.Crop(_pokeRegions1[i].Name));
                using var pokeimg_ability = pokeimg.CloneAs<Rgba32>();
                pokeimg_ability.Mutate(x => x.Crop(_pokeRegions1[i].Ability));
                using var pokeimg_item = pokeimg.CloneAs<Rgba32>();
                pokeimg_item.Mutate(x => x.Crop(_pokeRegions1[i].Item));
                using var pokeimg_move = pokeimg.CloneAs<Rgba32>();
                pokeimg_move.Mutate(x => x.Crop(_pokeRegions1[i].RectangleMove));
                using var pokeimg_tera = pokeimg.CloneAs<Rgba32>();
                pokeimg_tera.Mutate(x => x.Crop(_pokeRegions1[i].RectangleTera));

                string pathtemp = Path.GetTempFileName();
                await pokeimg_tera.SaveAsPngAsync(pathtemp);
                await pokeimg_name.SaveAsPngAsync(path + $"/Poke{i}_1name.png");
                await pokeimg_ability.SaveAsPngAsync(path + $"/Poke{i}_2ability.png");
                await pokeimg_item.SaveAsPngAsync(path + $"/Poke{i}_3item.png");
                await pokeimg_move.SaveAsPngAsync(path + $"/Poke{i}_4move.png");

                teras.Add(GetTeraType.GetTeraTypeML(File.ReadAllBytes(pathtemp)));
            }

            var list = PyExtensions.CallPaddleOcr(path, lang);
            List<PokeDetectResult> results = new();
            int type = 0;
            int idx = 0;
            PokeDetectResult? current = null;

            foreach (var item in list)
            {
                if (item.StartsWith("filePath"))
                {
                    if (item.EndsWith("name.png"))
                    {
                        current = new PokeDetectResult();
                        results.Add(current);
                        type = 1;
                    }
                    else if (item.EndsWith("ability.png"))
                    {
                        type = 2;
                    }
                    else if (item.EndsWith("item.png"))
                    {
                        type = 3;
                    }
                    else if (item.EndsWith("move.png"))
                    {
                        type = 4;
                    }
                }
                else if (current != null)
                {
                    switch (type)
                    {
                        case 1:
                            current.Names.Add(TranslateHelper.TranslateNameToChs(item, lang));
                            if (idx < teras.Count)
                                current.TeraTypes.Add(teras[idx++]);
                            break;
                        case 2:
                            current.Abilities.Add(TranslateHelper.TranslateNameToChs(item, lang, "ability"));
                            break;
                        case 3:
                            current.Items.Add(TranslateHelper.TranslateNameToChs(item, lang, "item"));
                            break;
                        case 4:
                            current.Moves.Add(TranslateHelper.TranslateNameToChs(item, lang, "waza"));
                            break;
                    }
                }
            }

            return results;
        }
    }
}
