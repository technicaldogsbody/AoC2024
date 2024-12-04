
using AdventOfCode2024.Common.CSharp;

var wordSearch = FileService.GetFileAs2dCharArray("input.txt");

string targetWord = "XMAS";

FancyConsole.WriteInfo("Ceres Search", new List<(string, Func<object>)>
{
    ("Total Occurrences of XMAS", () => CountWordOccurrences(wordSearch, targetWord)),
    ("Total Occurrences of X-MAS", () => CountXmasPatterns(wordSearch))
});

static int CountWordOccurrences(char[,] grid, string word)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int wordLength = word.Length;
    int count = 0;


    var directions = new int[,]
    {
        { 0, 1 },  // Right
        { 1, 0 },  // Down
        { 0, -1 }, // Left
        { -1, 0 }, // Up
        { 1, 1 },  // Diagonal Down-Right
        { 1, -1 }, // Diagonal Down-Left
        { -1, 1 }, // Diagonal Up-Right
        { -1, -1 } // Diagonal Up-Left
    };

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int rowDelta = directions[d, 0];
                int colDelta = directions[d, 1];

                if (CanMatchWord(grid, word, row, col, rowDelta, colDelta))
                {
                    count++;
                }
            }
        }
    }

    return count;
}

static bool CanMatchWord(char[,] grid, string word, int startRow, int startCol, int rowDelta, int colDelta)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int wordLength = word.Length;

    for (int i = 0; i < wordLength; i++)
    {
        int newRow = startRow + i * rowDelta;
        int newCol = startCol + i * colDelta;

        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
        {
            return false;
        }

        if (grid[newRow, newCol] != word[i])
        {
            return false;
        }
    }

    return true;
}

static int CountXmasPatterns(char[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    int count = 0;

    // Check every possible center of the X pattern
    for (int row = 1; row < rows - 1; row++)
    {
        for (int col = 1; col < cols - 1; col++)
        {
            if (IsXmasPattern(grid, row, col))
            {
                count++;
            }
        }
    }

    return count;
}

static bool IsXmasPattern(char[,] grid, int centerRow, int centerCol)
{
    if (!IsMas(grid[centerRow - 1, centerCol - 1], grid[centerRow, centerCol], grid[centerRow + 1, centerCol + 1]))
        return false;

    if (!IsMas(grid[centerRow + 1, centerCol - 1], grid[centerRow, centerCol], grid[centerRow - 1, centerCol + 1]))
        return false;

    return true;
}

static bool IsMas(char first, char middle, char last)
{
    return (first == 'M' && middle == 'A' && last == 'S') ||
           (first == 'S' && middle == 'A' && last == 'M');
}
