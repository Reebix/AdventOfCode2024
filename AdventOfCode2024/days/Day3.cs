using System.Text.RegularExpressions;

namespace AdventOfCode2024.days;

public class Day3() : Day(3)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;

        var result = 0;
        var result2 = 0;
        var line = "";
        Input.ToList().ForEach(x => line += x);

        var enabled = true;
        foreach (Match match in Regex.Matches(line, "mul\\(\\d+,\\d+\\)|do(|n't)\\(\\)"))
        {
            if (match.ToString() == "do()")
            {
                enabled = true;
                continue;
            }
            if (match.ToString() == "don't()")
            {
                enabled = false;
                continue;
            }

            var numbers = Regex.Replace(match.ToString(), "mul\\(|\\)", "").Split(",");
            var a = int.Parse(numbers[0]);
            var b = int.Parse(numbers[1]);
            result += a * b;
            if (enabled)
                result2 += a * b;
        }

        Console.WriteLine(result);
        Console.WriteLine();
        Console.WriteLine(result2);
    }
}