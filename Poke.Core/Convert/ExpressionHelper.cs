namespace Poke.Core.Convert;

public class ExpressionHelper
{
    /// <summary>
    /// 用于转换程序用模型和数据库模型
    /// </summary>
    /// <typeparam name="Tin"></typeparam>
    /// <typeparam name="Tout"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Func<Tin, Tout> Map<Tin, Tout>()
    {
        throw new NotImplementedException();
    }
}