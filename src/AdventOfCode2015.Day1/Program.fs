open AdventOfCode2024.Common
open System

let calculateFloor (instructions: string) =
    instructions
    |> Seq.fold (fun floor char ->
        match char with
        | '(' -> floor + 1
        | ')' -> floor - 1
        | _ -> floor
    ) 0

let findBasementPosition (instructions: string) =
    let rec helper (chars: char list) (floor: int) (position: int) =
        match chars with
        | [] -> -1
        | c :: rest ->
            let newFloor =
                match c with
                | '(' -> floor + 1
                | ')' -> floor - 1
                | _ -> floor
            if newFloor = -1 then position
            else helper rest newFloor (position + 1)
    helper (Seq.toList instructions) 0 1

let input = FileService.getFileAsString("input.txt")

let tasks = 
    [ "Calculate Floor", (fun () -> calculateFloor input :> obj)
      "Calculate Basement", (fun () -> findBasementPosition input :> obj) ]

FancyConsole.writeInfo "AoC 2015 - Day 1" tasks


let i = Console.ReadLine()

