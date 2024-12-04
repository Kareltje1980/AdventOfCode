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
	public override int Year => 2015;

	public override int Day => 1;

	public override object SolveA(string[] lines) => Solve(true);	

	public override object SolveB(string[] lines)=> Solve(false);	
	
	private int Solve(bool first)
	{
		var input = this.Lines[0];
		int floor = 0;
		for (int i = 0; i < input.Length; i++)
		{
			var _ = input[i] switch
			{
				'(' => floor++,
				')' => floor--,
				_ => throw new Exception("huh?")
			};

			if (first == false && floor == -1)
				return i + 1;
		}

		return floor;
	}
}




