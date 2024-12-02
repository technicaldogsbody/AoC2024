open AdventOfCode2024.Common
open System.Text.RegularExpressions

let parseInputFile (filename: string) =
    let leftList = ResizeArray<int>()
    let rightList = ResizeArray<int>()
    
    // Read the file and split into two lists with validation
    FileService.getFileAsArray(filename)
    |> Seq.iter (fun line ->
        let parts = Regex.Split(line.Trim(), "\s+") |> Array.map (fun part ->
            match System.Int32.TryParse(part) with
            | (true, value) -> Some value
            | (false, _) -> None
        )
        if parts.Length = 2 && parts.[0].IsSome && parts.[1].IsSome then
            leftList.Add(parts.[0].Value)
            rightList.Add(parts.[1].Value)
        else
            printfn "Warning: Skipping invalid line '%s'" line
    )
    
    leftList, rightList

let calculateTotalDistance (leftList: ResizeArray<int>) (rightList: ResizeArray<int>) =
    // Sort both lists
    let leftSorted = leftList |> Seq.sort |> Seq.toList
    let rightSorted = rightList |> Seq.sort |> Seq.toList
    
    // Calculate the sum of absolute differences
    let totalDistance =
        List.zip leftSorted rightSorted
        |> List.sumBy (fun (left, right) -> abs (left - right))
    
    totalDistance

let calculateSimilarityScore (leftList: ResizeArray<int>) (rightList: ResizeArray<int>) =
    // Create a frequency map for the right list
    let rightFrequencyMap =
        rightList
        |> Seq.countBy id
        |> dict
    
    // Calculate the similarity score based on occurrences in the right list
    let similarityScore =
        leftList
        |> Seq.sumBy (fun left ->
            let count = if rightFrequencyMap.ContainsKey(left) then rightFrequencyMap.[left] else 0
            left * count
        )
    
    similarityScore


[<EntryPoint>]
let main argv =
    // Parse input file
    let leftList, rightList = parseInputFile "input.txt"
    
    // Calculate total distance
    let totalDistance = calculateTotalDistance leftList rightList
    let similarityScore = calculateSimilarityScore leftList rightList

    // Define tasks for FancyConsole
    let tasks = 
        [ "Calculate Total Distance", (fun () -> totalDistance :> obj)
          "Calculate Similarity Score", (fun () -> similarityScore :> obj) ]
        
    // Output the result using FancyConsole
    FancyConsole.writeInfo "Historian Hysteria" tasks
    0