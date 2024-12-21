namespace AdventOfCode2024.days;

public class Day18() : Day(18)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        var one = true;
        var dimensions = 71;
        var readingLength = 2023;
        while (true)
        {
            readingLength++;
            var grid = new bool[dimensions, dimensions];

            for (var index = 0; index < readingLength; index++)
            {
                var se = Input[index];
                var split = se.Split(",");
                var x = int.Parse(split[0]);
                var y = int.Parse(split[1]);
                grid[x, y] = true;
            }

            // PrintGrid(grid, dimensions);

            var shortestPath = int.MaxValue;
            var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            var visited = new bool[dimensions, dimensions];
            var start = (0, 0);
            var end = (dimensions - 1, dimensions - 1);


            var queue = new Queue<((int, int) position, int pathLength)>();
            queue.Enqueue((start, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (current, pathLength) = queue.Dequeue();
                if (current == end)
                {
                    shortestPath = Math.Min(shortestPath, pathLength);
                    break;
                }

                foreach (var (dx, dy) in directions)
                {
                    var next = (current.Item1 + dx, current.Item2 + dy);
                    if (next.Item1 >= 0 && next.Item1 < dimensions && next.Item2 >= 0 && next.Item2 < dimensions &&
                        !visited[next.Item1, next.Item2] && !grid[next.Item1, next.Item2])
                    {
                        visited[next.Item1, next.Item2] = true;
                        queue.Enqueue((next, pathLength + 1));
                    }
                }
            }

            if (one)
            {
                Console.WriteLine(shortestPath);
                one = false;
            } else
            {
                if (shortestPath == int.MaxValue)
                {
                    Console.WriteLine();
                    
                    Console.WriteLine(Input[readingLength - 1]);
                    break;
                }
            }
        }
    }

    private void PrintGrid(bool[,] grid, int dimensions)
    {
        for (var i = 0; i < dimensions; i++)
        {
            for (var j = 0; j < dimensions; j++) Console.Write(grid[j, i] ? "#" : ".");
            Console.WriteLine();
        }
    }
}