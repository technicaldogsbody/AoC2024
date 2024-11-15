open System
open AdventOfCode2024.Common

// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

Console.WriteLine("Hello from Console")

printfn "%s" (FileService.getFileAsString("Test.txt"))

let input = Console.ReadLine()

