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
        
        List<int> program = new();
        Input[4].Split(" ")[1].Split(",").ToList().ForEach(x => program.Add(int.Parse(x))); 
        
        
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