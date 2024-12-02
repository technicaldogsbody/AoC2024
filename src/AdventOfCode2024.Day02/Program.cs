using AdventOfCode2024.Common.CSharp;

var reports = FileService.GetFileAsArray("input.txt")
                         .Select(line => line.Split(' ')
                                             .Select(int.Parse)
                                             .ToList())
                         .ToList();

var tasks = new List<(string, Func<object>)>
{
    ("Count Safe Reports", () => reports.Count(IsReportSafe)),
    ("Count Safe Reports with Dampener", () => reports.Count(IsReportSafeWithDampener))
};

FancyConsole.WriteInfo("Red-Nosed Reports", tasks);

static bool IsReportSafe(List<int> levels)
{
    if (levels.Count < 2) return false;

    var isIncreasing = levels[1] > levels[0];
    var isDecreasing = levels[1] < levels[0];

    for (int i = 1; i < levels.Count; i++)
    {
        var diff = levels[i] - levels[i - 1];

        if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
        {
            return false;
        }

        if ((isIncreasing && diff < 0) || (isDecreasing && diff > 0))
        {
            return false;
        }
    }

    return true;
}

static bool IsReportSafeWithDampener(List<int> levels)
{
    if (levels.Count < 2) return false;

    if (IsReportSafe(levels))
    {
        return true;
    }

    for (int i = 0; i < levels.Count; i++)
    {
        var modifiedLevels = levels.Where((_, index) => index != i).ToList();
        if (IsReportSafe(modifiedLevels))
        {
            return true;
        }
    }

    return false;
}