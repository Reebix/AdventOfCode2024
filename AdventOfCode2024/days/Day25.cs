namespace AdventOfCode2024.days;

public class Day25() : Day(25)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        var answer = 0;
        for (var i = 0; i < Input.Length; i += 8)
        {
            var heights = new int[5];
            for (var j = i; j < i + 7; j++)
            for (var x = 0; x < 5; x++)
                if (Input[j][x].Equals('#'))
                    heights[x]++;

            if (!Input[i].Contains('.'))
                locks.Add(heights);
            else
                keys.Add(heights);
        }

        foreach (var l in locks)
        foreach (var key in keys)
        {
            var fits = true;
            for (var x = 0; x < 5; x++)
                if (key[x] + l[x] > 7)
                    fits = false;

            if (fits)
                answer++;
        }

        Console.WriteLine(answer);
    }
}