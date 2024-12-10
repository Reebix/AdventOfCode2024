namespace AdventOfCode2024.days;

public class Day10() : Day(10)
{
    private int _sum;
    private int _sumpt2;
    private bool _isPart2;
    
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        for (var y = 0; y < Input.Length; y++)
        for (var x = 0; x < Input[y].Length; x++)
        {
            var c = Input[y][x];
            if (c == '.') continue; // DEBUG
            if (c == '0')
            {
                var visited = new List<(int, int)>();
                TraversePath(0, x, y - 1, visited); // UP
                TraversePath(0, x, y + 1, visited); // DOWN
                TraversePath(0, x - 1, y, visited); // LEFT
                TraversePath(0, x + 1, y, visited); // RIGHT
            }
        }
        Console.WriteLine(_sum);
        Console.WriteLine();
        Console.WriteLine(_sumpt2);
    }

    private void TraversePath(int prev, int x, int y, List<(int, int)> visited)
    {
        if (!IsInside(x, y)) return;
       
        var c = Input[y][x];
        if (c == '.') return; // DEBUG
        var next = prev + 1;
        var num = int.Parse(c + "");
        if (num == next)
        {
            if(num == 9)
            {
                _sumpt2++;
                if (visited.Contains((x, y))) return;
                visited.Add((x, y));
                _sum++;
                return;
            }
            TraversePath(next, x, y - 1,visited); // UP
            TraversePath(next, x, y + 1,visited); // DOWN
            TraversePath(next, x - 1, y,visited); // LEFT
            TraversePath(next, x + 1, y,visited); // RIGHT
        }
        // ADD COUNTING
        return;
    }

    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < Input.Length && y >= 0 && y < Input[0].Length;
    }
}