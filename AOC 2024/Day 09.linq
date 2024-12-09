<Query Kind="Program">
  <NuGetReference Version="0.13.8">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Xunit</Namespace>
</Query>

#load "BenchmarkDotNet"
#load "xunit"
#load "..\AOC v3"
#LINQPad optimize+


static Day09 day = new Day09();
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
public class Day09 : AdventOfCode
{
	public override int Year => 2024;

	public override int Day => 9;

	public override object SolveA(string[] lines)
	{
		var fs = new FillyStem(lines[0]);
		return fs.OptA();
	}
	public override object SolveB(string[] lines)
	{
		var fs = new FillyStem(lines[0]);
		return fs.OptB();
	}	
}

[Fact]
void TestA()
{
	var fs = new FillyStem("2333133121414131402");
	Assert.Equal(1928, fs.OptA());
}

[Fact]
void TestB()
{
	var fs = new FillyStem("2333133121414131402");
	Assert.Equal(2858, fs.OptB());
}


class FillyStem
{
	DumpContainer dc = null;//new DumpContainer().Dump();
	private List<int?> data = new ();
	
	public (int i, int? d, string, string)[] ov => data.Select(
	(x, i) => (i, x, 
	 i >= indexEmpty && i < indexEmpty + sizeEmpty ? "empty" : "",
	 i >= indexFile && i < indexFile + sizeFile ? "file" : ""		
	)).ToArray();
	public FillyStem(string inpu)
	{		
		for(int i = 0; i < inpu.Length; i++)
		{
			if (i % 2 == 0)
			{
				var id = i / 2;
				int block = inpu[i] - 48;
				while (block-- > 0)
				{
					data.Add(id);
				}
			}
			else
			{
				int block = inpu[i] - 48;
				while (block-- > 0)
				{
					data.Add(null);
				}
			}			
		}		
		//one extra as it makes it easier
		data.Add(null);
		dc?.UpdateContent(this);
	}

	public long OptA()
	{
		(indexEmpty, sizeEmpty) = GetNextEmpty(0);
		(indexFile, sizeFile) = GetNextFile(data.Count - 1);

		while (indexEmpty < indexFile)
		{
			dc?.UpdateContent(this);
			data[indexEmpty] = data[indexFile + sizeFile - 1];
			data[indexFile + sizeFile - 1] = null;
			dc?.UpdateContent(this);
			
			
			if(--sizeFile == 0)
				(indexFile, sizeFile) = GetNextFile(indexFile -1);
			
			(indexEmpty, sizeEmpty) = GetNextEmpty(indexEmpty);
		}

		return this.data.Select((z, i) => z != null ? (long)z * i : 0).Sum().Dump();
	}
	
	int indexEmpty;
	int indexFile;
	int sizeEmpty;
	int sizeFile;
	
	public long OptB()
	{	
		(indexEmpty, sizeEmpty) = GetNextEmpty(0);
		(indexFile, sizeFile) = GetNextFile(data.Count -1);

		while(indexEmpty < indexFile)
		{
			//check if there is room in the emptyes			
			while (sizeFile > sizeEmpty && indexFile > indexEmpty)
			{
				(indexEmpty, sizeEmpty) = GetNextEmpty(indexEmpty + sizeEmpty);
				dc?.UpdateContent(this);
			}

			if (indexEmpty + sizeEmpty > indexFile + sizeFile)
			{
				(indexEmpty, sizeEmpty) = GetNextEmpty(0);
				(indexFile, sizeFile) = GetNextFile(indexFile-1);	
				dc?.UpdateContent(this);
				continue;
			}

			for(int s = 0; s < sizeFile; s++)
			{
				dc?.UpdateContent(this);		
				data[indexEmpty + s] = data[indexFile + s];
				data[indexFile + s] = null;
				dc?.UpdateContent(this);								
			}
									
			(indexEmpty, sizeEmpty) = GetNextEmpty(0);
			(indexFile, sizeFile) = GetNextFile(indexFile-1);
						
			dc?.UpdateContent(this);
		}
		
		return this.data.Select((z, i) => z != null ? (long) z * i : 0).Sum().Dump();
	}

	private (int index, int size) GetNextFile(int i)
	{		
		while (data[i] == null)	
			i--;
				
		var index = i;
		while (i >= 0 && data[i] == data[index])
			i--;
		
		//data.Slice(i+1, index - i).Dump();

		return (i+1, index - i);
	}

	private (int index, int size) GetNextEmpty(int i)
	{		
		while (data[i] != null)	
			i++;
				
		var index = i;
		while (i < data.Count && data[i] == null)
			i++;

		return (index, i - index);
	}

	public string D => string.Join("", data.Select(z => z?.ToString() ?? "."));
}


