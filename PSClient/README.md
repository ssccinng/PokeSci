# PSClient - Pokemon Showdown 客户端库

## 概述

PSClient 是一个现代化的 Pokemon Showdown 客户端库，基于 .NET 9.0 构建。它提供了与 Pokemon Showdown 服务器交互的完整功能，包括对战管理、用户认证、房间管理等。

## 主要特性

- **现代化架构**: 基于 .NET 9.0，使用最新的 C# 特性
- **异步支持**: 全面支持异步操作
- **类型安全**: 强类型的数据模型和API
- **扩展性**: 模块化设计，易于扩展
- **异常处理**: 完整的异常处理机制
- **事件驱动**: 基于事件的消息处理

## 快速开始

### 1. 创建客户端实例

```csharp
using PSClient.Client;

var client = new PSClient("your_username", "your_password");
```

### 2. 连接和登录

```csharp
// 连接到服务器
await client.ConnectAsync();

// 登录（可选，游客模式）
await client.LoginAsGuestAsync();

// 或者使用账号密码登录
await client.LoginAsync();
```

### 3. 设置事件处理

```csharp
// 对战开始事件
client.OnBattleStart += (battle) => 
{
    Console.WriteLine($"Battle started in room: {battle.RoomTag}");
};

// 需要选择行动时
client.OnRequestAction += async (battle) => 
{
    // 使用第一个招式
    await battle.SendMoveAsync(PSClientExtensions.Move(1));
};

// 对战结束事件
client.OnBattleEnd += (battle, isWin) => 
{
    Console.WriteLine($"Battle ended. Result: {(isWin ? "Win" : "Loss")}");
};
```

### 4. 搜索对战

```csharp
// 搜索随机对战
await client.SearchBattleAsync("gen9randombattle");
```

## 核心类说明

### PSClient

主要的客户端类，负责与 Pokemon Showdown 服务器的连接和通信。

```csharp
// 主要方法
await client.ConnectAsync();                    // 连接服务器
await client.LoginAsync();                      // 登录
await client.SearchBattleAsync("format");       // 搜索对战
await client.ChallengeAsync("player", "format"); // 发起挑战
await client.DisconnectAsync();                 // 断开连接
```

### PSBattle

对战管理类，代表一个具体的对战实例。

```csharp
// 主要属性
battle.RoomTag                    // 房间标识
battle.PlayerPosition             // 玩家位置 (Player1/Player2)
battle.MyTeam                     // 我方队伍
battle.OpponentTeam              // 对手队伍
battle.Turn                      // 当前回合

// 主要方法
await battle.SendMoveAsync(move);           // 发送招式
await battle.SendTeamOrderAsync(order);     // 发送队伍排序
await battle.ForfeitAsync();                // 投降
```

### ChooseData 系列

用于表示对战中的选择数据：

```csharp
// 招式选择
var move = new MoveChooseData(1, 999, false); // 招式1，默认目标，不极巨化
var moveWithTarget = new MoveChooseData(2, 1); // 招式2，目标位置1
var dynamaxMove = new MoveChooseData(3, 999, true); // 招式3，极巨化

// 交换宝可梦
var switch = new SwitchData(2); // 交换到队伍中的第2只宝可梦

// 跳过
var pass = new MoveChooseData(0) { IsPass = true };
```

## 扩展方法

库提供了便捷的扩展方法：

```csharp
using PSClient.Extensions;

// 快速创建选择数据
var move = PSClientExtensions.Move(1);           // 招式1
var switch = PSClientExtensions.Switch(2);       // 交换到宝可梦2
var pass = PSClientExtensions.Pass();            // 跳过

// 对战信息查询
var activePokemon = battle.GetMyActivePokemon();        // 获取我方场上宝可梦
var alivePokemon = battle.GetMyAlivePokemon();          // 获取我方存活宝可梦
var canUse = battle.CanUseMove(1);                      // 检查是否可以使用招式1
var canSwitch = battle.CanSwitchTo(2);                  // 检查是否可以交换到宝可梦2
var stats = battle.GetBattleStats();                    // 获取对战统计
```

