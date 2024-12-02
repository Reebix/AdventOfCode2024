namespace AdventOfCode2024.days;

public class Day2() : Day(2)
{
    protected override void Run(bool isPart2 = false)
    {
        var lines = new List<List<int>>();
        foreach (var line in Input)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToList();
            lines.Add(numbers);
        }

        var bad = new List<int>();
        var total = 0;
        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            if (IsBad(line))
            {
                bad.Add(index);
                continue;
            }

            total++;
        }

        if (!isPart2)
        {
            Console.WriteLine(total);
            return;
        }

        bad.ForEach(index =>
        {
            var line = lines[index];
            if (SafeAfterDampen(line))
            {
                Interlocked.Increment(ref total);
            }
        });
        Console.WriteLine(total);
        
    }

    private bool SafeAfterDampen(List<int> line)
    {
        for (var i = 0; i < line.Count; i++)
        {
            var copy = new List<int>(line);
            copy.RemoveAt(i);
            if (!IsBad(copy))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsBad(List<int> line)
    {
        var unsafeCount = 0;
        var last = -1;
        var set = false;
        var ascending = false;
        for (var index = 0; index < line.Count; index++)
        {
            var number = line[index];
            if (last == -1)
            {
                last = number;
                continue;
            }

            if (!set)
            {
                set = true;
                ascending = last < number;
            }

            if (Math.Abs(last - number) > 3 || (ascending && last >= number) || (!ascending && last <= number))
            {
                unsafeCount++;
            }

            last = number;
        }

        return unsafeCount != 0;
    }
}