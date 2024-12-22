namespace AdventOfCode2024.days;

public class Day22() : Day(22)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) goto Part2;
        long sum = 0;
        foreach (var line in Input)
        {
            var secret = long.Parse(line);
            for (var i = 0; i < 2000; i++) secret = NextSecret(secret);
            sum += secret;
        }

        Console.WriteLine(sum);
        return;

        Part2:
        var prices = new List<List<long>>();
        foreach (var line in Input)
        {
            var secret = long.Parse(line);
            var price = new List<long>();
            for (var i = 0; i < 2000; i++)
            {
                secret = NextSecret(secret);
                price.Add(GetLastDigit(secret));
            }
            prices.Add(price);
        }

        var changes = prices.Select(p => p.Zip(p.Skip(1), (a, b) => b - a).ToList()).ToList();

        var amounts = new Dictionary<(long, long, long, long), long>();
        for (var i = 0; i < changes.Count; i++)
        {
            var change = changes[i];
            var keys = new HashSet<(long, long, long, long)>();
            for (var j = 0; j < change.Count - 3; j++)
            {
                var key = (change[j], change[j + 1], change[j + 2], change[j + 3]);
                if (keys.Contains(key)) continue;
                if (!amounts.ContainsKey(key)) amounts[key] = 0;
                amounts[key] += prices[i][j + 4];
                keys.Add(key);
            }
        }

        var max = amounts.Max(a => a.Value);
        Console.WriteLine(max);
    }

    private long GetLastDigit(long secret)
    {
        return secret % 10;
    }

    private long NextSecret(long secret)
    {
        secret = ((secret * 64) ^ secret) % 16777216;
        secret = ((secret / 32) ^ secret) % 16777216;
        secret = ((secret * 2048) ^ secret) % 16777216;
        return secret;
    }
}