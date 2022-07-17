using System;

namespace PSBattleHelper.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
