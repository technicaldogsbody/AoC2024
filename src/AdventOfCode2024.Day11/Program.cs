using AdventOfCode2024.Common.CSharp;

namespace AdventOfCode2024.Day11;

class Program
{
    static void Main(string[] args)
    {
        var stones = FileService.GetFileAsLongArray("input.txt", " ");

        var tasks = new List<(string, Func<object>)>
        {
            ("Number of Stones After 25 Blinks", () => PlutonianPebbles.CountStonesAfterBlinks(stones.ToList(), 25)),
            ("Number of Stones After 75 Blinks", () => PlutonianPebbles.CountStonesAfterBlinks(stones.ToList(), 75))
        };

        FancyConsole.WriteInfo("Plutonian Pebbles", tasks);
    }
}