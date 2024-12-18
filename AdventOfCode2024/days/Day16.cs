using System;
using System.Collections.Generic;

namespace AdventOfCode2024.days
{
    public class Day16 : Day
    {
        private static readonly (int, int)[] Directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };

        public Day16() : base(16) { }

        protected override void Run(bool isPart2 = false)
        {
            if (isPart2) return;

            var map = new char[,] { };
            for (var i = 0; i < Input.Length; i++)
            {
                if (i == 0)
                {
                    map = new char[Input.Length, Input[i].Length];
                }

                for (var j = 0; j < Input[i].Length; j++)
                {
                    map[i, j] = Input[i][j];
                }
            }

            var (start, end) = FindStartAndEnd(map);
            var (result, paths) = FindPath(map, start, end);
            Console.WriteLine($"Minimum score to reach the end: {result}");
           
        }

        private ((int, int), (int, int)) FindStartAndEnd(char[,] map)
        {
            (int, int) start = (-1, -1);
            (int, int) end = (-1, -1);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'S')
                    {
                        start = (i, j);
                    }
                    else if (map[i, j] == 'E')
                    {
                        end = (i, j);
                    }
                }
            }

            return (start, end);
        }

        private (int, List<List<(int, int)>>) FindPath(char[,] map, (int, int) start, (int, int) end)
        {
            var priorityQueue = new PriorityQueue<((int, int) pos, int dir, int score, List<(int, int)> path), int>();
            var visited = new HashSet<((int, int), int)>();
            var paths = new List<List<(int, int)>>();
            int minScore = int.MaxValue;

            priorityQueue.Enqueue((start, 0, 0, new List<(int, int)> { start }), 0); // Start facing East
            visited.Add((start, 0));

            while (priorityQueue.Count > 0)
            {
                var (pos, dir, score, path) = priorityQueue.Dequeue();
                if (pos == end)
                {
                    if (score < minScore)
                    {
                        minScore = score;
                        paths.Clear();
                    }
                    if (score == minScore)
                    {
                        paths.Add(path);
                    }
                    continue;
                }

                // Move forward
                var newPos = (pos.Item1 + Directions[dir].Item1, pos.Item2 + Directions[dir].Item2);
                if (IsValidMove(map, newPos) && !visited.Contains((newPos, dir)))
                {
                    var newPath = new List<(int, int)>(path) { newPos };
                    priorityQueue.Enqueue((newPos, dir, score + 1, newPath), score + 1);
                    visited.Add((newPos, dir));
                }

                // Rotate clockwise
                var newDir = (dir + 1) % 4;
                if (!visited.Contains((pos, newDir)))
                {
                    priorityQueue.Enqueue((pos, newDir, score + 1000, new List<(int, int)>(path)), score + 1000);
                    visited.Add((pos, newDir));
                }

                // Rotate counterclockwise
                newDir = (dir + 3) % 4;
                if (!visited.Contains((pos, newDir)))
                {
                    priorityQueue.Enqueue((pos, newDir, score + 1000, new List<(int, int)>(path)), score + 1000);
                    visited.Add((pos, newDir));
                }
            }

            return (minScore, paths); // Return the minimum score and all paths with that score
        }

        private bool IsValidMove(char[,] map, (int, int) pos)
        {
            return pos.Item1 >= 0 && pos.Item1 < map.GetLength(0) && pos.Item2 >= 0 && pos.Item2 < map.GetLength(1) && map[pos.Item1, pos.Item2] != '#';
        }
    }
}