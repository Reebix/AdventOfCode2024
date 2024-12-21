namespace AdventOfCode2024.days;

public class Day17() : Day(17)
{
    private int _registerA;
    private int _registerB;
    private int _registerC;

    protected override void Run(bool isPart2 = false)
    {
        if (isPart2) return;

        _registerA = int.Parse(Input[0].Split(" ")[2]);
        _registerB = int.Parse(Input[1].Split(" ")[2]);
        _registerC = int.Parse(Input[2].Split(" ")[2]);

        var program = new List<int>();
        var result = new List<int>();
        Input[4].Split(" ")[1].Split(",").ToList().ForEach(x => program.Add(int.Parse(x)));
        var instructionPointer = 0;

        while (instructionPointer < program.Count)
        {
            var opcode = program[instructionPointer];
            var operant = program[instructionPointer + 1];

            // adv
            if (opcode == 0)
            {
                _registerA = (int)(_registerA / Math.Pow(2, GetComboOperant(operant)));
            }
            //bxl
            else if (opcode == 1)
            {
                _registerB = _registerB ^ GetComboOperant(operant);
            }

            else if (opcode == 2)
            {
                _registerB = GetComboOperant(operant) % 8;
            }
            else if (opcode == 3)
            {
                if (_registerA != 0)
                {
                    instructionPointer = operant;
                    continue;
                }
            }
            else if (opcode == 4)
            {
                _registerB = _registerB ^ _registerC;
            }
            else if (opcode == 5)
            {
                result.Add(GetComboOperant(operant) % 8);
            }
            else if (opcode == 6)
            {
                _registerB = (int)(_registerA / Math.Pow(2, GetComboOperant(operant)));
            }
            else if (opcode == 7)
            {
                _registerC = (int)(_registerA / Math.Pow(2, GetComboOperant(operant)));
            }

            instructionPointer += 2;
        }

        Console.WriteLine(String.Join(",", result));
    }

    private int GetComboOperant(int operant)
    {
        return operant switch
        {
            0 => operant,
            1 => operant,
            2 => operant,
            3 => operant,
            4 => _registerA,
            5 => _registerB,
            6 => _registerC,
            _ => throw new Exception("Invalid operant")
        };
    }
}