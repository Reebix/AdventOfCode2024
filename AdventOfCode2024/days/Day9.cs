namespace AdventOfCode2024.days;

public class Day9() : Day(9)
{
    protected override void Run(bool isPart2 = false)
    {
        var disk = new List<int>();
        var line = Input[0];

        var spaces = false;
        var id = 0;
        foreach (var c in line)
        {
            var num = int.Parse(c + "");

            for (var i = 0; i < num; i++)
                if (spaces) disk.Add(-1);
                else disk.Add(id);

            spaces = !spaces;

            if (!spaces) id++;
        }

        if (isPart2) goto Part2;

        while (MoveDisk(disk))
        {
        }

        Console.WriteLine(GetChecksum(disk));

        return;
        Part2:
        var queue = new List<(int, int)>();

        var segmentLength = 1;
        var last = -2;
        for (var i = disk.Count - 1; i >= 0; i--)
        {
            var current = disk[i];
            if (current == last)
            {
                segmentLength++;
            }
            else
            {
                if (last != -1)
                    queue.Add((i + 1, segmentLength));
                segmentLength = 1;
            }

            last = current;
        }

        queue.RemoveAt(0);

        for (var i = 0; i < queue.Count; i++)
        {
            var item = queue[i];
            TryMove(disk, item.Item1, item.Item2);
        }
        Console.WriteLine(GetChecksum(disk));
        
    }

    private void TryMove(List<int> disk, int startIndex, int length)
    {
        var segment = disk.GetRange(startIndex, length);
        var segmentLength = 0;
        for (var i = 0; i <= startIndex; i++)
            if (disk[i] == -1)
            {
                segmentLength++;
            }
            else
            {
                if (segmentLength >= length)
                {
                    for (var j = 0; j < length; j++)
                    {
                        disk[i - segmentLength + j] = segment[j];
                        disk[startIndex + j] = -1;
                    }
                    return;
                }

                segmentLength = 0;
            }
    }


    private long GetChecksum(List<int> disk)
    {
        long sum = 0;
        for (var i = 0; i < disk.Count; i++)
        {
            if (disk[i] == -1) continue;
            sum += i * disk[i];
        }

        return sum;
    }

    private bool MoveDisk(List<int> disk)
    {
        var firstFree = disk.FindIndex(x => x == -1);
        if (firstFree == -1) return false;

        var last = disk[^1];
        disk.RemoveAt(disk.Count - 1);
        if (last == -1) return true;

        disk[firstFree] = last;
        return true;
    }

    private void PrintDisk(List<int> disk)
    {
        foreach (var i in disk)
            if (i == -1)
                Console.Write(".");
            else Console.Write(i);
    }
}