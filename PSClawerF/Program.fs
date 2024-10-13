// For more information see https://aka.ms/fsharp-console-apps
open PokePSCore
let psClient = PSClient ("testt", "")

let getRuleRoomAsync rule = 
    let room = psClient.GetRoomListAsync ()
    printfn "%A" room

while true do
    let result = psClient.GetPokemonAsync("pikachu").Result
    printfn "%A" result
    System.Threading.Thread.Sleep(1000)