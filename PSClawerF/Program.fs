open PsRoomListener.ListenBot
open System

let rules = [|  "gen9vgc2024regh";"gen9vgc2024reghbo3";"gen9vgc2024regg";"gen9vgc2024reggbo3" |]

let bot = Bot("gen9vgc2024regh", None)

bot.initRoomListener()

Console.ReadLine() |> ignore