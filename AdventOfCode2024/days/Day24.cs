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

        var otherwires = new SortedDictionary<string, Wire>();
        var othergates = new List<Gate>();

        // read all otherwires and othergates
        foreach (var line in Input)
            if (line.Contains("->"))
            {
                var elements = line.Split(' ');
                addWire(elements[0]);
                addWire(elements[2]);
                addWire(elements[4]);

                othergates.Add(new Gate(otherwires[elements[0]], otherwires[elements[2]], otherwires[elements[4]],
                    elements[1]));
            }

        var suspiciousothergates = new List<Gate>();
        var outputotherwires = otherwires.Values.Select(w => w).Where(w => w.name.StartsWith('z')).ToList();
        foreach (var gate in othergates)
        {
            // starting othergates should be followed by OR if AND, and by AND if XOR, except for the first one
            if ((gate.inputs[0].name.StartsWith('x') || gate.inputs[1].name.StartsWith('x')) &&
                (gate.inputs[0].name.StartsWith('y') || gate.inputs[1].name.StartsWith('y')) &&
                !gate.inputs[0].name.Contains("00") && !gate.inputs[1].name.Contains("00"))
                foreach (var secondGate in othergates)
                    if (gate.output == secondGate.inputs[0] || gate.output == secondGate.inputs[1])
                        if ((gate.op.Equals("AND") && secondGate.op.Equals("AND")) ||
                            (gate.op.Equals("XOR") && secondGate.op.Equals("OR")))
                            suspiciousothergates.Add(gate);

            // othergates in the middle should not have XOR operators
            if (!gate.inputs[0].name.StartsWith('x') && !gate.inputs[1].name.StartsWith('x') &&
                !gate.inputs[0].name.StartsWith('y') && !gate.inputs[1].name.StartsWith('y') &&
                !gate.output.name.StartsWith('z') && gate.op.Equals("XOR"))
                suspiciousothergates.Add(gate);

            // othergates at the end should always have XOR operators, except for the last one
            if (outputotherwires.Contains(gate.output) && !gate.output.name.Equals($"z{outputotherwires.Count - 1}") &&
                !gate.op.Equals("XOR"))
                suspiciousothergates.Add(gate);
        }

        var answer = string.Empty;
        foreach (var sGate in suspiciousothergates.OrderBy(x => x.output.name))
            answer += $"{sGate.output.name},";

        Console.WriteLine(answer[..^1]);

        void addWire(string wireName)
        {
            if (!otherwires.ContainsKey(wireName))
                otherwires.Add(wireName, new Wire(wireName));
        }
    }

    private class Gate(Wire in1, Wire in2, Wire output, string op)
    {
        public readonly Wire[] inputs = [in1, in2];
        public readonly string op = op;
        public readonly Wire output = output;
    }

    private class Wire(string name)
    {
        public readonly string name = name;
    }
}