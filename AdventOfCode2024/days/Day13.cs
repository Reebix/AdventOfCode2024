namespace AdventOfCode2024.days;

public class Day13() : Day(13)
{
    private struct DayMachine
    {
        public (long, long) ButtonA;
        public (long, long) ButtonB;
        public (long, long) Prize;

        public override string ToString()
        {
            return "{"+$"ButtonA: {ButtonA.Item1}, {ButtonA.Item2}  ButtonB: {ButtonB.Item1}, {ButtonB.Item2}  Prize: {Prize.Item1}, {Prize.Item2}"+"}";
        }
    }
    protected override void Run(bool isPart2 = false)
    {
        if(isPart2) return;
        var machines = new List<DayMachine>();
        for (var i = 0; i < Input.Length; i++)
        {
            var machine = new DayMachine();
            var parts = Input[i].Split(", ");
            machine.ButtonA = (long.Parse(parts[0].Replace("Button A: X+", "")), long.Parse(parts[1].Replace("Y+", "")));
            i++;
            parts = Input[i].Split(", ");
            machine.ButtonB = (long.Parse(parts[0].Replace("Button B: X+", "")), long.Parse(parts[1].Replace("Y+", "")));
            i++;
            parts = Input[i].Split(", ");
            machine.Prize = (long.Parse(parts[0].Replace("Prize: X=", "")), long.Parse(parts[1].Replace("Y=", "")));
            i++;
            machines.Add(machine);
        }

        Console.WriteLine(machines.Sum(TryMachine));
        Console.WriteLine();
        Console.WriteLine(machines.Sum(TryMachineMath));
    }

    //154978722789171 too high
    private long TryMachineMath(DayMachine machine)
    {
        machine.Prize = (machine.Prize.Item1 + 10000000000000, machine.Prize.Item2 + 10000000000000);
     
        var timesB = (machine.Prize.Item2 * machine.ButtonA.Item1 - machine.Prize.Item1 * machine.ButtonA.Item2) / (machine.ButtonB.Item2 * machine.ButtonA.Item1 - machine.ButtonB.Item1 * machine.ButtonA.Item2);
        var timesA = (machine.Prize.Item1 - machine.ButtonB.Item1 * timesB) / machine.ButtonA.Item1;
        
        if(timesA % 1 == 0 && timesB % 1 == 0)
        {
            return (long)timesA * 3 + (long)timesB;
        }
        
        return 0;
    }

    private long TryMachine(DayMachine machine)
    {
        long minimumTokens = 1000;
        (long, long) position = (0, 0);
        for (long i = 0; i < 101; i++)
        {
            var positionCopy = position;
            for (long j = 0; j < 101; j++)
            {
                if(positionCopy == machine.Prize)
                {
                    // Console.WriteLine($"Machine {machine} won at {i}, {j}, with a total of {i*3+j} tokens");
                    minimumTokens = long.Min(minimumTokens, i*3+j);
                }
                positionCopy = (positionCopy.Item1 + machine.ButtonB.Item1, positionCopy.Item2 + machine.ButtonB.Item2);
            }
            position = (position.Item1 + machine.ButtonA.Item1, position.Item2 + machine.ButtonA.Item2);
        }

        return minimumTokens == 1000 ? 0 : minimumTokens;
    }
}