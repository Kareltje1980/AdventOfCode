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
	if(testResult.Any(z => z.FailureInfo != null))	
		return;
	
	
	day.SolveA();
	day.SolveB();
	
	day.BenchMark();		
	
	"DONE".Dump();
}

[ShortRunJob]
public class Day02 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 2;
	
	public override object SolveA(string [] lines)
	{
		return lines.Select(z => z.Split(' ').Select(int.Parse).ToArray())
			.Where(z => Check(z, false))
			.Count();
	}
	
	public override object SolveB(string [] lines)
	{
		return lines.Select(z => z.Split(' ').Select(int.Parse).ToArray())
			.Where(z => Check(z, true))
			.Count();
	}

	public bool Check(int[] data, bool errorAllow)
	{
		int mod = data[0] > data[1] ? 1 : -1;

		for (int i = 0; i < data.Length - 1; i++)
		{
			var delta = (data[i] - data[i + 1]) * mod;

			if (delta < 1 || delta > 3)
			{
				if (errorAllow)
				{
					var d1 = data.ToList();
					d1.RemoveAt(i);
					if (Check(d1.ToArray(), false)) return true;

					d1 = data.ToList();
					d1.RemoveAt(i + 1);
					if (Check(d1.ToArray(), false)) return true;

					//remove the first one that determines the direction
					d1 = data.ToList();
					d1.RemoveAt(0);
					if (Check(d1.ToArray(), false)) return true;
				}
				return false;
			}
		}
		return true;
	}
}

#region data
string[] TestData = """
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""".Split('\n', StringSplitOptions.TrimEntries);
#endregion

[Theory]
[InlineData(new[] { 1, 2, 3})]
[InlineData(new[] { 3, 2, 1})]
void ShouldBeOk(int [] data)
{
	Assert.True(day.Check(data, false));
}

[Theory]
[InlineData(new[] { 1, 2, 6})]
[InlineData(new[] { 1, 3, 2})]
[InlineData(new[] { 3, 1, 1})]
void ShouldBeNoOk(int[] data)
{
	Assert.False(day.Check(data, false));
}


[Fact] void Test_A() => Assert.Equal(2, day.SolveA(TestData));
[Fact] void Test_B() => Assert.Equal(4, day.SolveB(TestData));