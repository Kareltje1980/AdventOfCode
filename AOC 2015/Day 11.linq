<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day11 day = new Day11();
void Main()
{
	//var testResult = RunTests();  // Call RunTests() or press Alt+Shift+T to initiate testing.
	//if (testResult.Any(z => z.FailureInfo != null))
	//	return;
	//					
	//day.SolveA();
	//day.SolveB();
	
	day.BenchMark();
	
	//vzbxxyzz
	//vzbxxxyz
	
	
	"DONE".Dump();
}
	

[ShortRunJob]
public class Day11 : AdventOfCode
{
	public override int Year => 2015;

	public override int Day => 11;

	public override object SolveA(string[] lines)
	{
		var input = lines[0].ToCharArray();

		while (true)
		{			
			if (Solve(new string(input))) return new string(input).Dump();
			Next(input);									
		}		
	}

	public static void Next(char[] input)
	{
		bool changed = true;
		int p = input.Length -1;
		while (changed)
		{
			changed = false;
			input[p]++;
			if (input[p] > 'z')
			{
				input[p] = 'a';
				p--;
				changed = true;
			}
		}
	}

	public override object SolveB(string[] lines)
	{
		var input = SolveA().ToString().Dump().ToCharArray();
		while (true)
		{			
			Next(input);
			if (Solve(new string(input))) return new string(input).Dump();
		}
	}

	static public bool Solve(string s)
	{
		//check rule 2
		for(int i = 0; i < s.Length; i++)
			if(IsRule2(s[i]) == false) return false;
			
		//check rule 1
		if(IsRule1(s) == false) return false;
		
		return IsRule3(s);		
	}
	
	public static bool IsRule3(string s)
	{
		//Passwords must contain at least two different, non - overlapping pairs of letters, like aa, bb, or zz.
		char? c = null;
		for(int i = 0; i < s.Length -1; i++)
		{
			if(s[i] == s[i+1] && c == null) c = s[i];
			else if(s[i] == s[i+1] && c != null && c != s[i]) return true;
		}

		return false;
	}
	
	public static bool IsRule2(char c)
	{
		//Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
		if(c == 'i') return false;
		if(c == 'o') return false;
		if(c == 'l') return false;
		return true;
	}
	
	public static bool IsRule1(string s)
	{
		//Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
		for (int i = 0; i < s.Length - 2; i++)
		{	
		
			var a = s[i + 0] + 2;
			var b = s[i + 1] + 1;
			var c = s[i + 2] + 0;			
		
			if(b <= 'z' && a == b && b == c) return true;
		}
		return false;
	}
}

[Theory]
[InlineData("abc", true)]
void TestRule1(string s, bool exp) => Assert.Equal(exp, Day11.IsRule1(s));

[Fact]
void T() => Assert.True(Day11.Solve("vzbxxyzz"));




