<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day09 day = new Day09();
void Main()
{
	var testResult = RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
	if (testResult.Any(z => z.FailureInfo != null))
		return;
	
	
	day.SolveA();
	day.SolveB();
	
	day.BenchMark();
	
	//"DONE".Dump();
}
	

[ShortRunJob]
public class Day09 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 9;

	public override object SolveA(string[] lines)
	{
		var airports = Airport.Create(lines).Dump(0);
		return airports.AsParallel().SelectMany(z => z.Find(new List<string>(), airports.Length, 0)).Min();		
	}
	
	public override object SolveB(string[] lines)
	{
		var airports = Airport.Create(lines).Dump(0);
		return airports.AsParallel().SelectMany(z => z.Find(new List<string>(), airports.Length, 0)).Max();
	}
}

class Airport
{	
	public string Name {get;}
	
	public Dictionary<Airport, int> Connections {get;} = new();	
			
	public Airport(string Name) => this.Name = Name;
				
	public static Airport[] Create(string [] lines)
	{
		Dictionary<string, Airport> dic = new();
		foreach (var line in lines)
		{
			var split = line.Split(' ');
			if (dic.ContainsKey(split[0]) == false) dic.Add(split[0], new Airport(split[0]));
			if (dic.ContainsKey(split[2]) == false) dic.Add(split[2], new Airport(split[2]));
			
			var d = int.Parse(split[4]);
			
			dic[split[0]].Connections.Add(dic[split[2]], d);
			dic[split[2]].Connections.Add(dic[split[0]], d);
		}

		return dic.Values.ToArray();
	}	
	
	public IEnumerable<int> Find(List<string> visited, int needed, int distance)
	{		
		visited.Add(this.Name);
		
		if (visited.Count == needed)
		{
			if(distance == 21)
			{
				visited.Dump();
			}
			yield return distance;
		}
		else
		{
			foreach (var ap in this.Connections)
			{
				if (visited.Contains(ap.Key.Name)) continue;
				
				foreach (var result in ap.Key.Find(visited.ToList(), needed, distance + ap.Value))
					yield return result;
			}
		}
	}			
}





























