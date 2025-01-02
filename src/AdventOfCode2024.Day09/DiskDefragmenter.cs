namespace AdventOfCode2024;

public class DiskDefragmenter
{
    public static long CompactDiskAndCalculateChecksumPart1(string diskMap)
    {
        // Parse the disk map into blocks
        var blocks = ParseDiskMap(diskMap);

        // Compact the blocks using block moves
        CompactBlocks(blocks);

        // Calculate the checksum
        return CalculateChecksum(blocks);
    }

    public static long CompactDiskAndCalculateChecksumPart2(string diskMap)
    {
        // Parse the disk map into a Disk instance
        var disk = Disk.Parse(diskMap);

        // Compact the files using file moves
        disk.CompactFiles();

        // Calculate the checksum
        return disk.CalculateChecksum();
    }

    private static char[] ParseDiskMap(string diskMap)
    {
        var blocks = new List<char>();
        int fileId = 0;

        for (int i = 0; i < diskMap.Length; i++)
        {
            int length = diskMap[i] - '0';
            if (i % 2 == 0) // File blocks
            {
                blocks.AddRange(Enumerable.Repeat((char)('0' + fileId), length));
                fileId++;
            }
            else // Free space
            {
                blocks.AddRange(Enumerable.Repeat('.', length));
            }
        }
        return blocks.ToArray();
    }

    private static void CompactBlocks(char[] blocks)
    {
        for (int readIndex = blocks.Length - 1; readIndex >= 0; readIndex--)
        {
            if (blocks[readIndex] != '.')
            {
                // Move the file block to the leftmost free space
                for (int writeIndex = 0; writeIndex < blocks.Length; writeIndex++)
                {
                    if (blocks[writeIndex] == '.' && readIndex > writeIndex)
                    {
                        blocks[writeIndex] = blocks[readIndex];
                        blocks[readIndex] = '.';
                        break;
                    }
                }
            }
        }
    }

    private static long CalculateChecksum(char[] blocks)
    {
        long checksum = 0;
        for (int position = 0; position < blocks.Length; position++)
        {
            if (blocks[position] != '.')
            {
                checksum += (long)position * (blocks[position] - '0');
            }
        }
        return checksum;
    }
}