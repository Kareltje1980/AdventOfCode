<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day04 day = new Day04();
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
public class Day04 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 4;

	private char GetP(int x, int y)
	{
		if(x >= this.Lines.Length) return '.';
		if(x < 0) return '.';
		if(y >= this.Lines[0].Length) return '.';
		if(y < 0) return '.';
		return this.Lines[x][y];
	}

	public override object SolveA(string[] lines)
	{
		var result = 0;
		
		for (int x = 0; x < this.Lines.Length; x++)
		{
			for (int y = 0; y < this.Lines[0].Length; y++)
			{
				var c1 = GetP(x, y);
				if (c1 != 'X' && c1 != 'S') continue;

				if (c1 == 'X' && GetP(x + 1, y) == 'M' && GetP(x + 2, y) == 'A' && GetP(x + 3, y) == 'S') result++;
				if (c1 == 'X' && GetP(x, y + 1) == 'M' && GetP(x, y + 2) == 'A' && GetP(x, y + 3) == 'S') result++;
				if (c1 == 'X' && GetP(x + 1, y + 1) == 'M' && GetP(x + 2, y + 2) == 'A' && GetP(x + 3, y + 3) == 'S') result++;
				if (c1 == 'X' && GetP(x + 1, y - 1) == 'M' && GetP(x + 2, y - 2) == 'A' && GetP(x + 3, y - 3) == 'S') result++;

				if (c1 == 'S' && GetP(x + 1, y) == 'A' && GetP(x + 2, y) == 'M' && GetP(x + 3, y) == 'X') result++;
				if (c1 == 'S' && GetP(x, y + 1) == 'A' && GetP(x, y + 2) == 'M' && GetP(x, y + 3) == 'X') result++;
				if (c1 == 'S' && GetP(x + 1, y + 1) == 'A' && GetP(x + 2, y + 2) == 'M' && GetP(x + 3, y + 3) == 'X') result++;
				if (c1 == 'S' && GetP(x + 1, y - 1) == 'A' && GetP(x + 2, y - 2) == 'M' && GetP(x + 3, y - 3) == 'X') result++;
			}
		}
		return result;
	}


	[Benchmark]
	public object InitialSolve()
	{					
		var result = 0;
		for(int x = 0; x < this.Lines.Length; x++)
		{
			for(int y = 0; y < this.Lines[0].Length; y++)
			{
				if(GetP(x, y) == 'X' && GetP(x + 1, y) == 'M' && GetP(x + 2, y) == 'A' && GetP(x + 3, y) == 'S') result++;
				if(GetP(x, y) == 'X' && GetP(x, y + 1) == 'M' && GetP(x, y + 2) == 'A' && GetP(x, y+3) == 'S') result++;
				if(GetP(x, y) == 'X' && GetP(x + 1, y + 1) == 'M' && GetP(x + 2, y + 2) == 'A' && GetP(x + 3, y + 3) == 'S') result++;
				if(GetP(x, y) == 'X' && GetP(x + 1, y - 1) == 'M' && GetP(x + 2, y - 2) == 'A' && GetP(x + 3, y - 3) == 'S') result++;

				if (GetP(x, y) == 'S' && GetP(x + 1, y) == 'A' && GetP(x + 2, y) == 'M' && GetP(x + 3, y) == 'X') result++;
				if (GetP(x, y) == 'S' && GetP(x, y + 1) == 'A' && GetP(x, y + 2) == 'M' && GetP(x, y + 3) == 'X') result++;
				if (GetP(x, y) == 'S' && GetP(x + 1, y + 1) == 'A' && GetP(x + 2, y + 2) == 'M' && GetP(x + 3, y + 3) == 'X') result++;
				if (GetP(x, y) == 'S' && GetP(x + 1, y - 1) == 'A' && GetP(x + 2, y - 2) == 'M' && GetP(x + 3, y - 3) == 'X') result++;
			}
		}
		return result;
	}

	public override object SolveB(string[] lines)
	{
		var result = 0;

		for (int x = 0; x < lines.Length; x++)
		{
			for (int y = 0; y < lines[0].Length; y++)
			{
				if (GetP(x, y) == 'A')
				{
					var c1 = GetP(x-1, y -1);
					var c2 = GetP(x+1, y +1);
					
					if(!((c1 == 'M' && c2 == 'S') || (c1 == 'S' && c2 == 'M'))) continue;

					c1 = GetP(x - 1, y + 1);
					c2 = GetP(x + 1, y - 1);

					if (!((c1 == 'M' && c2 == 'S') || (c1 == 'S' && c2 == 'M'))) continue;

					result++;
				}
			}
		}
		return result;

	}
}


