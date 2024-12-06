<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day05 day = new Day05();
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
public class Day05 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 5;

	public override object SolveA(string[] lines) => lines.Where(z=> CheckLine(z)).Count();
	public override object SolveB(string[] lines) => lines.Where(z=> CheckLine2(z)).Count();

	static HashSet<char> vowels = "aeiou".ToHashSet();
	static HashSet<string> iligal = new HashSet<string>() { "ab", "cd", "pq", "xy" };

	public static bool CheckLine(string line)
	{		
		int vCount = vowels.Contains(line.Last()) ? 1 : 0;		
		
		bool seenDouble = false;
		for (int i = 0; i < line.Length - 1; i++)
		{			
			if (iligal.Contains(line.Substring(i, 2))) return false;
			if (line[i] == line[i + 1]) seenDouble = true;
			if(vowels.Contains(line[i])) vCount++;
		}
		
		if(vCount < 3) return false;
		return seenDouble;
	}

	public static bool CheckLine2(string line)
	{
		bool p1 = false;
		for (int i = 0; i < line.Length - 2; i++)
		{
			var s = line.Substring(i, 2);
			if (line.IndexOf(s, i + 2) != -1)
			{
				p1 = true;
				break;
			}
		}
		if(p1 == false) return false;

		for (int i = 0; i < line.Length - 2; i++)
		{			
			if (line[i] == line[i+2])
			{
				return true;
			}
		}
		
		return false;
	}
}

[Theory]
[InlineData("ugknbfddgicrmopn")]
[InlineData("aaa")]
[InlineData("aeiouaeiouaeiouu")]
void ShouldBeNice(string s) => Assert.True(Day05.CheckLine(s));

[Theory]
[InlineData("jchzalrnumimnmhp")]
[InlineData("haegwjzuvuyypxyu")]
[InlineData("dvszwmarrgswjxmb")]
void ShouldBeNaughty(string s) => Assert.False(Day05.CheckLine(s));


[Theory]
[InlineData("qjhvhtzxzqqjkmpb")]
[InlineData("xxyxx")]
[InlineData("aabcbefgaa")]
[InlineData("xyxybeb")]
void ShouldBeNice2(string s) => Assert.True(Day05.CheckLine2(s));

[Theory]
[InlineData("uurcxstgmygtbstg")]
[InlineData("ieodomkazucvgmuy")]
void ShouldBeNaughty2(string s) => Assert.False(Day05.CheckLine2(s));






