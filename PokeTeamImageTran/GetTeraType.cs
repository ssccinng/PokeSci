using PokeTranslate;

namespace PokeTeamImageTran
{
    public class GetTeraType
    {
        public static string[] types = {"Bug", "Dark", "Dragon", "Electric", "Fairy", "Fight", "Fire", "Fly", "Ghost", "Grass", "Ground", "Ice", "Normal", "Posion", "Psychic", "Rock", "Steel", "Water"};
        public static Dictionary<string, string> TypeTable = new Dictionary<string, string>
        {
            {types[0], "虫"},
            {types[1], "恶"},
            {types[2], "龙"},
            {types[3], "电"},
            {types[4], "妖精"},
            {types[5], "格斗"},
            {types[6], "火"},
            {types[7], "飞行"},
            {types[8], "幽灵"},
            {types[9], "草"},
            {types[10], "地面"},
            {types[11], "冰"},
            {types[12], "一般"},
            {types[13], "毒"},
            {types[14], "超能力"},
            {types[15], "岩石"},
            {types[16], "钢"},
            {types[17], "水"},
        };
        public static string GetTeraTypeML(byte[] bytes)
        {
            //var imageBytes = File.ReadAllBytes(@"F:\VSProject\PokeSci\PokeOCRSV\bin\Debug\net7.0\TeamDataTera\TeraType\Bug\00001.jpg");
            MLModel.ModelInput sampleData = new MLModel.ModelInput()
            {
                ImageSource = bytes,
            };

            //Load model and predict output
            var result = MLModel.Predict(sampleData);
            return TypeTable[result.PredictedLabel];
        }
    }
}
