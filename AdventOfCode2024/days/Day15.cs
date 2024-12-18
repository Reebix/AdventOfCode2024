namespace AdventOfCode2024.days;

public class Day15() : Day(15)
{
    private (int, int) botPos = (0, 0);

    protected override void Run(bool isPart2 = false)
    {
        var splitindex = 0;
        for (var i = 0; i < Input.Length; i++)
            if (Input[i] == "")
            {
                splitindex = i;
                break;
            }

        var map = new char[splitindex, Input[0].Length];

        for (var i = 0; i < splitindex; i++)
        for (var j = 0; j < Input[i].Length; j++)
        {
            map[i, j] = Input[i][j];
            if (Input[i][j] == '@') botPos = (i, j);
        }

        var actions = new List<(int, int)>();
        for (var i = splitindex + 1; i < Input.Length; i++)
        {
            var line = Input[i];
            for (var i1 = 0; i1 < line.Length; i1++)
                if (line[i1] == '>')
                    actions.Add((1, 0));
                else if (line[i1] == '<')
                    actions.Add((-1, 0));
                else if (line[i1] == '^')
                    actions.Add((0, -1));
                else if (line[i1] == 'v') actions.Add((0, 1));
        }

        if (isPart2) goto Part2;


        foreach (var action in actions) AttemptMove(map, action);

        var sum = 0;
        for (var i = 0; i < map.GetLength(0); i++)
        for (var j = 0; j < map.GetLength(1); j++)
            if (map[i, j] == 'O')
                sum += 100 * i + j;

        Console.WriteLine(sum);

        return;

        Part2:
        var newMap = new char[map.GetLength(0), map.GetLength(1) * 2];

        for (var i = 0; i < map.GetLength(0); i++)
        for (var j = 0; j < map.GetLength(1); j++)
            if (map[i, j] == '#')
            {
                newMap[i, j * 2] = '#';
                newMap[i, j * 2 + 1] = '#';
            }
            else if (map[i, j] == 'O')
            {
                newMap[i, j * 2] = '[';
                newMap[i, j * 2 + 1] = ']';
            }
            else if (map[i, j] == '.')
            {
                newMap[i, j * 2] = '.';
                newMap[i, j * 2 + 1] = '.';
            }
            else if (map[i, j] == '@')
            {
                newMap[i, j * 2] = '@';
                botPos = (j * 2, i);
                newMap[i, j * 2 + 1] = '.';
            }

        // for (var i = 0; i < newMap.GetLength(0); i++)
        // {
            // for (var j = 0; j < newMap.GetLength(1); j++) Console.Write(newMap[i, j]);

            // Console.WriteLine();
        // }

        foreach (var action in actions)
        {
            if (AttemptMovePart2(newMap, action, botPos))
                botPos = (botPos.Item1 + action.Item1, botPos.Item2 + action.Item2);

            // for (var i = 0; i < newMap.GetLength(0); i++)
            // {
                // for (var j = 0; j < newMap.GetLength(1); j++) Console.Write(newMap[i, j]);

                // Console.WriteLine();
            // }
        }
    }

   private bool AttemptMovePart2(char[,] map, (int, int) action, (int, int) pos, bool side = false)
{
    var newX = pos.Item1 + action.Item1; // Column (x-coordinate)
    var newY = pos.Item2 + action.Item2; // Row (y-coordinate)

    if (newY < 0 || newY >= map.GetLength(0) || newX < 0 || newX >= map.GetLength(1))
        // Out of bounds
        return false;

    var c = map[pos.Item2, pos.Item1];

    if (c == '[' || c == ']')
    {
        var otherPartX = c == '[' ? pos.Item1 + 1 : pos.Item1 - 1;
        var otherPartY = pos.Item2;

        if (!side)
        {
            if (AttemptMovePart2(map, action, (otherPartX, otherPartY), true) && AttemptMovePart2(map, action, (newX, newY)))
            {
                map[pos.Item2, pos.Item1] = '.';
                map[newY, newX] = c;
                return true;
            }
        }
        else if (AttemptMovePart2(map, action, (newX, newY)))
        {
            map[pos.Item2, pos.Item1] = '.';
            map[newY, newX] = c;
            return true;
        }

        return false;
    }

    if (map[newY, newX] == '#')
        // Wall, cannot move
        return false;

    if (map[newY, newX] == '.')
    {
        // Empty space, move bot or crate
        map[pos.Item2, pos.Item1] = '.';
        map[newY, newX] = c;
        return true;
    }

    if (c == '@' && AttemptMovePart2(map, action, (newX, newY)))
    {
        map[pos.Item2, pos.Item1] = '.';
        map[newY, newX] = c;
        return true;
    }

    return false;
}


    private void AttemptMove(char[,] map, (int, int) action)
    {
        var newY = botPos.Item2 + action.Item2; // Row (y-coordinate)
        var newX = botPos.Item1 + action.Item1; // Column (x-coordinate)

        if (newY < 0 || newY >= map.GetLength(0) || newX < 0 || newX >= map.GetLength(1))
            // Out of bounds
            return;

        if (map[newY, newX] == '#')
            // Wall, cannot move
            return;

        if (map[newY, newX] == '.')
        {
            // Empty space, move bot
            map[botPos.Item2, botPos.Item1] = '.';
            map[newY, newX] = '@';
            botPos = (newX, newY);
            return;
        }

        if (map[newY, newX] == 'O')
        {
            // Obstacle, try to push
            var pushY = newY;
            var pushX = newX;

            // Check if we can push the entire row of crates
            while (pushY >= 0 && pushY < map.GetLength(0) && pushX >= 0 && pushX < map.GetLength(1) &&
                   map[pushY, pushX] == 'O')
            {
                pushY += action.Item2;
                pushX += action.Item1;
            }

            if (pushY < 0 || pushY >= map.GetLength(0) || pushX < 0 || pushX >= map.GetLength(1) ||
                map[pushY, pushX] != '.')
                // Out of bounds or cannot push
                return;

            // Push the entire row of crates
            while (pushY != newY || pushX != newX)
            {
                pushY -= action.Item2;
                pushX -= action.Item1;
                map[pushY + action.Item2, pushX + action.Item1] = 'O';
            }

            // Move bot
            map[newY, newX] = '@';
            map[botPos.Item2, botPos.Item1] = '.';
            botPos = (newX, newY);
        }
    }
}