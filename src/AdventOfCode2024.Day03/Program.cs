using System.Text.RegularExpressions;
using AdventOfCode2024.Common.CSharp;

string corruptedMemory = FileService.GetFileAsString("input.txt");

FancyConsole.WriteInfo("Mull It Over", new List<(string, Func<object>)>
{
    ("Total Sum of Valid Multiplications", () => GetTotalSum(corruptedMemory)),
    ("Total Sum of Conditional Multiplications", () => GetConditionalSum(corruptedMemory))
});
static int GetTotalSum(string corruptedMemory)
{
    var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)"); 
    var matches = regex.Matches(corruptedMemory);
    int i = 0;

    foreach (Match match in matches)
    {
        i += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
    }
    
    return i;
}

static int GetConditionalSum(string corruptedMemory)
{
    var mulRegex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
    var doRegex = new Regex(@"do\(\)");
    var dontRegex = new Regex(@"don't\(\)");

    bool isEnabled = true;
    int totalSum = 0;

    int position = 0;
    while (position < corruptedMemory.Length)
    {
        var doMatch = doRegex.Match(corruptedMemory, position);
        if (doMatch.Success && doMatch.Index == position)
        {
            isEnabled = true;
            position += doMatch.Length;
            continue;
        }

        var dontMatch = dontRegex.Match(corruptedMemory, position);
        if (dontMatch.Success && dontMatch.Index == position)
        {
            isEnabled = false;
            position += dontMatch.Length;
            continue;
        }

        var mulMatch = mulRegex.Match(corruptedMemory, position);
        if (mulMatch.Success && mulMatch.Index == position)
        {
            if (isEnabled)
            {
                int x = int.Parse(mulMatch.Groups[1].Value);
                int y = int.Parse(mulMatch.Groups[2].Value);
                totalSum += x * y;
            }

            position += mulMatch.Length;
            continue;
        }

        position++;
    }

    return totalSum;
}