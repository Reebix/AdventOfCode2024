namespace AdventOfCode2024.days;

public class Day7() : Day(7)
{
    protected override void Run(bool isPart2 = false)
    {
        double validSum = 0;
        foreach (var line in Input)
        {
            var split = line.Split(": ");
            var desired = double.Parse(split[0]);
            var usableNumbers = split[1].Split(" ").Select(double.Parse).ToList();

            var operators = new List<char> { '+', '*'};
            if (isPart2) operators.Add('|');

            var actionList = new List<string>();
            GenerateOperatorCombinations(operators, "", usableNumbers.Count - 1, actionList);


            foreach (var action in actionList)
            {
                var result = usableNumbers[0];
                for (var i = 0; i < action.Length; i++)
                {
                    var op = action[i];
                    result = op switch
                    {
                        '+' => result + usableNumbers[i + 1],
                        '*' => result * usableNumbers[i + 1],
                        '|' => double.Parse(result + "" + usableNumbers[i + 1]),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }

                if (result == desired)
                {
                    validSum += desired;
                    break;
                }
            }
        }

        Console.WriteLine(validSum);
    }

    private void GenerateOperatorCombinations(List<char> operators, string current, int remaining,
        List<string> actionList)
    {
        if (remaining == 0)
        {
            actionList.Add(current);
            return;
        }

        foreach (var op in operators) GenerateOperatorCombinations(operators, current + op, remaining - 1, actionList);
    }
}