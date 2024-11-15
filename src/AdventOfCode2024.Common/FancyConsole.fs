namespace AdventOfCode2024.Common

open System.Diagnostics
open Spectre.Console
open Humanizer

module FancyConsole =

    let writeInfo (day: string) (functions: seq<string * (unit -> obj)>) =
        // Display the Figlet text for the day
        AnsiConsole.Write(FigletText(day).Color(Color.Green))

        // Create and configure the table
        let table =
            Table()
                .AddColumn("[bold]Name[/]")
                .AddColumn("[bold]Result[/]")
                .AddColumn("[bold]Time Taken[/]")
                .Border(TableBorder.Rounded)

        // Use Live Rendering with pipelining for clean flow
        AnsiConsole.Live(table).Start(fun ctx ->
            functions
            |> Seq.iter (fun (name, func) ->
                let sw = Stopwatch.StartNew()
                let result = func()
                sw.Stop()
                
                // Add a row with formatted text
                table.AddRow(
                    $"[yellow]{name}[/]",
                    $"[cyan]{result}[/]",
                    $"[green]{sw.Elapsed.Humanize()}[/]"
                ) |> ignore

                ctx.Refresh()
            )
        )


