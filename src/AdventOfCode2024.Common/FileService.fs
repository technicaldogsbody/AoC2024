namespace AdventOfCode2024.Common

open System
open System.IO
open System.Text.RegularExpressions

module FileService =

    let getFileAsString (fileName: string) : string =
        File.ReadAllText($"Data/{fileName}")

    let getFileAsArray (fileName: string) (delimiterPattern: string) : seq<string> =
        File.ReadAllText($"Data/{fileName}")
        |> fun contents -> Regex.Split(contents, delimiterPattern)
        |> Seq.filter (fun line -> not (String.IsNullOrWhiteSpace(line)))

    let getFileAs2dIntArray (fileName: string) (rowDelimiterPattern: string) : int[,] =
        let rows = 
            File.ReadAllText($"Data/{fileName}")
            |> fun contents -> Regex.Split(contents, rowDelimiterPattern)
            |> Array.filter (fun row -> not (String.IsNullOrWhiteSpace(row)))

        if rows.Length = 0 then
            Array2D.zeroCreate 0 0
        else
            let height = rows.Length
            let width = rows.[0].Length
            Array2D.init height width (fun i j -> int (rows.[i].[j].ToString()))

    let getFileAsListOfListOfInt (fileName: string) (rowDelimiterPattern: string) : list<list<int>> =
        let rows = 
            File.ReadAllText($"Data/{fileName}")
            |> fun contents -> Regex.Split(contents, rowDelimiterPattern)
            |> Array.filter (fun row -> not (String.IsNullOrWhiteSpace(row)))

        if rows.Length = 0 then
            []
        else
            rows
            |> Array.map (fun row -> row.Split(" ") |> Array.map int |> Array.toList)
            |> Array.toList
