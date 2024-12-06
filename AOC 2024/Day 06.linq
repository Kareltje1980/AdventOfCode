<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize-


static Day06 day = new Day06();
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
public class Day06 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 6;

	private char[,] data {get;}	
	
	public Gurd start {get;}
	
	private Gurd[] _Locations = null;
	private Gurd[] Locations {
		get
		{
			if(_Locations == null)
				SolveA();
			return _Locations;
		}
	}
	
	public Day06()
	{		
		var input = this.Lines;

		//input = new string[]{
		//	"....#.....",
		//	".........#",
		//	"..........",
		//	"..#.......",
		//	".......#..",
		//	"..........",
		//	".#..^.....",
		//	"........#.",
		//	"#.........",
		//	"......#...",
		//	}.Dump();
		
		this.data = new char[input.Length, input[0].Length];

		

		var dc = new DumpContainer().Dump();
		for (int x = 0; x < this.data.GetLength(0); x++)
		{
			for (int y = 0; y < this.data.GetLength(1); y++)
			{
				this.data[y, x] = input[y][x];
				
				if (input[y][x] == '^')
				{
					this.start = new(y, x, 0);
				}
			}
		}
	}

	public override object SolveA(string[] lines)
	{
		var init = start.Copy();
		var arr = Gurd.CopyArray(this.data);
		
		var dc = new DumpContainer().Dump();
		//part 1
		List<Gurd> Steps = new();		
		var position = start;
		while (true)
		{
			
			position = position.Move(arr);
			if (position == null)
			{
				break;
			}
			Steps.Add(position);
			dc.UpdateContent(Gurd.Dump(arr));
		}
		//store for B		
		_Locations = Steps			
			.Select(z => new Gurd(z.y, z.x, 0))
			.Distinct().ToArray();
		
		return Steps.Select(z => (z.x, z.y)).Distinct().Count();
	}

	public override object SolveB(string[] lines)
	{
		int c = 0;
		var dc = new DumpContainer().Dump();
		
		Parallel.ForEach(this.Locations.Where(z => !(z.y == start.y && z.x == start.x)), loc => 		
		{			
			//dc.UpdateContent(Gurd.Dump(arr));			
			if(DeadEnd(start, this.data, loc))
				Interlocked.Increment(ref c);
		});		
		return c;
	}

	public static bool DeadEnd(Gurd start, char[,] dataIn, Gurd changeLoc)
	{
		var arr = Gurd.CopyArray(dataIn);
		arr[changeLoc.y, changeLoc.x] = 'O';

		HashSet<Gurd> Steps = new();
		while (true)
		{
			start = start.Move(arr);
			if (start == null)
			{
				return false;
			}
			if (Steps.Contains(start))
			{
				return true;
			}
			Steps.Add(start);
		}
	}
}

public record Gurd(int y, int x, int m)
{
	static (int x, int y)[] moves = new[]
	{
		(+0, -1),
		(+1, +0),
		(+0, +1),
		(-1, 0)
	};

	public Gurd Move(char[,] arr)
	{
		var pM = this.m;
		var pX = 0;
		var pY = 0;
		while (true)
		{
			pX = this.x + moves[pM].x;
			pY = this.y + moves[pM].y;

			//end of array
			if (pX == -1 || pX >= arr.GetLength(0) || pY == -1 || pY >= arr.GetLength(1))
			{
				pM = (++pM % 4);
				return null;
			}

			//get next block
			var nextStep = arr[pY, pX];

			//blocked rotate
			if (nextStep == '#' || nextStep == 'O')
			{
				pM = (++pM % 4);
				continue;
			}

			break;
		}
		arr[pY, pX] = 'X';
		return new Gurd(pY, pX, pM);
	}



	public static char[,] CopyArray(char[,] arr)
	{
		var tmpArr = new char[arr.GetLength(0), arr.GetLength(0)];
		Array.Copy(arr, tmpArr, arr.Length);
		return tmpArr;
	}

	public static object Dump(char[,] arr)
	{
		var sb = new StringBuilder();		
		for (int y = 0; y < arr.GetLength(1); y++)
		{
			for (int x = 0; x < arr.GetLength(0); x++)
			{
				sb.Append(arr[y, x]);
			}
			sb.AppendLine("</br>");
		}
		return Util.RawHtml("""
		<style>
		k0 { font-size: 7pt; display:block; line-height: 1;}
		k1 { color: red; }
		k2 { color: green; }
		</style><code><k0>
		""" + sb.ToString()
			.Replace("#", "<k1>#</k1>")
			.Replace("X", "<k2>+</k2>")
			.Replace(".", "&nbsp;")
		+ "</k0></code>");
	}

	internal Gurd Copy() => new Gurd(this.y, this.x, this.m);
}








