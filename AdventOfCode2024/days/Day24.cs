namespace AdventOfCode2024.days;

public class Day24() : Day(24)
{
    protected override void Run(bool isPart2 = false)
    {
        var wires = new Dictionary<string, bool>();
        var gates = new List<(string a, string op, string b, string restult)>();

        var current = 0;
        while (Input[current] != "")
        {
            var parts = Input[current].Split(": ");
            wires[parts[0]] = parts[1] == "1";
            current++;
        }

        var wireCopy = new Dictionary<string, bool>(wires);
        current++;
        while (current < Input.Length)
        {
            var parts = Input[current].Split(" ");
            gates.Add((parts[0], parts[1], parts[2], parts[4]));
            current++;
        }

        var gatesCopy = new List<(string a, string op, string b, string restult)>(gates);

        void Reset()
        {
            wires = new Dictionary<string, bool>(wireCopy);
            gates = new List<(string a, string op, string b, string restult)>(gatesCopy);
        }

        void ApplyGates()
        {
            var toSearch = new List<(string a, string op, string b, string restult)>(gates);
            while (toSearch.Count > 0)
            {
                gates = toSearch;
                toSearch = new List<(string a, string op, string b, string restult)>();
                foreach (var gate in gates)
                    if (wires.ContainsKey(gate.a) && wires.ContainsKey(gate.b))
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
                    else
                        toSearch.Add(gate);
            }
        }

        ApplyGates();
        var sorted = wires.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        if (isPart2) goto Part2;

        var result = "";
        for (var i = sorted.Keys.Count - 1; i >= 0; i--)
            if (sorted.Keys.ElementAt(i)[0] == 'z')
                result += sorted[sorted.Keys.ElementAt(i)] ? "1" : "0";
        // Console.WriteLine(result);
        Console.WriteLine(Convert.ToInt64(result, 2));
        return;

        Part2:

        var swapsCount = 2;
         
            var swapped = new List<int>();
            var swappedNames = new List<string>();

            Reset();
            for (var i = 0; i < 2; i++)
            {
                var first = new Random().Next(0, gates.Count);
                var second = new Random().Next(0, gates.Count);

                while (swapped.Contains(first) )
                    first = new Random().Next(0, gates.Count);
                while (swapped.Contains(second))
                    second = new Random().Next(0, gates.Count);

                swapped.Add(first);
                swapped.Add(second);
                swappedNames.Add(gates[first].restult);
                swappedNames.Add(gates[second].restult);
                var temp = gates[first].restult;
                gates[first] = (gates[first].a, gates[first].op, gates[first].b, gates[second].restult);
                gates[second] = (gates[second].a, gates[second].op, gates[second].b, temp);
            }


            ApplyGates();

            sorted = wires.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            result = "";
            for (var i = sorted.Keys.Count - 1; i >= 0; i--)
                if (sorted.Keys.ElementAt(i)[0] == 'x')
                    result += sorted[sorted.Keys.ElementAt(i)] ? "1" : "0";
            var x = Convert.ToInt64(result, 2);
            result = "";
            for (var i = sorted.Keys.Count - 1; i >= 0; i--)
                if (sorted.Keys.ElementAt(i)[0] == 'y')
                    result += sorted[sorted.Keys.ElementAt(i)] ? "1" : "0";
            var y = Convert.ToInt64(result, 2);
            result = "";
            for (var i = sorted.Keys.Count - 1; i >= 0; i--)
                if (sorted.Keys.ElementAt(i)[0] == 'z')
                    result += sorted[sorted.Keys.ElementAt(i)] ? "1" : "0";
            // if (x + y == Convert.ToInt64(result, 2))
            if (Convert.ToInt64(result, 2) == 40)
            {
                swappedNames.Sort();
                Console.WriteLine(string.Join(",", swappedNames));
                return;
            }
        
    }
}