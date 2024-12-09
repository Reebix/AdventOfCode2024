using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode2024.days;

public class Day6() : Day(6)
{
    private Vector2 _added;
    private Vector2 _direction;
    private char[][] _grid;
    private Vector2 _guardPosition;
    private Vector2 _originalGuard;
    private bool _part2;
    private int _visited;
    private bool exited;
    private int ogCount;

    private List<Vector2> visited = new();

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Char[]; size: 198MB")]
    protected override void Run(bool isPart2 = false)
    {
        _direction = new Vector2(0, -1);
        exited = false;
        _part2 = isPart2;

        _guardPosition = Vector2.Zero;
        _grid = new char[Input.Length][];


        for (var y = 0; y < Input.Length; y++)
        {
            _grid[y] = new char[Input[0].Length];
            var line = Input[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == '^') _guardPosition = new Vector2(x, y);
                _grid[y][x] = Input[y][x];
            }
        }


        var grid = new char[Input.Length][];
        if (isPart2)
            for (var y = 0; y < Input.Length; y++)
            {
                grid[y] = new char[Input[0].Length];
                for (var x = 0; x < Input[0].Length; x++)
                {
                    var c = Input[y][x];
                    if (c == '^') grid[y][x] = '|';
                    else grid[y][x] = c;
                }
            }

        _originalGuard = new Vector2(_guardPosition.X, _guardPosition.Y);

        while (!exited) GuardStep();
        // foreach (var line in _grid) Console.WriteLine(string.Join("", line));
        if (!isPart2)
        {
            Console.WriteLine(_visited);
            return;
        }

        _visited = 0;
        // Trying to loop
        List<Vector2> possiblePositions = new();
        for (var y = 0; y < _grid.Length; y++)
        for (var x = 0; x < _grid[0].Length; x++)
            if (_grid[y][x] == 'X' && new Vector2(x, y) != _originalGuard)
                possiblePositions.Add(new Vector2(x, y));

        foreach (var position in possiblePositions)
        {
            visited = new List<Vector2>();
            ogCount = 0;
            _added = new Vector2(position.X, position.Y);
            _guardPosition = new Vector2(_originalGuard.X, _originalGuard.Y);
            _direction = new Vector2(0, -1);

            for (var y = 0; y < Input.Length; y++)
            {
                _grid[y] = new char[Input[0].Length];
                for (var x = 0; x < Input[0].Length; x++)
                {
                    var c = Input[y][x];
                    _grid[y][x] = c;
                    // if (c == '^') _grid[y][x] = '.';
                }
            }

            _grid[(int)position.Y][(int)position.X] = '#';


            exited = false;
            while (!exited) GuardStep();
        }

        Console.WriteLine(_visited);
    }

    private void GuardStep()
    {
        var nextPosition = _guardPosition + _direction;
        var x = (int)nextPosition.X;
        var y = (int)nextPosition.Y;
        if (x < 0 || y < 0 || x >= Input[0].Length || y >= Input.Length)
        {
            _grid[(int)_guardPosition.Y][(int)_guardPosition.X] = 'X';
            exited = true;
            if (!_part2)
                _visited++;
            return;
        }

        var c = _grid[y][x];
        if (c == '#')
        {
            var originalDirection = new Vector2(_direction.X, _direction.Y);
            _direction = _direction switch
            {
                { X: 0, Y: -1 } => new Vector2(1, 0),
                { X: 1, Y: 0 } => new Vector2(0, 1),
                { X: 0, Y: 1 } => new Vector2(-1, 0),
                { X: -1, Y: 0 } => new Vector2(0, -1),
                _ => _direction
            };

            if (_part2 && originalDirection != _direction)
            {
                if (visited.Contains(nextPosition))
                {
                    ogCount++;
                    if (ogCount > 10)
                    {
                        _visited++;
                        exited = true;
                        return;
                    }
                }
                else
                {
                    visited.Add(nextPosition);
                }
            }

            return;
        }

        if (_grid[(int)_guardPosition.Y][(int)_guardPosition.X] != 'X')
        {
            _grid[(int)_guardPosition.Y][(int)_guardPosition.X] = 'X';
            if (!_part2)
                _visited++;
        }

        _guardPosition = nextPosition;
    }
}