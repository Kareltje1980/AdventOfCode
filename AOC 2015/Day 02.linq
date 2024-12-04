<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day02 day = new Day02();
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
public class Day02 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 2;

	public override object SolveA(string[] lines) => Solve(true);	

	public override object SolveB(string[] lines)=> Solve(false);	
	
	private int Solve(bool first)
	{
		var data = this.Lines			
			.Select(line => line.Split('x').Select(int.Parse).ToArray())
			.Select(z => new cadeau(z[0], z[1], z[2]))
			.ToArray();

		if(first) return data.Sum(z => z.Sum).DumpTell();
		return data.Sum(z => z.Sum2).DumpTell();

	}
}

public record cadeau(int l, int w, int h)
{
	public int Ribbon => 2 * l * w + 2 * w * h + 2 * h * l;
	public int SmallestSize => new[] { l * w, w * h, h * l }.Min();
	public int Sum => Ribbon + SmallestSize;

	public int[] P2 { get; } = new[] { l, w, h }.OrderBy(z => z).Take(2).ToArray();
	public int Ribbon2 => 2 * P2[0] + 2 * P2[1];
	public int SmallestSize2 => l * w * h;

	public int Sum2 => Ribbon2 + SmallestSize2;
}

#region private::Tests
[Fact] void Test_A1() => Assert.Equal(58, new cadeau(2, 3, 4).Sum);
[Fact] void Test_A2() => Assert.Equal(43, new cadeau(1, 1, 10).Sum);
[Fact] void Test_B1() => Assert.Equal(34, new cadeau(2, 3, 4).Dump().Sum2);
[Fact] void Test_B2() => Assert.Equal(14, new cadeau(1, 1, 10).Dump().Sum2);
#endregion