namespace AdventOfCode2024.days;

public class Day5() : Day(5)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;

        var rules = new Dictionary<int, List<int>>();
        if (rules == null) throw new ArgumentNullException(nameof(rules));
        var pages = new List<int>();
        var readingRules = true;
        foreach (var line in Input)
        {
            if (line == "")
            {
                readingRules = false;
                continue;
            }

            if (readingRules)
            {
                var split = line.Split("|");
                var first = int.Parse(split[0]);
                var second = int.Parse(split[1]);
                if(!rules.ContainsKey(first)) rules[first] = new List<int>();
                rules[first].Add(second);
            }
            else
            {
                pages.AddRange(line.Split(",").Select(int.Parse));
            }
        }

        for (var index = 0; index < pages.Count; index++)
        {
            var page = pages[index];
            if (rules.ContainsKey(page))
            {
                // Console.WriteLine(page + " -> " + String.Join(", ", rules[page]));
                List<int> notAllowed = rules[page];
                var slice = pages.Slice(0, index);
                // Console.WriteLine(page + " -> " + String.Join(", ", slice));
                var valid = true;
                foreach (var i in slice)
                {
                    if (notAllowed.Contains(i))
                    {
                        valid = false;
                        break;
                    }
                }
                Console.WriteLine(page + " -> " + valid);
            }
        }
    }
}