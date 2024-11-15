open System
open AdventOfCode2024.Common

let fileContent = FileService.getFileAsString("Test.txt")

printfn "%s" fileContent

let tasks = 
    [ "Parse Input", (fun () -> fileContent :> obj)
      "Calculate Part 1", (fun () -> 1234 :> obj)
      "Calculate Part 2", (fun () -> 56789 :> obj) ]

FancyConsole.writeInfo "Advent of Code - Day 1" tasks

let input = Console.ReadLine()

