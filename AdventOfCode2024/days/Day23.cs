namespace AdventOfCode2024.days;

public class Day23() : Day(23)
{
    protected override void Run(bool isPart2 = false)
    {
        var computers = new Dictionary<string, List<string>>();
        foreach (var line in Input)
        {
            var split = line.Split("-");
            if (!computers.ContainsKey(split[0])) computers.Add(split[0], new List<string>());
            computers[split[0]].Add(split[1]);

            if (!computers.ContainsKey(split[1])) computers.Add(split[1], new List<string>());
            computers[split[1]].Add(split[0]);
        }

        if (isPart2) goto Part2;
        var pairs = new List<List<string>>();
        foreach (var key in computers.Keys)
        {
            var connections = computers[key];
            foreach (var connection in connections)
            foreach (var se in computers[connection])
                if (computers[se].Contains(key))
                {
                    if (!key.StartsWith("t") && !connection.StartsWith("t") && !se.StartsWith("t")) continue;
                    if (pairs.Any(p => p.Contains(key) && p.Contains(connection) && p.Contains(se))) continue;
                    pairs.Add([key, connection, se]);
                }
        }

        Console.WriteLine(pairs.Count);
        return;


        Part2:

        var parties = new List<HashSet<string>>();
        HashSet<string> FindConnections(string computer)
        {
            var connections = new HashSet<string>();
            foreach (var computerConnection in computers[computer])
            foreach (var secondGradeConnection in computers[computerConnection])
                if (computers[computer].Contains(secondGradeConnection))
                {
                    connections.Add(computerConnection);
                    connections.Add(secondGradeConnection);
                }

            return connections;
        }

        void RemoveComputersThatAreNotConnectedToAllOthers(HashSet<string> computerNames)
        {
            if (computerNames.Count == 0) return;
    
            var toRemove = new List<string>();
            foreach (var name1 in computerNames)
            foreach (var name2 in computerNames)
                if (name1 != name2)
                    if (!computers[name1].Contains(name2))
                        toRemove.Add(name1);

            foreach (var name in toRemove)
                computerNames.Remove(name);
        }

       foreach (var computersKey in computers.Keys)
       {
              var mutualConnections = FindConnections(computersKey);
              RemoveComputersThatAreNotConnectedToAllOthers(mutualConnections);
              if (mutualConnections.Count == 0) continue;
              var party = new HashSet<string> {computersKey};
              party.UnionWith(mutualConnections);
              parties.Add(party);
       }



       var longest = "";
        foreach (var party in parties)
        {
            var partyList = party.ToList();
            partyList.Sort();
            var partyString = string.Join(",", partyList);
            if (partyString.Length > longest.Length)
            {
                longest = partyString;
            }
        }
        
        Console.WriteLine(longest);
    }
    
   
}