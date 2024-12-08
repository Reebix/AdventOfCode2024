namespace AdventOfCode2024.days;

public class Day8() : Day(8)
{
    private int _antinodes;
    private readonly List<(int, int)> _antinodesPositions = new();
    private bool _isPart2;

    protected override void Run(bool isPart2 = false)
    {
        _isPart2 = isPart2;

        for (var y = 0; y < Input.Length; y++)
        for (var x = 0; x < Input[y].Length; x++)
            CheckAntinodesForPosition(x, y);

        Console.WriteLine(_antinodes);
    }

    private void CheckAntinodesForPosition(int x, int y)
    {
        var charAtPosition = Input[y][x];
        if (charAtPosition == '.') return;

        var currentRelativePosition = (x: 0, y: 0);

        for (var i = 0; i < Input.Length; i++)
        {
            currentRelativePosition.y = i - y;

            for (var j = 0; j < Input[i].Length; j++)
            {
                if (x == j && y == i)
                {
                    // Part 2 self adding
                    if (_isPart2 && !_antinodesPositions.Contains((x, y)))
                    {
                        _antinodes++;
                        _antinodesPositions.Add((x, y));
                    }

                    continue;
                }

                currentRelativePosition.x = j - x;
                var otherChar = Input[i][j];
                if (otherChar != charAtPosition) continue;
                var otherPosition = (x: x - currentRelativePosition.x, y: y - currentRelativePosition.y);
                
                // Part 2
                if (_isPart2)
                {
                    while (IsInBounds(otherPosition.x, otherPosition.y))
                    {
                        if (!_antinodesPositions.Contains((otherPosition.x, otherPosition.y)))
                        {
                            _antinodes++;
                            _antinodesPositions.Add((otherPosition.x, otherPosition.y));
                        }

                        otherPosition.x -= currentRelativePosition.x;
                        otherPosition.y -= currentRelativePosition.y;
                    }

                    continue;
                }

                // Part 1
                if (!IsInBounds(otherPosition.x, otherPosition.y) ||
                    _antinodesPositions.Contains((otherPosition.x, otherPosition.y)))
                    continue;
                _antinodes++;
                _antinodesPositions.Add((otherPosition.x, otherPosition.y));
            }
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Input[0].Length && y >= 0 && y < Input.Length;
    }
}