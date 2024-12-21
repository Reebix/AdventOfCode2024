using System.Numerics;

namespace AdventOfCode2024.days;

public class Day21() : Day(21)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;

        var lines = Input;
        var numPadLines = new[] { "789", "456", "123", ".0A" };
        var dirPadLines = new[] { ".^A", "<v>" };

        var numPad = numPadLines.SelectMany((line, i) => line.Select((c, j) => ((long)i, (long)j, c)))
            .Where(t => t.c != '.')
            .ToDictionary(t => (t.Item1, t.Item2), t => t.c);
        var dirPad = dirPadLines.SelectMany((line, i) => line.Select((c, j) => ((long)i, (long)j, c)))
            .Where(t => t.c != '.')
            .ToDictionary(t => (t.Item1, t.Item2), t => t.c);

        var numPadReverse = numPad.ToDictionary(kvp => kvp.Value, kvp => (kvp.Key.Item1, kvp.Key.Item2));
        var dirPadReverse = dirPad.ToDictionary(kvp => kvp.Value, kvp => (kvp.Key.Item1, kvp.Key.Item2));

        string Step(char source, char target, Dictionary<char, (long, long)> pad,
            Dictionary<(long, long), char> padReverse)
        {
            var (si, sj) = pad[source];
            var (ti, tj) = pad[target];
            var di = ti - si;
            var dj = tj - sj;
            var vert = new string('v', Math.Max(0, (int)di)) + new string('^', Math.Max(0, (int)-di));
            var horiz = new string('>', Math.Max(0, (int)dj)) + new string('<', Math.Max(0, (int)-dj));

            if (dj > 0 && padReverse.ContainsKey((ti, sj)))
                return vert + horiz + "A";
            if (padReverse.ContainsKey((si, tj)))
                return horiz + vert + "A";
            if (padReverse.ContainsKey((ti, sj)))
                return vert + horiz + "A";
            return string.Empty;
        }

        string Routes(string path, Dictionary<char, (long, long)> pad, Dictionary<(long, long), char> padReverse)
        {
            var outList = new List<string>();
            var start = 'A';
            foreach (var end in path)
            {
                outList.Add(Step(start, end, pad, padReverse));
                start = end;
            }

            return string.Join("", outList);
        }

        var numRoutes = lines.Select(line => Routes(line, numPadReverse, numPad)).ToList();
        var radRoutes = numRoutes.Select(route => Routes(route, dirPadReverse, dirPad)).ToList();
        var coldRoutes = radRoutes.Select(route => Routes(route, dirPadReverse, dirPad)).ToList();
        var p1 = checked(coldRoutes
            .Zip(lines, (route, line) => route.Length * long.Parse(line.Substring(0, line.Length - 1))).Sum());
        Console.WriteLine("Part 1: " + p1);

        Dictionary<string, long> Routes2(string path, Dictionary<char, (long, long)> pad,
            Dictionary<(long, long), char> padReverse)
        {
            var outDict = new Dictionary<string, long>();
            var start = 'A';
            foreach (var end in path)
            {
                var step = Step(start, end, pad, padReverse);
                if (outDict.ContainsKey(step))
                    outDict[step]++;
                else
                    outDict[step] = 1;
                start = end;
            }

            return outDict;
        }

        long RouteLen(Dictionary<string, long> route)
        {
            return route.Sum(kv => kv.Key.Length * kv.Value);
        }

        var robotRoutes = numRoutes.Select(route => new Dictionary<string, long> { { route, 1 } }).ToList();
        for (var i = 0; i < 25; i++)
        {
            var newRoutes = new List<Dictionary<string, long>>();
            foreach (var routeCounter in robotRoutes)
            {
                var newRoute = new Dictionary<string, long>();
                foreach (var kv in routeCounter)
                {
                    var newCounts = Routes2(kv.Key, dirPadReverse, dirPad);
                    foreach (var k in newCounts.Keys.ToList())
                        newCounts[k] *= kv.Value;
                    foreach (var k in newCounts)
                        if (newRoute.ContainsKey(k.Key))
                            newRoute[k.Key] += k.Value;
                        else
                            newRoute[k.Key] = k.Value;
                }

                newRoutes.Add(newRoute);
            }

            robotRoutes = newRoutes;
        }

        var p2 = checked(robotRoutes.Zip(lines,
                (route, line) => RouteLen(route) * BigInteger.Parse(line.Substring(0, line.Length - 1)))
            .Aggregate(BigInteger.Zero, (acc, val) => acc + val));
        Console.WriteLine("Part 2: " + p2);
    }
}