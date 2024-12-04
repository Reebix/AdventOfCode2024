using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.days;

public class Day4() : Day(4)
{
    private int _height;
    private int _width;

    protected override void Run(bool isPart2 = false)
    {
        var sum = 0;
        _width = Input[0].Length;
        _height = Input.Length;


        if (isPart2)
        {
            goto Part2;
            return;
        }

        var diagonals = new List<string>();
        var veticals = new string[_width];

        // top-bottom
        for (var i = 0; i < _height; i++)
        {
            var rLine = "";
            var lLine = "";
            var pos = new Vector2(0, i);
            while (pos.X < _width && pos.Y < _height)
            {
                rLine += Input[(int)pos.Y][(int)(_width - 1 - pos.X)];
                lLine += Input[(int)pos.Y][(int)pos.X];
                pos += Vector2.One;
            }

            diagonals.AddRange([rLine, lLine]);
        }

        // left-right
        for (var i = 1; i < _width; i++)
        {
            var lLine = "";
            var rLine = "";
            var pos = new Vector2(i, 0);
            while (pos.X < _width && pos.Y < _height)
            {
                lLine += Input[(int)pos.Y][(int)pos.X];
                rLine += Input[(int)pos.Y][(int)(_width - 1 - pos.X)];
                pos += Vector2.One;
            }

            diagonals.AddRange([rLine, lLine]);
        }

        foreach (var line in Input)
            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];
                veticals[i] += c;
            }


        sum += Input.Sum(XmasCount);
        sum += veticals.Sum(XmasCount);
        sum += diagonals.Sum(XmasCount);
        Console.WriteLine(sum);

        return;
        Part2:
        for (var x = 0; x < _width; x++)
        for (var y = 0; y < _height; y++)
        {
            var c = TryGetChar(x, y);
            if (c == 'A' && HasXAround(x, y)) sum++;
        }

        Console.WriteLine(sum);
    }

    private bool HasXAround(int x, int y)
    {
            return ((TryGetChar(x - 1, y - 1) == 'M' && TryGetChar(x + 1, y + 1) == 'S') ||
                    (TryGetChar(x - 1, y - 1) == 'S' && TryGetChar(x + 1, y + 1) == 'M'))
                   &&
                   ((TryGetChar(x + 1, y - 1) == 'M' && TryGetChar(x - 1, y + 1) == 'S') ||
                    (TryGetChar(x + 1, y - 1) == 'S' && TryGetChar(x - 1, y + 1) == 'M'));
    }

    private char TryGetChar(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return '.';
        return Input[y][x];
    }


    private int XmasCount(string line)
    {
        return Regex.Count(line, "XMAS") // Normal
               + Regex.Count(string.Join("", line.Reverse()), "XMAS"); // Reversed
    }
}