namespace AdventOfCode2024.days;

public class Day19() : Day(19)
{
    private readonly Dictionary<string, long> countCache = new();
    private readonly Dictionary<string, bool> evaluateCache = new();

    protected override void Run(bool isPart2 = false)
    {
        var patterns = Input[0].Split(", ").ToList();
        var designs = Input.Skip(2).ToList();

        if (isPart2)
        {
            var total = designs.Sum(design => CountDesign(design, patterns));
            Console.WriteLine(total);
        }
        else
        {
            var count = designs.Count(design => EvaluateDesign(design, patterns));
            Console.WriteLine(count);
        }
    }

    private bool EvaluateDesign(string design, List<string> patterns)
    {
        if (evaluateCache.ContainsKey(design)) return evaluateCache[design];

        if (design.Length == 0) return true;

        foreach (var pattern in patterns)
            if (design.StartsWith(pattern) && EvaluateDesign(design.Substring(pattern.Length), patterns))
            {
                evaluateCache[design] = true;
                return true;
            }

        evaluateCache[design] = false;
        return false;
    }

    private long CountDesign(string design, List<string> patterns)
    {
        if (countCache.ContainsKey(design)) return countCache[design];

        if (design.Length == 0) return 1;

        long count = 0;
        foreach (var pattern in patterns)
            if (design.StartsWith(pattern))
                count += CountDesign(design.Substring(pattern.Length), patterns);

        countCache[design] = count;
        return count;
    }
}