namespace AdventOfCode2024.days;

public class Day24() : Day(24)
{
    protected override void Run(bool isPart2 = false)
    {
        if(isPart2) return;
        var wires = new Dictionary<string, bool> ();

        var gates = new List<(string a, string op, string b, string restult)>();
        
        var current = 0;
        while (Input[current] != "")
        {
            var parts = Input[current].Split(": ");
            wires[parts[0]] = parts[1] == "1";
            current++;
        }
        current++;
        while (current < Input.Length)
        {
            var parts = Input[current].Split(" ");
                gates.Add((parts[0], parts[1], parts[2], parts[4]));
            current++;
        }
        
        
        var toSearch = new List<(string a, string op, string b, string restult)>(gates);
        while (toSearch.Count > 0)
        {
            gates = toSearch;
            toSearch = new List<(string a, string op, string b, string restult)>();
            foreach (var gate in gates)
            {
                if (wires.ContainsKey(gate.a) && wires.ContainsKey(gate.b))
                {
                    switch (gate.op)
                    {
                        case "AND":
                            wires[gate.restult] = wires[gate.a] && wires[gate.b];
                            break;
                        case "OR":
                            wires[gate.restult] = wires[gate.a] || wires[gate.b];
                            break;
                        case "XOR":
                            wires[gate.restult] = wires[gate.a] ^ wires[gate.b];
                            break;
                    }

                    // Console.WriteLine(
                        // $"{gate.a}(w:{wires[gate.a]}) {gate.op} {gate.b}(w:{wires[gate.b]}) = {wires[gate.restult]}");
                }
                else
                {
                    toSearch.Add(gate);
                }
            }
        }
        var sorted = wires.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        var result = "";
        for (var i = sorted.Keys.Count - 1; i >= 0; i--)
        {
            if (sorted.Keys.ElementAt(i)[0] == 'z')
            result += sorted[sorted.Keys.ElementAt(i)] ? "1" : "0";
        }
        // Console.WriteLine(result);
        Console.WriteLine(Convert.ToInt64(result, 2));

    }
}