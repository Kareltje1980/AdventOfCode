<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day10 day = new Day10();
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
public class Day10 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 10;

	public override object SolveA(string[] lines) 
	{
		var i = lines[0];
		for (int x = 0; x < 40; x++)
		{
			i = Convert(i);			
		}
		
		return i.Length;
	}

	public override object SolveB(string[] lines)
	{
		var i = lines[0];
		for (int x = 0; x < 50; x++)
		{
			i = Convert(i);
		}

		return i.Length;
	}

	public static string Convert(string s)
	{
		var sb = new StringBuilder();
		for(int i = 0; i < s.Length; i++)
		{
			var current = s[i];
			var count = 1;
			while (i + 1 < s.Length && s[i + 1] == current)
			{
				i++;
				count++;
			}
			sb.Append(count);
			sb.Append(current);						
		}
		return sb.ToString();
	}
}


[Fact] void TestInput() 
{
	var a = "1";
	a = Day10.Convert(a);
	Assert.Equal("11", a);

	a = Day10.Convert(a);
	Assert.Equal("21", a);

	a = Day10.Convert(a);
	Assert.Equal("1211", a);

	a = Day10.Convert(a);
	Assert.Equal("111221", a);

	a = Day10.Convert(a);
	Assert.Equal("312211", a);
}




