using AdventOfCode2024.Common.CSharp;

var inputMap = FileService.GetFileAsArray("input.txt");
var antennas = ParseMap(inputMap);

var tasks = new List<(string, Func<object>)>
{
    ("Part 1: Total Unique Antinodes",
        () => CalculateUniqueAntinodes(antennas, inputMap.Length, inputMap[0].Length).Count),
    ("Part 2: Total Unique Antinodes",
        () => CalculateUniqueAntinodesPart2(antennas, inputMap.Length, inputMap[0].Length).Count)
};

FancyConsole.WriteInfo("Resonant Collinearity", tasks);

static List<(char frequency, (int row, int col) position)> ParseMap(string[] map)
{
    var antennas = new List<(char frequency, (int row, int col) position)>();

    for (int row = 0; row < map.Length; row++)
    {
        for (int col = 0; col < map[row].Length; col++)
        {
            char cell = map[row][col];
            if (char.IsLetterOrDigit(cell))
            {
                antennas.Add((cell, (row, col)));
            }
        }
    }

    return antennas;
}

static HashSet<(int row, int col)> CalculateUniqueAntinodes(
    List<(char frequency, (int row, int col) position)> antennas,
    int numRows,
    int numCols)
{
    var uniqueAntinodes = new HashSet<(int row, int col)>();
    var groupedAntennas = antennas
        .GroupBy(a => a.frequency)
        .ToDictionary(g => g.Key, g => g.Select(a => a.position).ToList());

    foreach (var frequencyGroup in groupedAntennas)
    {
        var positions = frequencyGroup.Value;

        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                var (r1, c1) = positions[i];
                var (r2, c2) = positions[j];

                int midRow = (r1 + r2) / 2;
                int midCol = (c1 + c2) / 2;

                if ((r1 + r2) % 2 == 0 && (c1 + c2) % 2 == 0)
                {
                    uniqueAntinodes.Add((midRow, midCol));
                }

                int deltaRow = r2 - r1;
                int deltaCol = c2 - c1;

                int extrapolated1Row = r1 - deltaRow;
                int extrapolated1Col = c1 - deltaCol;

                if (IsInBounds(extrapolated1Row, extrapolated1Col, numRows, numCols))
                {
                    uniqueAntinodes.Add((extrapolated1Row, extrapolated1Col));
                }

                int extrapolated2Row = r2 + deltaRow;
                int extrapolated2Col = c2 + deltaCol;

                if (IsInBounds(extrapolated2Row, extrapolated2Col, numRows, numCols))
                {
                    uniqueAntinodes.Add((extrapolated2Row, extrapolated2Col));
                }
            }
        }
    }

    return uniqueAntinodes;
}

static HashSet<(int row, int col)> CalculateUniqueAntinodesPart2(
    List<(char frequency, (int row, int col) position)> antennas,
    int numRows,
    int numCols)
{
    var uniqueAntinodes = new HashSet<(int row, int col)>();

    // Group antennas by frequency
    var groupedAntennas = antennas
        .GroupBy(a => a.frequency)
        .ToDictionary(g => g.Key, g => g.Select(a => a.position).ToList());

    foreach (var frequencyGroup in groupedAntennas)
    {
        var positions = frequencyGroup.Value;

        // Add all antenna positions as antinodes
        foreach (var pos in positions)
        {
            uniqueAntinodes.Add(pos);
        }

        // Check all pairs of antennas with the same frequency
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                var (r1, c1) = positions[i];
                var (r2, c2) = positions[j];

                // Calculate the line between the two antennas
                int deltaRow = r2 - r1;
                int deltaCol = c2 - c1;
                int gcd = GCD(Math.Abs(deltaRow), Math.Abs(deltaCol));

                deltaRow /= gcd;
                deltaCol /= gcd;

                int currentRow = r1;
                int currentCol = c1;

                // Add all points along the line
                while (true)
                {
                    uniqueAntinodes.Add((currentRow, currentCol));
                    if ((currentRow, currentCol) == (r2, c2)) break;
                    currentRow += deltaRow;
                    currentCol += deltaCol;
                }
            }
        }

        // Check for all other antenna combinations
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = 0; j < positions.Count; j++)
            {
                if (i == j) continue;
                var (r1, c1) = positions[i];
                var (r2, c2) = positions[j];

                int deltaRow = r2 - r1;
                int deltaCol = c2 - c1;

                int currentRow = r2 + deltaRow;
                int currentCol = c2 + deltaCol;

                while (IsInBounds(currentRow, currentCol, numRows, numCols))
                {
                    uniqueAntinodes.Add((currentRow, currentCol));
                    currentRow += deltaRow;
                    currentCol += deltaCol;
                }
            }
        }
    }

    return uniqueAntinodes;
}

static bool IsInBounds(int row, int col, int numRows, int numCols)
{
    return row >= 0 && row < numRows && col >= 0 && col < numCols;
}

static int GCD(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }

    return a;
}