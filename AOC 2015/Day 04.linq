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
	public override int Year => 2015;

	public override int Day => 4;

	public override object SolveA(string[] lines) => Solve(true, lines[0]);

	public override object SolveB(string[] lines) => Solve(false, lines[0]);
	
	
	private object Solve(bool first, string key)
	{		
		using (var provider = new System.Security.Cryptography.MD5CryptoServiceProvider())
		{
			var bytes = Encoding.ASCII.GetBytes(key + "0");
			bytes[bytes.Length - 1]--;
			int last = bytes.Length - 1;
			for (int counter = 0; counter < int.MaxValue; counter++)
			{
				bytes[last]++;

				while (bytes[last] > '9')
				{
					bytes[last] = 0x30; //0				
					last--;
					if (last == key.Length - 1)
					{
						bytes = Encoding.ASCII.GetBytes(key + counter);
						last = bytes.Length - 1;
					}
					else
					{
						bytes[last]++;
					}
				}				
				last = bytes.Length - 1;

				var r = provider.ComputeHash(bytes);

				if (first && r[0] == 0x00 && r[1] == 0x00 && r[2] < 0x10)
					return System.Text.Encoding.Default.GetString(bytes)[key.Length..];

				if (!first && r[0] == 0x00 && r[1] == 0x00 && r[2] == 0x00)
					return System.Text.Encoding.Default.GetString(bytes)[key.Length..];
			}
			throw new NotImplementedException();
		}
	}
}




