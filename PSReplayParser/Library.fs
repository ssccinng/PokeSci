namespace PSReplayParser

open System

module PSReplayParser =

    type WhoWin =
        | Player1Win
        | Player2Win
        | Draw

    type Poke = { Name: string; Level: int; Sex: string }

    type PsReplayInfo =  {
        RoomId: string
        P1: string
        P2: string
        P1Avatar: string
        P2Avatar: string

        P1Score: int
        P2Score: int

        Winner: WhoWin
        Turn: int
        P1Team: array<Poke>
        P2Team: array<Poke>
        Replay: array<string>

        Tier: string
    }


    let Parser (data:string) : PsReplayInfo = 
        
        let lines = 
            data.Split('\n') 
                |> Array.map (fun x -> x.Trim()) 
                |> Array.filter (fun x -> x <> "") 

        


        let mutable roomId = lines.[0].[1..]
        let mutable p1 = ""
        let mutable p2 = ""
        let mutable p1Avatar = ""
        let mutable p2Avatar = ""
        let mutable p1Score = 0
        let mutable p2Score = 0
        let mutable winner = WhoWin.Draw // 默认值
        let mutable turn = 0
        let mutable p1Team = []
        let mutable p2Team = []
        let mutable replay = lines
        let mutable tier = ""



        for line in lines do
            let parts = line.Split('|')
            if parts.Length > 1 then
                match parts.[1] with
                | "player" -> 
                    if parts.Length > 5 then
                        let playerScoreStr = parts.[5].Trim() // 获取并修剪分数字符串

                        let scoreOpt = 
                            match System.Int32.TryParse(playerScoreStr) with
                            | true, score -> Some score // 转换成功，返回 Some(score)
                            | false, _ -> None // 转换失败，返回 None
                        if parts.[2] = "p1" then
                            p1 <- parts.[3]
                            p1Avatar <- parts.[4]
                            p1Score <- 
                                if scoreOpt.IsNone then
                                    0
                                else
                                    scoreOpt.Value
                        else
                            p2 <- parts.[3]
                            p2Avatar <- parts.[4]
                            p2Score <- 
                                if scoreOpt.IsNone then
                                    0
                                else
                                    scoreOpt.Value
                | "tier" -> 
                    tier <- parts.[2]
                | "win" -> 
                    winner <- 
                        if parts.[2] = p1 then
                            WhoWin.Player1Win
                        elif parts.[2] = p2 then
                            WhoWin.Player2Win
                        else
                            WhoWin.Draw
                | "poke" ->
                    if parts.Length > 3 then
                        let pokedata = parts.[3].Split(',')

                        let poke = {
                            Name = pokedata.[0].Trim()
                            Level = int pokedata.[1].[2..]
                            Sex = 
                                if pokedata.Length > 2 then
                                    pokedata.[2]
                                else
                                    ""
                         }

                        if parts.[2] = "p1" then
                            p1Team <- poke :: p1Team
                        else 
                            p2Team <- poke :: p2Team

                | "turn" -> 
                    turn <- int parts.[2]
                | _ -> ()

        {
            RoomId = roomId
            P1 = p1
            P2 = p2
            P1Avatar = p1Avatar
            P2Avatar = p2Avatar
            Winner = winner
            Turn = turn
            P1Team = p1Team |> List.toArray
            P2Team = p2Team |> List.toArray
            Replay = replay
            P1Score = p1Score
            P2Score = p2Score
            Tier = tier
        }
        



