namespace AdventOfCode2024.days;

public class Day12() : Day(12)
{
    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;
        var map = new char[Input.Length][];
        for (var i = 0; i < Input.Length; i++)
        {
            map[i] = new char[Input[0].Length];
            for (var j = 0; j < Input[0].Length; j++)
                map[i][j] = Input[i][j];
        }

        var areas = new List<char[][]>();
        for (var i = 0; i < Input.Length; i++)
        for (var j = 0; j < Input[0].Length; j++)
            if (map[i][j] != '#')
            {
                var area = IsolateAndRemoveArea(map, j, i);
                areas.Add(area);
            }

        var total = 0;
        for (var i = 0; i < areas.Count; i++)
        {
            var area = areas[i];
            var areaArea = GetAreaArea(area);
            var surrounding = GetSurrounding(area);
            var price = areaArea * surrounding;
            total += price;
            // Console.WriteLine($"Area {i + 1} has an area of {areaArea} and is surrounded by {surrounding}");
        }
        Console.WriteLine(total);
        
        // Part 2
        total = 0;
        for (var i = 0; i < areas.Count; i++)
        {
            var area = areas[i];
            var edges = GetEdges(area);
            var areaArea = GetAreaArea(area);
            total += areaArea * edges;
        }
        Console.WriteLine();
        Console.WriteLine(total);
    }

    private int GetEdges(char[][] area)
    {
        var total = 0;
        var edgeOne = false;
        var edgeTwo = false;
        // Horizontal
        for (var y = 0; y < area.Length; y++)
        {
            edgeOne = false;
            edgeTwo = false;
            for (var x = 0; x < area[0].Length; x++)
            {
                if (area[y][x] == '#')
                {
                    if (edgeTwo) total++;
                    edgeTwo = false;
                    if (edgeOne) total++;
                    edgeOne = false;
                    continue;
                };
                var c = area[y][x];
                // TOP
                if (!IsInside(area, x, y - 1) || area[y - 1][x] == '#')
                {
                    edgeOne = true;
                }
                else
                {
                    if (edgeOne) total++;
                    edgeOne = false;
                }
    
                // BOTTOM
                if (!IsInside(area, x, y + 1) || area[y + 1][x] == '#')
                {
                    edgeTwo = true;
                }
                else
                {
                    if (edgeTwo) total++;
                    edgeTwo = false;
                }
            }
            
            if (edgeOne) total++;
            if (edgeTwo) total++;
        }
        
        // Vertical
        for (var x = 0; x < area[0].Length; x++)
        {
            edgeOne = false;
            edgeTwo = false;
            for (var y = 0; y < area.Length; y++)
            {
                if (area[y][x] == '#')
                {
                    if (edgeTwo) total++;
                    edgeTwo = false;
                    if (edgeOne) total++;
                    edgeOne = false;
                    continue;
                };
                var c = area[y][x];
                // LEFT
                if (!IsInside(area, x - 1, y) || area[y][x - 1] == '#')
                {
                    edgeOne = true;
                }
                else
                {
                    if (edgeOne) total++;
                    edgeOne = false;
                }

                // RIGHT
                if (!IsInside(area, x + 1, y) || area[y][x + 1] == '#')
                {
                    edgeTwo = true;
                }
                else
                {
                    if (edgeTwo) total++;
                    edgeTwo = false;
                }
            }
            
            if (edgeOne) total++;
            if (edgeTwo) total++;
        }

        return  total;
    }
    
    private bool IsInside(char[][] area, int x, int y)
    {
        return x >= 0 && x < area[0].Length && y >= 0 && y < area.Length;
    }
    

    private int GetSurrounding(char[][] area)
    {
        var total = 0;
        for (var i = 0; i < area.Length; i++)
        for (var j = 0; j < area[0].Length; j++)
            if (area[i][j] != '#')
                total += 4-GetSurroundingAmount(area, j, i);
        return total;
    }

    private int GetSurroundingAmount(char[][] area, int x, int y)
    {
        var c = area[y][x];
        var surrounding = 0;
        if (x - 1 >= 0 && area[y][x - 1] == c) surrounding++;
        if (x + 1 < area[0].Length && area[y][x + 1] == c) surrounding++;
        if (y - 1 >= 0 && area[y - 1][x] == c) surrounding++;
        if (y + 1 < area.Length && area[y + 1][x] == c) surrounding++;
        
        return surrounding;
    }

    private int GetAreaArea(char[][] area)
    {
        var areaArea = 0;
        for (var i = 0; i < area.Length; i++)
        for (var j = 0; j < area[0].Length; j++)
            if (area[i][j] != '#')
                areaArea++;
        return areaArea;
    }

    private char[][] IsolateAndRemoveArea(char[][] map, int x, int y)
    {
        var isoMap = new char[map.Length][];
        for (var l = 0; l < Input.Length; l++)
        {
            isoMap[l] = new char[Input[0].Length];
            for (var i = 0; i < isoMap[l].Length; i++)
            {
                isoMap[l][i] = '#';
            }
        }
        var c = map[y][x];
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));

        while (queue.Count > 0)
        {
            var (cx, cy) = queue.Dequeue();
            if (cx < 0 || cx >= map[0].Length || cy < 0 || cy >= map.Length) continue;
            if (map[cy][cx] != c) continue;
            if (isoMap[cy][cx] == c) continue;
            isoMap[cy][cx] = c;
            map[cy][cx] = '#';
            queue.Enqueue((cx + 1, cy));
            queue.Enqueue((cx - 1, cy));
            queue.Enqueue((cx, cy + 1));
            queue.Enqueue((cx, cy - 1));
        }

        return isoMap;
    }
}