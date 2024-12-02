using Humanizer;
using Spectre.Console;
using System.Diagnostics;

namespace AdventOfCode2024.Common.CSharp
{
    public static class FancyConsole
    {
        public static void WriteInfo(string title, IEnumerable<(string Name, Func<object> Function)> functions)
        {
            // Display the Figlet text for the title
            AnsiConsole.Write(new FigletText(title).Color(Color.Green));

            // Create and configure the table
            var table = new Table()
                .AddColumn("[bold]Name[/]")
                .AddColumn("[bold]Result[/]")
                .AddColumn("[bold]Time Taken[/]")
                .Border(TableBorder.Rounded);

            // Use Live Rendering for dynamic updates
            AnsiConsole.Live(table).Start(ctx =>
            {
                foreach (var (name, func) in functions)
                {
                    try
                    {
                        // Measure function execution time
                        var stopwatch = Stopwatch.StartNew();
                        var result = func();
                        stopwatch.Stop();

                        // Add a row with formatted text
                        table.AddRow(
                            $"[yellow]{name}[/]",
                            $"[cyan]{result}[/]",
                            $"[green]{stopwatch.Elapsed.Humanize()}[/]"
                        );

                        // Refresh the live context
                        ctx.Refresh();
                    }
                    catch (Exception ex)
                    {
                        // Add an error row if the function throws an exception
                        table.AddRow(
                            $"[red]{name}[/]",
                            $"[red]{ex.Message}[/]",
                            "[red]Error[/]"                    
                        );

                        ctx.Refresh();
                    }
                }
            });
        }
    }
}
