using System.Globalization;
using AdventOfCode2024.Common.CSharp;

namespace AdventOfCode2024
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the disk map from input
            var input = FileService.GetFileAsString("input.txt").Trim();

            // Output the results for both parts
            var tasks = new List<(string, Func<object>)>
            {
                ("Part 1: Disk Fragmenter Checksum (Block Moves)", () => DiskDefragmenter.CompactDiskAndCalculateChecksumPart1(input)),
                ("Part 2: Disk Fragmenter Checksum (File Moves)", () => DiskDefragmenter.CompactDiskAndCalculateChecksumPart2(input))
            };

            FancyConsole.WriteInfo("Disk Fragmenter", tasks);
        }
    }
}
