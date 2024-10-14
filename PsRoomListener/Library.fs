namespace PsRoomListener

open PokePSCore

module ListenBot = 
    type Bot(rule: string, score: option<int>) =
        let psClient = PSClient ("testt", "")
        let mutable roomSet = set<string>()

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
            }

        let initRoomListener() =
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
                            let! res = psClient.SendJoinAsync key |> Async.AwaitTask
                            // Process `res` if needed
                            do! Async.Sleep 1000 // Replace Thread.Sleep with Async.Sleep to avoid blocking
                    }
                    |> Async.Start // Start the async operation non-blocking



            )

            do loopToSendGetRoom() |> Async.Start
