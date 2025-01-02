namespace AdventOfCode2024;

public record Block
{
    public int FileId { get; }
    public int Length { get; }

    public Block(int fileId, int length)
    {
        FileId = fileId;
        Length = length;
    }
}