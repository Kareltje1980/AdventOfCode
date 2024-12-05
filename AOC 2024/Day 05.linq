<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize--


static Day05 day = new Day05();
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
public class Day05 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 5;
	
	(int first, int second)[] data {get;}
	string[] books {get;}
	
	public Day05()
	{
		data = this.Lines.TakeWhile(z => !string.IsNullOrEmpty(z)).Select(z => z.Split('|').Select(int.Parse).ToArray())	
			.Select(z => (first: z[0], last: z[1])).ToArray().Dump();
		
		books = this.Lines[(data.Length+1)..].Dump();
	}

	public override object SolveA(string[] lines) => Solve(true);
	

	public override object SolveB(string[] lines) => Solve(false);
	
	private int Solve(bool first)
	{
		int sum = 0;
		foreach (var line in books)
		{
			var r = Calc(line, data, first);
			sum += r == -1 ? 0 : r;
		}
		return sum;
	}

	public static int Sort(List<int> numbers, (int first, int second)[] data)
	{
		bool changed = true;
		while (changed)
		{
			changed = false;

			foreach (var d in data)
			{
				int i1 = numbers.IndexOf(d.first);
				int i2 = numbers.IndexOf(d.second);

				if (i1 != -1 && i2 != -1 && i1 > i2)
				{
					changed = true;
					numbers[i1] = d.second;
					numbers[i2] = d.first;
				}
			}
		}

		return numbers[numbers.Count / 2];
	}

	public static int Calc(string s, (int first, int second)[] data, bool first)
	{
		var numbers = s.Split(',').Select(int.Parse).ToList();

		for (int i = 0; i < numbers.Count - 1; i++)
		{
			var i1 = numbers[i];
			var i2 = numbers[i + 1];
			foreach (var d in data)
			{
				if (d.first == i2 && d.second == i1)
				{
					if (first)
						return -1;
					return Sort(numbers, data);
				}
			}
		}

		if (first)
			return numbers[numbers.Count / 2];
		return 0;

	}
}


[Theory]
[InlineData("75,47,61,53,29", 61)]
[InlineData("97,61,53,29,13", 53)]
[InlineData("75,29,13", 29)]
[InlineData("75,97,47,61,53", -1)]
[InlineData("61,13,29", -1)]
[InlineData("97,13,75,29,47", -1)]
void Check(string s, int e)
{
	(int first, int second)[] data = """
	47|53
	97|13
	97|61
	97|47
	75|29
	61|13
	75|53
	29|13
	97|29
	53|29
	61|53
	97|53
	61|29
	47|13
	75|47
	97|75
	47|61
	75|61
	47|29
	75|13
	53|13
	""".Split('\n', StringSplitOptions.TrimEntries)
	.Select(z => z.Split('|').Select(int.Parse).ToArray())
	.Select(z => (first: z[0], last: z[1])).ToArray();

	var r = Day05.Calc(s, data, true);
	Assert.Equal(e, r);
}