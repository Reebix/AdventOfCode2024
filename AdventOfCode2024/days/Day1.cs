using System.Diagnostics;

namespace AdventOfCode2024.days;

public class Day1() : Day(1)
{
    protected override void Run(bool isPart2 = false)
    {
        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var line in Input)
        {
            var split = line.Split("   ");
            list1.Add(int.Parse(split[0]));
            list2.Add(int.Parse(split[1]));
        }
        
        if (isPart2) goto Part2;
        
        list1.Sort();
        list2.Sort();
        
        var diff = list1.Zip(list2).Select(x => Math.Abs(x.First - x.Second)).Sum();
        
        // Print the result
        Console.WriteLine(diff);
        return;
        
        
        Part2:
        Dictionary<int, int> similarities = new();
        foreach (var num in list1)
        {
            if(similarities.ContainsKey(num)) continue;
            var sum = list2.FindAll(x => x == num).Count();
            similarities.Add(num, sum);
        }
        var result = list1.Select(x => x * similarities[x]).Sum();
        
        // Print the result
        Console.WriteLine(result);
    }
}