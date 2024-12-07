<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day07 day = new Day07();
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
public class Day07 : AdventOfCode
{
	public enum Op {Mult, Add, Conc}
				
	public override int Year => 2024;

	public override int Day => 7;
	
	private static Op[] firstOps = new [] { Op.Add, Op.Mult};
	private static Op[] secondOps = new [] { Op.Add, Op.Mult, Op.Conc};
	public static long Valid(string line, bool first)
	{
		var split = line.Split(' ', StringSplitOptions.TrimEntries);
		var target = long.Parse(split[0][..^1]);
		var data = split.Skip(1).Select(long.Parse).ToArray();								

		if(Sovle(target, data[0], data, 1, first ? firstOps : secondOps))
			return target;
		return 0;
	}
	
	static bool Sovle(long target, long current, long[] data, int p, Op[] plus)
	{
		foreach (var operation in plus)
		{
			var c1 = operation switch
			{
				Op.Add => current + data[p],
				Op.Mult => current * data[p],
				Op.Conc => long.Parse(current.ToString() + data[p].ToString()),
				_ => throw new NotImplementedException()
			};

			if (c1 == target && data.Length - 1 == p) return true;			
			
			if (c1 <= target && p + 1< data.Length)
			{
				if (Sovle(target, c1, data, p + 1, plus))
					return true;				
			}
		}
		return false;
	}

	public override object SolveA(string[] lines)
	{
		return lines.AsParallel().Select(z => Valid(z, true)).AsParallel().Sum();		
	}

	public override object SolveB(string[] lines)
	{
		return lines.AsParallel().Select(z => Valid(z, false)).Sum();		
	}

	[Benchmark]
	public object NotParallel() => this.Lines.Select(z => Valid(z, false)).Sum();			
}



[Theory]
[InlineData("190: 10 19")]
[InlineData("3267: 81 40 27")]
[InlineData("292: 11 6 16 20")]
[InlineData("434142: 271 3 6 1 89 1")]
void ShouldBeTrue(string s) => Assert.True(Day07.Valid(s, true) > 0);

[Theory]
[InlineData("83: 17 5")]
[InlineData("156: 15 6")]
[InlineData("7290: 6 8 6 15")]
[InlineData("161011: 16 10 13")]
[InlineData("192: 17 8 14")]
[InlineData("21037: 9 7 18 13")]
void ShouldBeFalse(string s) => Assert.Equal(0, Day07.Valid(s, true));

[Fact]
void TestComplete()
{
	var lines = """
		190: 10 19
		3267: 81 40 27
		83: 17 5
		156: 15 6
		7290: 6 8 6 15
		161011: 16 10 13
		192: 17 8 14
		21037: 9 7 18 13
		292: 11 6 16 20
		""".Split('\n', StringSplitOptions.TrimEntries);
		
		var day = new Day07();		
		var result = (long) day.SolveA(lines);
		
		Assert.Equal(3749, result);
}























