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

//day.BenchMark();

"DONE".Dump();
}


    [ShortRunJob]
    public class Day03 : AdventOfCode
    {
    	public override int Year => 2024;
    
    	public override int Day => 3;
    
    	public override object SolveA(string[] lines)
    	{
    		var input = string.Join("", Regex.Split(string.Join("", lines), @"do\(\)")		
    		.ToArray());
    		
    		return CalcMul(input);
    	}
    
    	public override object SolveB(string[] lines)
    	{
    		var input = string.Join("", Regex.Split(string.Join("", lines), @"do\(\)")
    		.Select(z => Regex.Split(z, @"don't\(\)")[0])
    		.ToArray());
    
    		return CalcMul(input);
    	}
    
    	long CalcMul(string input)
    	{
    		var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
    		long sum = 0;
    		foreach (Match match in matches)
    		{
    			var i1 = int.Parse(match.Groups[1].Value);
    			var i2 = int.Parse(match.Groups[2].Value);
    			sum += i1 * i2;
    		}
    
    		return sum.Dump();
    	}
    }
