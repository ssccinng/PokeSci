namespace PsRoomListener

open PokePSCore
open PSReplayParser
open PSReplayParser.PSReplayParser

module ListenBot = 
    open System.Collections.Generic
    type Bot(rule: string, score: int option) =
        let psClient = PSClient ("testt", "")
        let mutable roomSet = set []

        let connectAndLoginAsync() =
            async {
                do! psClient.ConnectAsync() |> Async.AwaitTask
                let! _ = psClient.LoginGuestAsync() |> Async.AwaitTask
                printfn "Connected and logged in successfully!"
            }

        do connectAndLoginAsync() |> Async.RunSynchronously

        let loopToSendGetRoom() =
            async {
                let scoreLimit = 
                    match score with
                        | Some s -> s
                        | None -> -1
                while true do
                    do! psClient.GetRoomListAsync(rule, scoreLimit)
                        |> Async.AwaitTask

                    do! Async.Sleep 10000
                    
            }

        member this.initRoomListener() =
            psClient.add_OnGetRoomList (
                fun e -> 
                    async {
                        let newRoomSet = 
                            e.rooms.Keys 
                            |> Seq.map string
                            |> set

                        let diff = 
                            newRoomSet |> Set.difference roomSet

                        for key in diff do
                            do! psClient.SendJoinAsync key |> Async.AwaitTask

                            // Process `res` if needed
                            do! Async.Sleep 1000 // Replace Thread.Sleep with Async.Sleep to avoid blocking
                            do! psClient.SendLeaveAsync key |> Async.AwaitTask
                            do! Async.Sleep 1000

                        roomSet <- newRoomSet
                    }
                    |> Async.Start // Start the async operation non-blocking
            )

            psClient.add_BattleRoomData(
                fun e -> 
                    printfn "BattleRoomData: %A" e
                    printfn "BattleRoomData: %A" (e |> PSReplayParser.Parser)
                    // Process `e` if needed
                    ()
            )

            do loopToSendGetRoom() |> Async.Start


    type MuitBot(rules: string array, score: int option) =
        let psClient = PSClient ("testt", "")

        let mutable roomSetDict = Dictionary<string, Set<string>>()

        do for rule in rules do
            roomSetDict.Add(rule, set [])

        let mutable nowRule = ""

        let connectAndLoginAsync() =
            async {
                do! psClient.ConnectAsync() |> Async.AwaitTask
                let! _ = psClient.LoginGuestAsync() |> Async.AwaitTask
                printfn "Connected and logged in successfully!"
            }
        

        do connectAndLoginAsync() |> Async.RunSynchronously

        let loopToSendGetRoom() =
            async {
                let scoreLimit = 
                    match score with
                        | Some s -> s
                        | None -> -1
                while true do

                    for rule in rules do
                        nowRule <- rule
                        do! psClient.GetRoomListAsync(rule, scoreLimit)
                            |> Async.AwaitTask

                        do! Async.Sleep 10000
                    
            }
        member val event = new Event<PsReplayInfo>()

        member this.initRoomListener() =
            psClient.add_OnGetRoomList (
                fun e -> 
                    async {
                        let newRoomSet = 
                            e.rooms.Keys 
                            |> Seq.map string
                            |> set

                        let diff = 
                            newRoomSet |> Set.difference roomSetDict.[nowRule]
                        roomSetDict.[nowRule] <- newRoomSet

                        for key in diff do
                            do! psClient.SendJoinAsync key |> Async.AwaitTask

                            // Process `res` if needed
                            do! Async.Sleep 1000 // Replace Thread.Sleep with Async.Sleep to avoid blocking
                            do! psClient.SendLeaveAsync key |> Async.AwaitTask
                            do! Async.Sleep 1000

                    }
                    |> Async.Start // Start the async operation non-blocking
            )

            psClient.add_BattleRoomData(
                fun e -> 
                    printfn "BattleRoomData: %A" e
                    //printfn "BattleRoomData: %A" ()
                    // Process `e` if needed
                    e  |> PSReplayParser.Parser |> this.event.Trigger
            )

            

            do loopToSendGetRoom() |> Async.Start