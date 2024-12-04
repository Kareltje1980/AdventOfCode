<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day03 day = new Day03();
void Main()
{
	var testResult = RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
	if (testResult.Any(z => z.FailureInfo != null))
		return;
	
	
	day.SolveA();
	day.SolveB();
	
	day.BenchMark();
	
	"DONE".Dump();
}


[ShortRunJob]
public class Day03 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 3;

	private Dictionary<(int x, int y), int> dic = new();

	public override object SolveA(string[] lines)
	{
		dic.Clear();
		var input = string.Join("", lines[0]);
		Go(input);
		return dic.Count.Dump();
	}

	public override object SolveB(string[] lines)
	{
		dic.Clear();
		var input = lines[0];
		var i1 = string.Join("", Enumerable.Range(0, input.Length).Where(z => z % 2 == 0).Select(z => input[z]));
		var i2 = string.Join("", Enumerable.Range(0, input.Length).Where(z => z % 2 == 1).Select(z => input[z]));
		Go(i1);
		Go(i2);
		return dic.Count;
	}

	void Go(string input)
	{
		int x = 0;
		int y = 0;
		//initial visit.	
		if (dic.Count == 0)
			dic.Add((x, y), 1);
		for (int i = 0; i < input.Length; i++)
		{
			switch (input[i])
			{
				case '>': x++; break;
				case '<': x--; break;
				case 'v': y++; break;
				case '^': y--; break;
			}
			if (dic.ContainsKey((x, y)) == false) dic.Add((x, y), 0);
			dic[(x, y)]++;
		}
	}
}