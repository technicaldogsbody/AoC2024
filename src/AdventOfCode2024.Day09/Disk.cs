namespace AdventOfCode2024;

public class Disk
{
    private readonly List<Block> _blocks;

    private Disk(List<Block> blocks)
    {
        _blocks = blocks;
    }

    public static Disk Parse(string diskMap)
    {
        var blocks = new List<Block>();
        int fileId = 0;

        for (int i = 0; i < diskMap.Length; i++)
        {
            int length = diskMap[i] - '0';
            if (i % 2 == 0) // File blocks
            {
                blocks.Add(new Block(fileId++, length));
            }
            else // Free space
            {
                blocks.Add(new Block(-1, length));
            }
        }

        return new Disk(blocks);
    }

    public void CompactFiles()
    {
        for (int i = _blocks.Count - 1; i >= 0; i--)
        {
            var block = _blocks[i];
            if (block.FileId == -1) continue;

            for (int j = 0; j < i; j++)
            {
                var freeBlock = _blocks[j];
                if (freeBlock.FileId == -1 && freeBlock.Length >= block.Length)
                {
                    _blocks[j] = new Block(block.FileId, block.Length);
                    _blocks[i] = new Block(-1, block.Length);

                    int remainingFree = freeBlock.Length - block.Length;
                    if (remainingFree > 0)
                    {
                        _blocks.Insert(j + 1, new Block(-1, remainingFree));
                    }

                    break;
                }
            }
        }
    }

    public long CalculateChecksum()
    {
        long checksum = 0;
        int position = 0;

        foreach (var block in _blocks)
        {
            if (block.FileId != -1)
            {
                for (int i = 0; i < block.Length; i++)
                {
                    checksum += (long)position * block.FileId;
                    position++;
                }
            }
            else
            {
                position += block.Length;
            }
        }

        return checksum;
    }
}