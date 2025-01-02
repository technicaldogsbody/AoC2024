using AdventOfCode2024.Common.CSharp;
using System.Numerics;
using System.Text.RegularExpressions;

var machines = ParseInputFile("input.txt");

var tasks = new List<(string, Func<object>)>
{
    ("Calculate Minimum Total Cost", () => SolveMachines(machines))
};

FancyConsole.WriteInfo("Claw Contraption", tasks);

static List<Machine> ParseInputFile(string filename)
{
    var lines = FileService.GetFileAsArray(filename);
    var machines = new List<Machine>();

    for (int i = 0; i < lines.Length; i += 3)
    {
        var buttonA = ParseMovement(lines[i]);
        var buttonB = ParseMovement(lines[i + 1]);
        var prize = ParsePrize(lines[i + 2]);

        machines.Add(new Machine
        {
            ButtonA = buttonA,
            ButtonB = buttonB,
            Prize = prize
        });
    }

    return machines;
}

static (int X, int Y) ParseMovement(string line)
{
    var match = Regex.Match(line, @"X\+(\d+), Y\+(\d+)");
    return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
}

static (int X, int Y) ParsePrize(string line)
{
    var match = Regex.Match(line, @"X=(\d+), Y=(\d+)");
    return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
}

static int SolveMachines(List<Machine> machines)
{
    int totalCost = 0;

    foreach (var machine in machines)
    {
        var result = SolveMachine(machine);
        if (result.HasValue)
        {
            totalCost += result.Value;
        }
    }

    return totalCost;
}

static int? SolveMachine(Machine machine)
{
    var (aX, aY) = machine.ButtonA;
    var (bX, bY) = machine.ButtonB;
    var (pX, pY) = machine.Prize;

    // Iterate over possible A and B counts
    int minCost = int.MaxValue;
    for (int a = 0; a <= 100; a++)
    {
        for (int b = 0; b <= 100; b++)
        {
            if (aX * a + bX * b == pX && aY * a + bY * b == pY)
            {
                int cost = 3 * a + b;
                if (cost < minCost)
                {
                    minCost = cost;
                }
            }
        }
    }

    return minCost == int.MaxValue ? null : minCost;
}

