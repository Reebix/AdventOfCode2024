﻿var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

var i = 1;
if (args.Length == 0 && time is { Month: 12, Day: <= 25 }) i = time.Day;
else if (args.Length > 0) i = int.Parse(args[0]);
// for (; i <= time.Day && i <= 25; i++)
// {
var type = Type.GetType($"AdventOfCode2024.days.Day{i}");
if (type == null)
    Console.WriteLine($"Day {i} not found");
// continue;
else Activator.CreateInstance(type);
// }

/*
static void CreateDays()
{
    for (var i = 1; i <= 25; i++)
    {
        var path = $"../../../days/Day{i}.cs";
        if (!File.Exists(path)) File.Create(path);
        if (!File.Exists($"../../../days/Day{i}.cs"))
            File.WriteAllText($"../../../days/Day{i}.cs",
                File.ReadAllText("../../../days/TemplateDay.cs")
                    .Replace("TemplateDay", $"Day{i}")
                    .Replace("Day(1)", $"Day({i})"));
    }
}
*/