<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize-


static Day08 day = new Day08();
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
public class Day08 : AdventOfCode
{
	const string HEX = "0123456789ABCDF";
	public override int Year => 2015;

	public override int Day => 8;

	public override object SolveA(string[] lines)
	{
		//lines = new[]{
		//
		//	"\"\"",
		//	"\"abc\"",
		//	"""
		//	"aaa\"aaa"
		//	""", 
		//	"\"\\x27\""
		//};


		var linesLength = lines.Sum(z => z.Length);
		var linesTrimLength = 0;
		foreach (var line in lines)
		{
			if(CalcLineA(line) != CalcLineA2(line))
				line.Dump();
		}
		return linesLength -linesTrimLength;
	}

	public override object SolveB(string[] lines){return 0;}
	
	public static int CalcLineA(string line)
	{
		int c = 0;
		for (int i = 1; i < line.Length-1; i++)
		{
			if(string.IsNullOrWhiteSpace(line[i].ToString())) continue;
			c++;
			//size 2 \\ and \"
			
			if (line[i] == '\\' && (line[i + 1] == '\\' || line[i + 1] == '"'))
			{
				i++;
				continue;
			}
			if (i >= line.Length - 4) continue;
			if (line[i + 0] != '\\') continue;
			if (line[i + 1] != 'x') continue;
			if (HEX.IndexOf(line[i + 2]) == -1) continue;
			if (HEX.IndexOf(line[i + 3]) == -1) continue;

			i += 3;
		}
		return c;
	}
	
	public static int CalcLineA2(string s)
	{
		return Regex.Replace(
			s.Substring(1, s.Length - 2)
				.Replace("\\\"", "\"")
				.Replace("\\\\", "?"),
			@"\\x[0-9a-f]{2}", "?").Length;
	}

}


[Theory]
[InlineData("\"\"", 0)]
[InlineData("\"abc\"", 3)]
[InlineData("""
"aaa\"aaa"
""", 7)]
[InlineData("\"\\x27\"", 1)]
void Test(string s, int exp) => Assert.Equal(exp, Day08.CalcLineA(s));



