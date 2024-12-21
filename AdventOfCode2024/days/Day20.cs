namespace AdventOfCode2024.days
{

    public class Day20() : Day(20)
    {
        protected override void Run(bool isPart2 = false)
        {
            if(isPart2) return;
            var start = FindStart(Input);
            var path = SearchPath(Input, start);
            var pathLength = path.Count - 1;

            var distances = new Dictionary<(int, int), int>();
            for (int i = 0; i < path.Count; i++)
            {
                distances[path[i]] = i;
            }

            var savings2 = path
                .SelectMany(position => CheatPositions(Input, position)
                    .Select(cheat =>
                    {
                        var fromStart = distances[position];
                        var toEnd = pathLength - distances[cheat];
                        var saved = pathLength - (fromStart + toEnd + 2);
                        return saved > 0 ? (int?)saved : null;
                    })
                    .Where(saved => saved.HasValue)
                    .Select(saved => saved.Value))
                .ToList();

            var result = savings2.Count(x => x >= 100);
            Console.WriteLine(result);
            Console.WriteLine();

            var savings20 = path
                .SelectMany(position => Reachable(Input, position)
                    .Select(cheat =>
                    {
                        var fromStart = distances[position];
                        var toEnd = pathLength - distances[cheat.Item1];
                        var saved = pathLength - (fromStart + toEnd + cheat.Item2);
                        return saved > 0 ? (int?)saved : null;
                    })
                    .Where(saved => saved.HasValue)
                    .Select(saved => saved.Value))
                .ToList();

            var result20 = savings20.Count(x => x >= 100);
            Console.WriteLine(result20);
        }
        
        
    static (int, int) FindStart(string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == 'S')
                {
                    return (i, j);
                }
            }
        }
        throw new Exception("Start position not found");
    }

    static List<(int, int)> SearchPath(string[] input, (int, int) start)
    {
        var q = new Queue<List<(int, int)>>();
        q.Enqueue(new List<(int, int)> { start });
        var visited = new HashSet<(int, int)> { start };

        while (q.Count > 0)
        {
            var pathSoFar = q.Dequeue();
            var (x, y) = pathSoFar[0];
            if (input[x][y] == 'E')
            {
                pathSoFar.Reverse();
                return pathSoFar;
            }

            var nextPositions = new List<(int, int)>
            {
                (x + 1, y),
                (x - 1, y),
                (x, y + 1),
                (x, y - 1)
            };

            foreach (var pos in nextPositions)
            {
                if (input[pos.Item1][pos.Item2] != '#' && !visited.Contains(pos))
                {
                    visited.Add(pos);
                    var newPath = new List<(int, int)> { pos };
                    newPath.AddRange(pathSoFar);
                    q.Enqueue(newPath);
                }
            }
        }

        throw new Exception("Path to end not found");
    }

    static List<(int, int)> CheatPositions(string[] input, (int, int) pos)
    {
        var nextPositions = Next(pos)
            .Where(p => input[p.Item1][p.Item2] == '#')
            .ToList();

        var nextNext = nextPositions
            .SelectMany(p => Next(p)
                .Where(np => np.Item1 >= 0 && np.Item1 < input.Length &&
                             np.Item2 >= 0 && np.Item2 < input[0].Length &&
                             (np.Item1 != pos.Item1 || np.Item2 != pos.Item2) &&
                             input[np.Item1][np.Item2] != '#'))
            .ToList();

        return nextNext;
    }

    static List<(int, int)> Next((int, int) pos)
    {
        return new List<(int, int)>
        {
            (pos.Item1 + 1, pos.Item2),
            (pos.Item1 - 1, pos.Item2),
            (pos.Item1, pos.Item2 + 1),
            (pos.Item1, pos.Item2 - 1)
        };
    }

    static List<((int, int), int)> Reachable(string[] input, (int, int) pos)
    {
        var reachable = new List<((int, int), int)>();
        for (int x = 0; x < input.Length; x++)
        {
            for (int y = 0; y < input[0].Length; y++)
            {
                var steps = Math.Abs(x - pos.Item1) + Math.Abs(y - pos.Item2);
                if (steps <= 20 && input[x][y] != '#' && (x != pos.Item1 || y != pos.Item2))
                {
                    reachable.Add(((x, y), steps));
                }
            }
        }
        return reachable;
    }
}
    
}