namespace AdventOfCode2024.days;

public class Day7() : Day(7)
{
    protected override void Run(bool isPart2 = false)
    {
        double validSum = 0;
        foreach (var line in Input)
        {
            var split = line.Split(": ");
            var desired = long.Parse(split[0]);
            var usableNumbers = split[1].Split(" ").Select(long.Parse).ToList();

            var operators = new List<char> { '+', '*' };
            if (isPart2) operators.Add('|');

            if (CheckCombinations(operators, usableNumbers, desired, usableNumbers[0], 1))
            {
                validSum += desired;
            }
        }

        Console.WriteLine(validSum);
    }
    
    private bool CheckCombinations(List<char> operators, List<long> usableNumbers, long desired, long currentResult, int index)
    {
        if (index == usableNumbers.Count)
        {
            return currentResult == desired;
        }

        // ReSharper disable once ForCanBeConvertedToForeach
        for (short i = 0; i < operators.Count; i++)
        {
            var op = operators[i];
            var newResult = op switch
            {
                '+' => currentResult + usableNumbers[index],
                '*' => currentResult * usableNumbers[index],
                '|' => Convert.ToInt64(currentResult + "" + usableNumbers[index]),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (CheckCombinations(operators, usableNumbers, desired, newResult, index + 1))
            {
                return true;
            }
        }

        return false;
    }
}