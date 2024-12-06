<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
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
	public override int Year => 2015;

	public override int Day => 7;

	private Dictionary<string, string> Dictionary {get;}
	
	public Day07()
	{
		this.Dictionary = this.Lines.Select(z => z.Split(' '))
			.ToDictionary(z => z.Last(), z => string.Join(" ", z));
	}

	public override object SolveA(string[] lines)
	{
		var input = this.Dictionary.ToDictionary(z=> z.Key, z=> z.Value);
		return Line("a", input);		
	}

	public override object SolveB(string[] lines)
	{
		var input = this.Dictionary.ToDictionary(z => z.Key, z => z.Value);
		input["b"] = SolveA(Array.Empty<string>()).ToString();	
		return Line("a", input);
	}

	public static ushort Line(string source, Dictionary<string, string> dic)
	{
		//find the line that defines
		var line = dic[source];
		
		var steps = line.Split(' ');
		
		ushort? result = null;

		//ASSIGN
		if (steps.Length == 1)
			result = ushort.Parse(steps[0]);

		else if (steps.Length == 3)
		{
			if (ushort.TryParse(steps[0], out var v))
				result = v;
			else
				result = Line(steps[0], dic);
		}

		//NOT
		else if (steps.Length == 4)
		{
			if (ushort.TryParse(steps[1], out var v) == false)
				result = (ushort)~Line(steps[1], dic);
			else
				result = (ushort)~v;
		}

		else
		{
			var v1 = ushort.TryParse(steps[0], out var r1) ? r1 : Line(steps[0], dic);
			var v2 = ushort.TryParse(steps[2], out var r2) ? r2 : Line(steps[2], dic);

			switch (steps[1])
			{
				case "AND": result = (ushort)(v1 & v2); break;
				case "OR": result = (ushort)(v1 | v2); break;
				case "LSHIFT": result = (ushort)(v1 << v2); break;
				case "RSHIFT": result = (ushort)(v1 >> v2); break;
			}
		}


		if (result.HasValue == false)
			throw new NotImplementedException();
		dic[source] = result.ToString();
		//$"{source} <- {result}".Dump();
		return (ushort)result;
	}

}




