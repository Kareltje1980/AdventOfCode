<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


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
	public override int Year => 2024;

	public override int Day => 8;

	public override object SolveA(string[] lines) => Solve(lines, 1);
	public override object SolveB(string[] lines) => Solve(lines, int.MaxValue);

	public static int Solve(string[] lines, int depth)
	{		
		int yMax = lines.Length;
		int xMax = lines[0].Length;

		var arr = new char?[yMax, xMax];

		//create array
		var dic = new Dictionary<char, List<Antanne>>();
		for(int x = 0; x < lines.Length; x++)
		{
			for(int y = 0; y < lines.Length; y++)
			{
				arr[y, x] = lines[y][x];
				if (lines[y][x] != '.')
				{
					var r = new Antanne(y, x, lines[y][x]);					 
					if(dic.ContainsKey(r.c) == false)
						dic.Add(r.c, new ());
					dic[r.c].Add(r);
				}
			}
		}
		
		//visualize and 
		var dc = new DumpContainer().Dump();
		var hs = new HashSet<Antanne>();

		foreach (var c in dic)
		{
			//dc.UpdateContent(Antanne.Dump(arr, c.Key));		
			
			for(int a = 0; a < c.Value.Count; a++)
			for(int b = 0; b < c.Value.Count; b++)
			{
				if(a == b) continue;
				
				foreach (var r in c.Value[a].GetAntiNodes(c.Value[b], arr).Take(depth))
				{					
					arr[r.y, r.x] = r.c;
					//dc.UpdateContent(Antanne.Dump(arr, c.Key));
					hs.Add(r);

				}
			}
		}

		int count= 0; 
		for (int y = 0; y < arr.GetLength(1); y++)
		{
			for (int x = 0; x < arr.GetLength(0); x++)
			{
				if(arr[y,x].Value == '.') continue;
				count ++;
				
			}			
		}

		//wtf still not sure why
		if(depth == 1) 
			return hs.Count();		
		return count.Dump();		
	}


}

record Antanne(int y, int x, char c)
{
	
	public IEnumerable<Antanne> GetAntiNodes(Antanne a, char?[,] arr)
	{
		var b = this;
		
		int dX = b.x - a.x;
		int dY = b.y - a.y;

		Antanne r = new Antanne(
			this.y + dY,
			this.x + dX,
			'#'); 

		while(arr.IsInsideArray(r.y, r.x))
		{
			yield return r;
			
			r = new Antanne(
			r.y + dY,
			r.x + dX,
			'#');
		}
	}
	
	
	
	public static object Dump(char?[,] arr, char c)
	{
		var sb = new StringBuilder();
		for (int y = 0; y < arr.GetLength(1); y++)
		{
			for (int x = 0; x < arr.GetLength(0); x++)
			{
				sb.Append(arr[y, x] ?? '.');
			}
			sb.AppendLine("</**>");
		}
		return Util.RawHtml("""
		<style>
		k0 { font-size: 10pt; display:block; line-height: 1;}
		k1 { color: red; }
		k2 { color: green; }
		</style><code><k0>
		""" + sb.ToString()
			.Replace(c.ToString(), $"<k1>{c.ToString()}</k1>")
			//.Replace("X", "<k2>+</k2>")
			//.Replace(".", "&nbsp;")
			.Replace("</**>", "</br>")
		+ "</k0></code>");
	}
}



string[] testInput = """
			............
			........0...
			.....0......
			.......0....
			....0.......
			......A.....
			............
			............
			........A...
			.........A..
			............
			............
		""".Split('\n', StringSplitOptions.TrimEntries);
		
		
[Fact] void TestA() => Assert.Equal(14, Day08.Solve(testInput, 1));		
[Fact] void TestB() => Assert.Equal(34, Day08.Solve(testInput, int.MaxValue));		