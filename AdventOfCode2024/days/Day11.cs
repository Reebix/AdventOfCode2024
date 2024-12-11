namespace AdventOfCode2024.days;

public class Day11() : Day(11)
{
    private Dictionary<long, long> _allStones = new();
    
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        var stones = Input[0].Split(" ").Select(long.Parse).ToList();
        
        
        
        for (var i = 0; i < stones.Count; i++)
        {
            var stone = stones[i];
            _allStones[stone] = 1;
        }
        
        // Part 1
        for (var i = 0; i < 25; i++)
        {
            FullBlink();
        }
        
        Console.WriteLine(_allStones.Values.Sum());
        Console.WriteLine("");
        
        
        // Part 2
        for (int i = 0; i < 50; i++)
        {
            FullBlink();
        }
        Console.WriteLine(_allStones.Values.Sum());
    }

   private void FullBlink()
{
    var newStones = new Dictionary<long, long>();
    foreach (var stone in _allStones)
    {
        var blinkedStones = Blink(stone.Key);
        foreach (var newStone in blinkedStones)
        {
            if (newStones.ContainsKey(newStone))
            {
                newStones[newStone] += stone.Value;
            }
            else
            {
                newStones[newStone] = stone.Value;
            }
        }
    }

    _allStones = newStones;
}


    //TODO: move to two arrays and parallelize
    private List<long> Blink(long stone)
    {
        // 0 replace
        if (stone == 0) return new List<long> { 1 };
        // Even split
        var length = stone.ToString().Length;
        if (length % 2 == 0)
        {
            var first = stone.ToString().Substring(0, length / 2);
            var second = stone.ToString().Substring(length / 2);
            return new List<long> { long.Parse(first), long.Parse(second) };
        }

        // Last Rule
        return new List<long> { stone * 2024 };
    }
}