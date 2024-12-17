namespace AdventOfCode2024.days;

public class Day14() : Day(14)
{
    private static readonly int width = 101;
    private static readonly int height = 103;

    protected override void Run(bool isPart2 = false)
    {
        var robots = new List<Robot>();
        for (var i = 0; i < Input.Length; i++)
        {
            var line = Input[i];
            //p=0,4 v=3,-3
            var parts = line.Split(" ");
            var p = parts[0].Replace("p=", "").Split(",");
            var v = parts[1].Replace("v=", "").Split(",");
            var robot = new Robot
            {
                position = (int.Parse(p[0]), int.Parse(p[1])),
                direction = (int.Parse(v[0]), int.Parse(v[1]))
            };
            if (!isPart2)
                for (var j = 0; j < 100; j++)
                    robot.Move();
            robots.Add(robot);
        }

        var middleHeight = height / 2;
        var middleWidth = width / 2;

        if (isPart2) goto Part2;


        var sumTopLeft = robots.Count(r => r.position.x < middleWidth && r.position.y < middleHeight);
        var sumTopRight = robots.Count(r => r.position.x > middleWidth && r.position.y < middleHeight);
        var sumBottomLeft = robots.Count(r => r.position.x < middleWidth && r.position.y > middleHeight);
        var sumBottomRight = robots.Count(r => r.position.x > middleWidth && r.position.y > middleHeight);

        // Console.WriteLine($"Top Left: {sumTopLeft}");
        // Console.WriteLine($"Top Right: {sumTopRight}");
        // Console.WriteLine($"Bottom Left: {sumBottomLeft}");
        // Console.WriteLine($"Bottom Right: {sumBottomRight}");
        var factor = sumTopLeft * sumTopRight * sumBottomLeft * sumBottomRight;
        Console.WriteLine(factor);

        return;
        Part2:
        for (int s = 0; s < 20000; s++)
        {   
            //8279
            // Console.WriteLine($"Second: {s}");
            for (var i = 0; i < robots.Count; i++)
            {
                var robot = robots[i];
                robot.Move();
                robots[i] = robot;
            }
           
            var connectedRobots = CountConnectedRobots(robots);
            if (connectedRobots < 25)
            {
                continue;
            }
            
            // I had a off by one error here
            Console.WriteLine($"Second: {s+1} Connected Robots: {connectedRobots}");
            
            
            var grid = new char[height, width];
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                grid[i, j] = '.';

            for (var i = 0; i < robots.Count; i++)
            {
                var robot = robots[i];
                grid[robot.position.y, robot.position.x] = '#';
            }

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++) Console.Write(grid[i, j]);
                Console.WriteLine();
            }
        }
    }

    
    private int CountConnectedRobots(List<Robot> robots)
{
    var visited = new bool[height, width];
    var directions = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

    int CountBlob(int x, int y)
    {
        var stack = new Stack<(int x, int y)>();
        stack.Push((x, y));
        visited[y, x] = true;
        int count = 0;

        while (stack.Count > 0)
        {
            var (cx, cy) = stack.Pop();
            count++;

            foreach (var (dx, dy) in directions)
            {
                int nx = (cx + dx + width) % width;
                int ny = (cy + dy + height) % height;

                if (!visited[ny, nx] && robots.Any(r => r.position.x == nx && r.position.y == ny))
                {
                    visited[ny, nx] = true;
                    stack.Push((nx, ny));
                }
            }
        }

        return count;
    }

    int maxBlobSize = 0;

    foreach (var robot in robots)
    {
        if (!visited[robot.position.y, robot.position.x])
        {
            maxBlobSize = Math.Max(maxBlobSize, CountBlob(robot.position.x, robot.position.y));
        }
    }

    return maxBlobSize;
}

    private struct Robot
    {
        public (int x, int y) position;
        public (int x, int y) direction;

        public override string ToString()
        {
            return "{" + $"Position: {position.x},{position.y} Direction: {direction.x},{direction.y}" + "}";
        }

        public void Move()
        {
            position.x = (position.x + direction.x + width) % width;
            position.y = (position.y + direction.y + height) % height;
        }
    }
}