<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize-


static Day06 day = new Day06();
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
public class Day06 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 6;

	public override object SolveA(string[] lines)
	{	
		var arr = new int [1000,1000];		
		foreach (var line in Lines)
		{
			ChangeLigths(line, arr, true);
		}
		
		return CountLights(arr);
	}

	public override object SolveB(string[] lines)
	{
		var arr = new int[1000, 1000];
		foreach (var line in Lines)
		{
			ChangeLigths(line, arr, false);
		}

		return CountLights(arr);
	}

	public static int CountLights(int[,] arr)
	{
		var result = 0;
		for (int x = 0; x < 1000; x++)
		{
			for (int y = 0; y < 1000; y++)
				result += arr[x, y];
		}
		return result;
	}

	public static void ChangeLigths(string line, int[,] arr, bool first)
	{
		var match = Regex.Match(line, @"(\w+) (\d+),(\d+) through (\d+),(\d+)");
		if (match.Success)
		{
			var effect = match.Groups[1].Value;
			var xS = int.Parse(match.Groups[2].Value);
			var yS = int.Parse(match.Groups[3].Value);
			var xE = int.Parse(match.Groups[4].Value);
			var yE = int.Parse(match.Groups[5].Value);

			for (int x = xS; x <= xE; x++)
			{
				for (int y = yS; y <= yE; y++)
				{
					if (first)
					{
						switch (effect)
						{
							case "on": arr[x, y] = 1; break;
							case "off": arr[x, y] = 0; break;
							case "toggle": arr[x, y] = (arr[x, y] == 1 ? 0 : 1); break;
						}
					}
					else
					{
						switch (effect)
						{
							case "on": arr[x, y] += 1; break;
							case "off": arr[x, y] = Math.Max(0, arr[x, y] - 1); break;
							case "toggle": arr[x, y] += 2; break;
						}
					}
				}
			}
		}
	}


}

[Fact]
void TestA1()
{
	var arr = new int[1000,1000];
	Day06.ChangeLigths("turn on 0,0 through 999,999", arr, true);
	Assert.Equal(1000000, Day06.CountLights(arr));	
}

[Fact]
void TestA2()
{
	var arr = new int[1000, 1000];
	Day06.ChangeLigths("toggle 0,0 through 999,0", arr, true);
	Assert.Equal(1000, Day06.CountLights(arr));
}

[Fact]
void TestA3()
{
	var arr = new int[1000, 1000];
	Day06.ChangeLigths("turn on 499,499 through 500,500", arr, true);	
	Assert.Equal(4, Day06.CountLights(arr));
}



























