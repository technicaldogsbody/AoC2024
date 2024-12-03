using AdventOfCode2024.Common.CSharp;
using System.Text.RegularExpressions;

var (leftList, rightList) = ParseInputFile("input.txt");

var tasks = new List<(string, Func<object>)>
{
    ("Calculate Total Distance", () => CalculateTotalDistance(leftList, rightList)),
    ("Calculate Similarity Score", () => CalculateSimilarityScore(leftList, rightList))
};

FancyConsole.WriteInfo("Historian Hysteria", tasks);

static (List<int> LeftList, List<int> RightList) ParseInputFile(string filename)
{
    var leftList = new List<int>();
    var rightList = new List<int>();

    foreach (var line in File.ReadAllLines(Path.Combine("Data", filename)))
    {
        var parts = Regex.Split(line.Trim(), @"\s+")
                         .Select(part =>
                         {
                             if (int.TryParse(part, out var value)) return (int?)value;
                             return null;
                         })
                         .ToArray();

        if (parts.Length == 2 && parts[0].HasValue && parts[1].HasValue)
        {
            leftList.Add(parts[0].Value);
            rightList.Add(parts[1].Value);
        }
    }

    return (leftList, rightList);
}

static int CalculateTotalDistance(List<int> leftList, List<int> rightList)
{
    var leftSorted = leftList.OrderBy(x => x).ToList();
    var rightSorted = rightList.OrderBy(x => x).ToList();
    
    return leftSorted.Zip(rightSorted, (left, right) => Math.Abs(left - right)).Sum();
}

static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
{
    var rightFrequencyMap = rightList.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

    return leftList.Sum(left =>
    {
        rightFrequencyMap.TryGetValue(left, out var count);
        return left * count;
    });
}