using Microsoft.VisualBasic;

namespace AdventOfCode2024.days;

public class Day11() : Day(11)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        var stones = Input[0].Split(" ").Select(long.Parse).ToList();
        var allStones = new List<long>();
        foreach (var stone in stones)
        {
            var newStones = new List<long>() { stone };
            for (int i = 0; i < 25; i++)
            {
                Blink(newStones);
            }
            
            allStones.AddRange(newStones);
        }
        
        // Console.WriteLine(String.Join(" ", _stones));
        Console.WriteLine(allStones.Count);

        var allStonesPt2 = new List<long>();
        allStones.AsParallel().ForAll(
            stone =>
            {
                var newStones = new List<long>() { stone };
                for (int i = 0; i < 50; i++)
                {
                    Blink(newStones);
                }
                
                allStonesPt2.AddRange(newStones);
            });
       
     
        // Console.WriteLine(String.Join(" ", _stones));
        Console.WriteLine(allStones.Count);
        Console.WriteLine();
        Console.WriteLine(allStonesPt2.Count);
    }

    //TODO: move to two arrays and parallelize
    private void Blink(List<long> stones)
    {
        for (var i = stones.Count - 1; i >= 0; i--)
        {
            long stone = stones[i];
            // 0 replace
            if (stone == 0)
            {
                stones[i] = 1;
                continue;
            }
            // Even split
            var length = stone.ToString().Length;
            if (length % 2 == 0)
            {
                var first = stone.ToString().Substring(0, length / 2);
                var second = stone.ToString().Substring(length / 2);
                stones[i] = long.Parse(first);
                stones.Insert(i + 1, long.Parse(second));
                
                continue;
            }
            // Last Rule
            stones[i] = stone * 2024;
        }
    }
}