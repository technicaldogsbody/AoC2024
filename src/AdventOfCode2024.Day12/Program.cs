using AdventOfCode2024.Common.CSharp;

var map = FileService.GetFileAs2dCharArray("input.txt");

var tasks = new List<(string, Func<object>)>
{
    ("Total Price (Perimeter Method)", () => CalculateTotalPrice(map, useSidesInsteadOfPerimeter: false)),
    ("Total Price (Sides Method)", () => CalculateTotalPrice(map, useSidesInsteadOfPerimeter: true))
};

FancyConsole.WriteInfo("Garden Groups", tasks);

static int CalculateTotalPrice(char[,] map, bool useSidesInsteadOfPerimeter)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);
    var visited = new bool[rows, cols];
    int totalPrice = 0;

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (!visited[r, c] && map[r, c] != ' ')
            {
                var regionInfo = GetRegionInfo(map, visited, r, c, useSidesInsteadOfPerimeter);
                totalPrice += regionInfo.Area * regionInfo.Measure;
            }
        }
    }

    return totalPrice;
}

static RegionInfo GetRegionInfo(char[,] map, bool[,] visited, int startRow, int startCol, bool useSidesInsteadOfPerimeter)
{
    int rows = map.GetLength(0);
    int cols = map.GetLength(1);
    char plantType = map[startRow, startCol];

    int area = 0;
    var sidesCount = new Dictionary<(int dr, int dc), HashSet<(int r, int c)>>();

    var directions = new (int dr, int dc)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var queue = new Queue<(int r, int c)>();
    queue.Enqueue((startRow, startCol));
    visited[startRow, startCol] = true;

    while (queue.Count > 0)
    {
        var (r, c) = queue.Dequeue();
        area++;

        foreach (var (dr, dc) in directions)
        {
            int nr = r + dr;
            int nc = c + dc;

            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols)
            {
                if (map[nr, nc] == plantType)
                {
                    if (!visited[nr, nc])
                    {
                        visited[nr, nc] = true;
                        queue.Enqueue((nr, nc));
                    }
                }
                else
                {
                    if (!sidesCount.ContainsKey((dr, dc)))
                        sidesCount[(dr, dc)] = new HashSet<(int r, int c)>();

                    sidesCount[(dr, dc)].Add((r, c));
                }
            }
            else
            {
                if (!sidesCount.ContainsKey((dr, dc)))
                    sidesCount[(dr, dc)] = new HashSet<(int r, int c)>();

                sidesCount[(dr, dc)].Add((r, c));
            }
        }
    }

    int measure = useSidesInsteadOfPerimeter
        ? sidesCount.Values.Sum(set => CountDistinctRegions(set, directions))
        : sidesCount.Values.Sum(set => set.Count);

    return new RegionInfo(area, measure);
}

static int CountDistinctRegions(HashSet<(int r, int c)> regionCells, (int dr, int dc)[] directions)
{
    var visited = new HashSet<(int r, int c)>();
    int regions = 0;

    foreach (var cell in regionCells)
    {
        if (visited.Contains(cell))
            continue;

        regions++;
        var queue = new Queue<(int r, int c)>();
        queue.Enqueue(cell);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current)) continue;

            visited.Add(current);

            foreach (var (dr, dc) in directions)
            {
                var neighbor = (current.r + dr, current.c + dc);
                if (regionCells.Contains(neighbor) && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
    }

    return regions;
}