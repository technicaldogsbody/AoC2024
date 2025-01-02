using AdventOfCode2024.Common.CSharp;

var map = FileService.GetFileAsArray("input.txt");
var (grid, guardPosition, guardDirection) = ParseMap(map);

var tasks = new List<(string, Func<object>)>
{
    ("Distinct Positions Visited", () => SimulatePatrol(grid, guardPosition, guardDirection).Count),
    ("Valid Obstruction Positions", () => FindObstructionPositions(grid, guardPosition, guardDirection).Count)
};

FancyConsole.WriteInfo("Guard Gallivant", tasks);

static (char[,], (int row, int col), (int rowDelta, int colDelta)) ParseMap(string[] map)
{
    int rows = map.Length;
    int cols = map[0].Length;
    char[,] grid = new char[rows, cols];
    (int row, int col) guardPosition = (0, 0);
    (int rowDelta, int colDelta) guardDirection = (-1, 0); // Default facing up

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            grid[row, col] = map[row][col];
            if ("^>v<".Contains(grid[row, col]))
            {
                guardPosition = (row, col);
                guardDirection = grid[row, col] switch
                {
                    '^' => (-1, 0), // Up
                    '>' => (0, 1), // Right
                    'v' => (1, 0), // Down
                    '<' => (0, -1), // Left
                    _ => throw new InvalidOperationException("Invalid direction")
                };
                grid[row, col] = '.';
            }
        }
    }

    return (grid, guardPosition, guardDirection);
}

static HashSet<(int row, int col)> SimulatePatrol(
    char[,] grid,
    (int row, int col) guardPosition,
    (int rowDelta, int colDelta) guardDirection)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var visited = new HashSet<(int row, int col)> { guardPosition };

    while (true)
    {
        var (nextRow, nextCol) = (guardPosition.row + guardDirection.rowDelta,
            guardPosition.col + guardDirection.colDelta);

        if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols)
        {
            break;
        }

        if (grid[nextRow, nextCol] == '#')
        {
            guardDirection = TurnRight(guardDirection);
        }
        else
        {
            guardPosition = (nextRow, nextCol);
            visited.Add(guardPosition);
        }
    }

    return visited;
}

static (int rowDelta, int colDelta) TurnRight((int rowDelta, int colDelta) direction)
{
    return direction switch
    {
        (-1, 0) => (0, 1), // Up -> Right
        (0, 1) => (1, 0), // Right -> Down
        (1, 0) => (0, -1), // Down -> Left
        (0, -1) => (-1, 0), // Left -> Up
        _ => throw new InvalidOperationException("Invalid direction")
    };
}

static HashSet<(int row, int col)> FindObstructionPositions(
    char[,] grid,
    (int row, int col) guardPosition,
    (int rowDelta, int colDelta) guardDirection)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var validObstructions = new HashSet<(int row, int col)>();

    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            // Skip non-empty cells and the starting position
            if (grid[row, col] != '.' || (row, col) == guardPosition)
                continue;

            // Simulate patrol with obstruction at (row, col)
            if (SimulateWithObstruction(grid, guardPosition, guardDirection, (row, col)))
            {
                validObstructions.Add((row, col));
            }
        }
    }

    return validObstructions;
}

static bool SimulateWithObstruction(
    char[,] grid,
    (int row, int col) guardPosition,
    (int rowDelta, int colDelta) guardDirection,
    (int row, int col) obstructionPosition)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);
    var visitedStates = new HashSet<((int row, int col) position, (int rowDelta, int colDelta) direction)>();

    // Clone the grid and add the obstruction
    var gridCopy = (char[,])grid.Clone();
    gridCopy[obstructionPosition.row, obstructionPosition.col] = '#';

    var currentPosition = guardPosition;
    var currentDirection = guardDirection;

    while (true)
    {
        // Record the current state
        var state = (currentPosition, currentDirection);
        if (visitedStates.Contains(state))
        {
            // Loop detected
            return true;
        }

        visitedStates.Add(state);

        // Calculate the next position
        var (nextRow, nextCol) = (currentPosition.row + currentDirection.rowDelta,
            currentPosition.col + currentDirection.colDelta);

        // Check if the guard is about to leave the grid
        if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols)
        {
            break;
        }

        // If the next cell is an obstacle, turn right
        if (gridCopy[nextRow, nextCol] == '#')
        {
            currentDirection = TurnRight(currentDirection);
        }
        else
        {
            // Move forward
            currentPosition = (nextRow, nextCol);
        }
    }

    // No loop detected
    return false;
}