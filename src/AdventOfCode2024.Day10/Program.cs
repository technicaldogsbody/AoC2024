using AdventOfCode2024.Common.CSharp;

namespace AdventOfCode2024
{
    class Program
    {
        static void Main(string[] args)
        {
            var topographicMap = FileService.GetFileAs2dIntArray("input.txt");

            var tasks = new List<(string, Func<object>)>
            {
                ("Calculate Trailhead Scores", () => CalculateSumOfTrailheadScores(topographicMap)),
                ("Calculate Trailhead Ratings", () => CalculateSumOfTrailheadRatings(topographicMap))
            };

            FancyConsole.WriteInfo("Hoof It", tasks);
        }

        static int CalculateSumOfTrailheadScores(int[,] map)
        {
            int totalScore = 0;

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == 0) // Check if it's a trailhead
                    {
                        totalScore += CalculateTrailheadScore(map, row, col);
                    }
                }
            }

            return totalScore;
        }

        static int CalculateTrailheadScore(int[,] map, int startRow, int startCol)
        {
            var directions = new (int dRow, int dCol)[]
            {
                (-1, 0), // Up
                (1, 0),  // Down
                (0, -1), // Left
                (0, 1)   // Right
            };

            var reachableNines = new HashSet<(int, int)>();
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<(int row, int col, int height)>();

            queue.Enqueue((startRow, startCol, 0));
            visited.Add((startRow, startCol));

            while (queue.Count > 0)
            {
                var (currentRow, currentCol, currentHeight) = queue.Dequeue();

                foreach (var (dRow, dCol) in directions)
                {
                    int newRow = currentRow + dRow;
                    int newCol = currentCol + dCol;

                    if (newRow >= 0 && newRow < map.GetLength(0) &&
                        newCol >= 0 && newCol < map.GetLength(1) &&
                        !visited.Contains((newRow, newCol)) &&
                        map[newRow, newCol] == currentHeight + 1)
                    {
                        if (map[newRow, newCol] == 9)
                        {
                            reachableNines.Add((newRow, newCol));
                        }

                        queue.Enqueue((newRow, newCol, map[newRow, newCol]));
                        visited.Add((newRow, newCol));
                    }
                }
            }

            return reachableNines.Count;
        }

        static int CalculateSumOfTrailheadRatings(int[,] map)
        {
            int totalRating = 0;

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == 0) // Check if it's a trailhead
                    {
                        totalRating += CalculateTrailheadRating(map, row, col);
                    }
                }
            }

            return totalRating;
        }

        static int CalculateTrailheadRating(int[,] map, int startRow, int startCol)
        {
            var directions = new (int dRow, int dCol)[]
            {
                (-1, 0), // Up
                (1, 0),  // Down
                (0, -1), // Left
                (0, 1)   // Right
            };

            var distinctTrails = new HashSet<string>();
            var stack = new Stack<(int row, int col, int height, string trail)>();

            stack.Push((startRow, startCol, 0, ""));

            while (stack.Count > 0)
            {
                var (currentRow, currentCol, currentHeight, currentTrail) = stack.Pop();

                foreach (var (dRow, dCol) in directions)
                {
                    int newRow = currentRow + dRow;
                    int newCol = currentCol + dCol;

                    if (newRow >= 0 && newRow < map.GetLength(0) &&
                        newCol >= 0 && newCol < map.GetLength(1) &&
                        map[newRow, newCol] == currentHeight + 1)
                    {
                        var newTrail = currentTrail + $"({newRow},{newCol})";

                        if (map[newRow, newCol] == 9)
                        {
                            distinctTrails.Add(newTrail);
                        }
                        else
                        {
                            stack.Push((newRow, newCol, map[newRow, newCol], newTrail));
                        }
                    }
                }
            }

            return distinctTrails.Count;
        }
    }
}

