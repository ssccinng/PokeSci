// For more information see https://aka.ms/fsharp-console-apps
open PokePSCore

let rules = [|  "gen9vgc2024regh";"gen9vgc2024reghbo3";"gen9vgc2024regg";"gen9vgc2024reggbo3" |]

let psClient = PSClient ("testt", "")




let connectAndLoginAsync() =
    async {
        
        // 等待 ConnectAsync 完成
        do! psClient.ConnectAsync() |> Async.AwaitTask
        
        // 等待 LoginGuestAsync 完成
        let! _ = psClient.LoginGuestAsync() |> Async.AwaitTask

        printfn "Connected and logged in successfully!"
    }

connectAndLoginAsync() |> Async.RunSynchronously

let getRuleRoomAsync rule = 
    let room = psClient.GetRoomListAsync ()
    printfn "%A" room


let roomSet = set 

psClient.add_OnGetRoomList (
    fun e -> 
        let roomIds = 
            e.rooms.Keys 
            |> Seq.map string

        for key in e.rooms.Keys do
            let joinRoom = async { 
                let! res = psClient.SendJoinAsync key |> Async.AwaitTask
                return res;
            }

            joinRoom |> Async.RunSynchronously
            System.Threading.Thread.Sleep(1000)
            

            // 看看对战
        printfn "RoomList: %A" 
            (roomIds |> String.concat ", ")


        //printfn "RoomList: %A" 
        //    (keysString |> String.concat ", ")
    )
while true do
    for rule in rules do
        let getRoom = async { 
            let! res = psClient.GetRoomListAsync rule |> Async.AwaitTask
            return res;
        }
        getRoom |> Async.RunSynchronously
        System.Threading.Thread.Sleep(10000)
