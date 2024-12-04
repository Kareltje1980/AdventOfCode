<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day01 day = new Day01();
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
public class Day01 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 1;

	public override object SolveA(string[] lines)
	{	
		var parsed = lines.Select(z => z.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();		
		var l1 = parsed.Select(z => z[0]).OrderBy(z=>z).ToArray();
		var l2 = parsed.Select(z => z[1]).OrderBy(z=>z).ToArray();
										
		return l1.Zip(l2).Sum(z => Math.Abs(z.First - z.Second));			
	}

	public override object SolveB(string[] lines)
	{
		var parsed = lines.Select(z => z.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
		var dic = parsed.Select(z => z[1]).GroupBy(z=> z).ToDictionary(z=> z.Key, z=> z.Count());
		var result = 0;
		foreach(var i in parsed.Select(z => z[0]))
		{
			if(dic.TryGetValue(i, out var m))
				result += m * i;						
		}
		
		return result;
		
	}
}
