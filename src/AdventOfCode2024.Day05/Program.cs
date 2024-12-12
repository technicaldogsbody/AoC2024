using AdventOfCode2024.Common.CSharp;

var input = FileService.GetFileAsString("input.txt");

var (rules, updates) = ParseInput(input);

var orderingGraph = BuildOrderingGraph(rules);

var validMiddlePagesSum = updates
    .Where(update => IsValidUpdate(update, orderingGraph))
    .Select(GetMiddlePage);

var fixedMiddlePagesSum = updates
    .Where(update => !IsValidUpdate(update, orderingGraph))
    .Select(update => FixOrder(update, orderingGraph))
    .Select(GetMiddlePage);

var tasks = new List<(string, Func<object>)>
{
    ("Sum of Middle Pages (Valid Updates)", () => validMiddlePagesSum.Sum()),
    ("Sum of Middle Pages After Fixing Updates", () => fixedMiddlePagesSum.Sum())
};

FancyConsole.WriteInfo("Print Queue", tasks);

static (List<(int, int)>, List<List<int>>) ParseInput(string input)
{
    var sections = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

    var rules = sections[0]
        .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(line =>
        {
            var parts = line.Split('|').Select(int.Parse).ToArray();
            return (parts[0], parts[1]);
        })
        .ToList();

    var updates = sections[1]
        .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(line =>
            line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList())
        .ToList();

    return (rules, updates);
}

static Dictionary<int, HashSet<int>> BuildOrderingGraph(List<(int, int)> rules)
{
    var graph = new Dictionary<int, HashSet<int>>();

    foreach (var (x, y) in rules)
    {
        if (!graph.ContainsKey(x)) graph[x] = new HashSet<int>();
        graph[x].Add(y);
    }

    return graph;
}

static bool IsValidUpdate(List<int> update, Dictionary<int, HashSet<int>> graph)
{
    var indexMap = update
        .Select((page, index) => (page, index))
        .ToDictionary(pair => pair.page, pair => pair.index);

    foreach (var x in graph.Keys)
    {
        foreach (var y in graph[x])
        {
            if (!indexMap.ContainsKey(x) || !indexMap.TryGetValue(y, out var value)) continue;
            if (indexMap[x] >= value) return false;
        }
    }

    return true;
}

static List<int> FixOrder(List<int> update, Dictionary<int, HashSet<int>> graph)
{
    var inDegree = new Dictionary<int, int>();
    var adjList = new Dictionary<int, List<int>>();

    foreach (var page in update)
    {
        inDegree[page] = 0;
        adjList[page] = new List<int>();
    }

    foreach (var x in graph.Keys)
    {
        foreach (var y in graph[x])
        {
            if (update.Contains(x) && update.Contains(y))
            {
                adjList[x].Add(y);
                inDegree[y]++;
            }
        }
    }

    var sortedOrder = new List<int>();
    var queue = new Queue<int>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        sortedOrder.Add(current);

        foreach (var neighbor in adjList[current])
        {
            inDegree[neighbor]--;
            if (inDegree[neighbor] == 0) queue.Enqueue(neighbor);
        }
    }

    return sortedOrder;
}

static int GetMiddlePage(List<int> update)
{
    return update[update.Count / 2];
}
