using AdventOfCode2024.Common.CSharp;

var input = FileService.GetFileAsArray("input.txt");

var tasks = new List<(string, Func<object>)>
{
    ("Part 1: Total Calibration Result (+, *)", () => input
        .Select(ParseLine)
        .Where(line => IsSolvable(line.testValue, line.numbers, includeConcatenation: false))
        .Sum(line => line.testValue)),
    ("Part 2: Total Calibration Result (+, *, ||)", () => input
        .Select(ParseLine)
        .Where(line => IsSolvable(line.testValue, line.numbers, includeConcatenation: true))
        .Sum(line => line.testValue))
};

FancyConsole.WriteInfo("Bridge Repair", tasks);

static (long testValue, List<long> numbers) ParseLine(string line)
{
    var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
    var testValue = long.Parse(parts[0].Trim());
    var numbers = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToList();

    return (testValue, numbers);
}

static bool IsSolvable(long testValue, List<long> numbers, bool includeConcatenation)
{
    if (numbers.Count == 1)
    {
        return false;
    }

    var operatorCombinations = GenerateOperatorCombinations(numbers.Count - 1, includeConcatenation);

    foreach (var ops in operatorCombinations)
    {
        if (EvaluateExpression(numbers, ops) == testValue)
        {
            return true;
        }
    }

    return false;
}

static IEnumerable<List<string>> GenerateOperatorCombinations(int count, bool includeConcatenation)
{
    var operators = includeConcatenation ? new[] { "+", "*", "||" } : new[] { "+", "*" };
    var combinations = new List<List<string>>();

    void Backtrack(List<string> current)
    {
        if (current.Count == count)
        {
            combinations.Add(new List<string>(current));
            return;
        }

        foreach (var op in operators)
        {
            current.Add(op);
            Backtrack(current);
            current.RemoveAt(current.Count - 1);
        }
    }

    Backtrack(new List<string>());
    return combinations;
}

static long EvaluateExpression(List<long> numbers, List<string> operators)
{
    long result = numbers[0];

    for (int i = 0; i < operators.Count; i++)
    {
        result = operators[i] switch
        {
            "+" => result + numbers[i + 1],
            "*" => result * numbers[i + 1],
            "||" => Concatenate(result, numbers[i + 1]),
            _ => throw new InvalidOperationException("Unknown operator")
        };
    }

    return result;
}

static long Concatenate(long left, long right)
{
    return long.Parse($"{left}{right}");
}
