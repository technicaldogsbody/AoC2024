namespace AdventOfCode2024.Day11;

public class PlutonianPebbles
{
    public static long CountStonesAfterBlinks(List<long> initialStones, int blinks)
    {
        // Frequency map to track the number of stones with each value
        var frequencies = new Dictionary<long, long>();

        // Initialise frequencies from the input
        foreach (var stone in initialStones)
        {
            if (!frequencies.ContainsKey(stone))
                frequencies[stone] = 0;
            frequencies[stone]++;
        }

        // Process blinks iteratively
        for (int i = 0; i < blinks; i++)
        {
            frequencies = BlinkFrequencies(frequencies);
        }

        // Sum up all stone counts
        return frequencies.Values.Sum();
    }

    private static Dictionary<long, long> BlinkFrequencies(Dictionary<long, long> frequencies)
    {
        var newFrequencies = new Dictionary<long, long>();

        foreach (var (stone, count) in frequencies)
        {
            if (stone == 0)
            {
                // Rule 1: Stone becomes 1
                AddToDictionary(newFrequencies, 1, count);
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                // Rule 2: Split into two stones
                var digits = stone.ToString();
                int mid = digits.Length / 2;
                long left = long.Parse(digits.Substring(0, mid));
                long right = long.Parse(digits.Substring(mid));
                AddToDictionary(newFrequencies, left, count);
                AddToDictionary(newFrequencies, right, count);
            }
            else
            {
                // Rule 3: Multiply by 2024
                AddToDictionary(newFrequencies, stone * 2024, count);
            }
        }

        return newFrequencies;
    }

    private static void AddToDictionary(Dictionary<long, long> dictionary, long key, long count)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary[key] = 0;
        }
        dictionary[key] += count;
    }
}
