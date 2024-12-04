using System.Text.RegularExpressions;

namespace AdventOfCode2024.Common.CSharp;

public static class FileService
{
    public static string GetFileAsString(string fileName)
    {
        var contents = File.ReadAllText($"Data/{fileName}");

        return contents;
    }

    public static IEnumerable<string> GetFileAsArray(string fileName, string delimiterPattern = @"\r\n|\n")
    {
        var contents = File.ReadAllText($"Data/{fileName}");
        return Regex.Split(contents, delimiterPattern).Where(line => !string.IsNullOrEmpty(line));
    }

    public static int[,] GetFileAs2dIntArray(string fileName, string rowDelimiterPattern = @"\r\n|\n")
    {
        var rows = Regex.Split(File.ReadAllText($"Data/{fileName}"), rowDelimiterPattern).Where(row => !string.IsNullOrEmpty(row)).ToArray();

        if (rows.Length == 0)
        {
            return new int[0, 0];
        }

        var response = new int[rows.Length, rows[0].Length];

        for (var i = 0; i < rows.Length; i++)
        {
            var row = rows[i].ToCharArray();

            for (var j = 0; j < row.Length; j++)
            {
                response[i, j] = int.Parse(row[j].ToString());
            }
        }

        return response;
    }

    public static List<List<int>> GetFileAsListOfListOfInt(string fileName, string rowDelimiterPattern = @"\r\n|\n")
    {
        var rows = Regex.Split(File.ReadAllText($"Data/{fileName}"), rowDelimiterPattern).Where(row => !string.IsNullOrEmpty(row)).ToArray();

        return rows.Length == 0 ?
            new() : rows.Select(t => t.Split(" ")).Select(row => row.Select(int.Parse).ToList()).ToList();
    }

    public static char[,] GetFileAs2dCharArray(string fileName)
    {
        var lines = GetFileAsArray(fileName).ToArray();
        int rows = lines.Length;
        int cols = lines[0].Length;

        var grid = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }
        return grid;
    }
}