## 事件处理

### 对战相关事件

```csharp
client.OnBattleStart += (battle) => { /* 对战开始 */ };
client.OnBattleEnd += (battle, isWin) => { /* 对战结束 */ };
client.OnRequestAction += (battle) => { /* 需要选择行动 */ };
client.OnTeamPreview += (battle) => { /* 队伍预览阶段 */ };
client.OnForceSwitch += (battle, switches) => { /* 强制交换 */ };
```

### 用户相关事件

```csharp
client.OnChallenge += (challenger, format) => { /* 收到挑战 */ };
client.OnChat += (user, message) => { /* 收到聊天消息 */ };
client.OnUserUpdate += () => { /* 用户信息更新 */ };
```

## 异常处理

库定义了专门的异常类型：

```csharp
try
{
    await client.ConnectAsync();
}
catch (PSConnectionException ex)
{
    // 连接异常
    Console.WriteLine($"Connection failed: {ex.Message}");
}
catch (PSAuthenticationException ex)
{
    // 认证异常
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (PSBattleException ex)
{
    // 对战异常
    Console.WriteLine($"Battle error in room {ex.RoomId}: {ex.Message}");
}
```

## 完整示例

```csharp
using PSClient.Client;
using PSClient.Extensions;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new PSClient("username", "password");
        
        // 设置日志
        client.LogTo(Console.WriteLine);
        
        // 设置事件处理
        client.OnBattleStart += (battle) => 
        {
            Console.WriteLine($"Battle started: {battle.RoomTag}");
        };
        
        client.OnRequestAction += async (battle) => 
        {
            // 简单的AI：总是使用第一个招式
            if (battle.CanUseMove(1))
            {
                await battle.SendMoveAsync(PSClientExtensions.Move(1));
            }
            else
            {
                // 如果不能使用招式，尝试交换
                for (int i = 2; i <= 6; i++)
                {
                    if (battle.CanSwitchTo(i))
                    {
                        await battle.SendMoveAsync(PSClientExtensions.Switch(i));
                        break;
                    }
                }
            }
        };
        
        client.OnBattleEnd += (battle, isWin) => 
        {
            Console.WriteLine($"Battle ended. Result: {(isWin ? "Win" : "Loss")}");
        };
        
        try
        {
            // 连接和登录
            await client.ConnectAsync();
            await client.LoginAsync();
            
            // 搜索对战
            await client.SearchBattleAsync("gen9randombattle");
            
            // 保持运行
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync();
            client.Dispose();
        }
    }
}
```

## 注意事项

1. **资源管理**: PSClient 实现了 IDisposable，使用完毕后请调用 Dispose() 方法
2. **异步操作**: 所有网络操作都是异步的，请使用 await 关键字
3. **事件处理**: 事件处理程序中的异常不会传播到调用方，请妥善处理
4. **连接状态**: 在进行操作前确保客户端已连接到服务器
5. **API限制**: 遵守 Pokemon Showdown 的使用条款和API限制

## 项目结构

```
PSClient/
├── Core/                    # 核心数据类型
│   ├── ChooseData.cs       # 选择数据基类
│   ├── MoveChooseData.cs   # 招式选择数据
│   └── SwitchData.cs       # 交换选择数据
├── Models/                  # 数据模型
│   ├── BattleData.cs       # 对战数据模型
│   ├── PSBattlePokemon.cs  # 对战宝可梦模型
│   └── UserModels.cs       # 用户相关模型
├── Client/                  # 客户端实现
│   ├── PSClient.cs         # 主客户端类
│   └── PSClientCommands.cs # 命令扩展
├── Battle/                  # 对战管理
│   ├── PSBattle.cs         # 对战管理类
│   └── PSBattleLogic.cs    # 对战逻辑处理
├── Extensions/              # 扩展方法
│   └── PSClientExtensions.cs
├── Utilities/               # 工具类
│   └── PSTools.cs
└── Exceptions/              # 异常定义
    └── PSClientExceptions.cs
```
