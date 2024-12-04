<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>Humanizer</NuGetReference>
  <NuGetReference>LiteDB</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>Humanizer</Namespace>
  <Namespace>Humanizer.Localisation.NumberToWords</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>LiteDB</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>BenchmarkDotNet.Configs</Namespace>
  <Namespace>BenchmarkDotNet.Diagnosers</Namespace>
</Query>

#load "BenchmarkDotNet"

void Main()
{
		
	//reset cookie
	//Util.SetPassword(Login.Google.ToString(), "XXXXXXXX");
	
	var aoc = new Aoc202401();			
	aoc.BenchMark();
	//aoc.HttpPostAnswer(Part.A, "225");
}

internal class Aoc202401 : AdventOfCode
{
	public override int Year => 2016;
	public override int Day => 25;
		
	public override object SolveA(string[] lines)
	{
		for(int i = 0;  i < 100; i++)
		{
			i++;
		}
		return "";
	}
		
	public override object SolveB(string[] lines)
	{
		for(int i = 0;  i < 100; i++)
		{
			i++;
		}
		return "";
	}
}



public enum Part { A = 1, B = 2 }
public enum Login { Google, Reddit }

public abstract class AdventOfCode
{				
	public abstract int Year { get; }
	public abstract int Day { get; }
	
	private string _answerA;
	private string _answerB;
	public string AnswerA{
		get => _answerA;
		set
		{
			if(value == this.Storage.ValidAnswerA)
				_answerA = value + " ✔";
			else 
				_answerA = value + " ❌";
			
		}
	}
	public string AnswerB
	{
		get => _answerB;
		set
		{
			if (value == this.Storage.ValidAnswerB)
				_answerB = value + " ✔";
			else
				_answerB = value + " ❌";

		}
	}

	public abstract object SolveA(string[] lines);
	public abstract object SolveB(string[] lines);

	public BenchmarkDotNet.Reports.Summary BenchMark()
	{
		var config = ManualConfig.CreateEmpty()
							.AddValidator(DefaultConfig.Instance.GetValidators().ToArray())
							.AddAnalyser(DefaultConfig.Instance.GetAnalysers().ToArray())
							.AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray())
							.AddDiagnoser(MemoryDiagnoser.Default)
							// When optimizations are disabled, issue a warning, but still allow benchmarking to go ahead.
							// (It can sometimes be useful to benchmark unoptimized code.)
							.WithOptions(ConfigOptions.DisableOptimizationsValidator);
		using (var logger = new LINQPad.Benchmark.LiveSummaryLogger(config))
		using (ExecutionEngine.SuspendDump())
		{

			var powerRestorer = new LINQPad.Benchmark.PowerPlanRestorer();
			UserQuery.QueryInstance.QueryCancelToken.Register(() =>
			{
				powerRestorer.Restore();
				logger.Cancel();
				// Now that we've cleaned up, we can cancel benchmarking immediately by exiting the process.
				Environment.Exit(0);
			});

			config.AddLogger(logger);
			var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run(this.GetType(), config);
			logger.Complete(new[] { summary });
			return summary;
		}
	}


	[Benchmark]
	public virtual object SolveA()
	{
		this.AnswerA = this.SolveA(this.Lines).ToString();
		this.dc.UpdateContent(this);
		return this.AnswerA;
	}

	[Benchmark]
	public virtual object SolveB()
	{		
		this.AnswerB = this.SolveB(this.Lines).ToString();
		this.dc.UpdateContent(this);
		return this.AnswerB;
	}

	public string[] Lines => this.Storage.InputLines;


	private DumpContainer dc = new DumpContainer().Dump(1);
	
	//private static readonly string NotSolved = nameof(NotSolved).Humanize();	
	private static readonly Uri urlAOC = new( @$"https://adventofcode.com/");

	public AoCStorage Storage {get; private set;}

	private Login Login {get;} = Login.Google;
	
	public Hyperlink Refresh;
	
	public AdventOfCode()
	{
		this.Refresh = new Hyperlink("Click me", onClick: btn => 
		{ 
			btn.Text = "Checking";
			this.RefreshFromSite();
			dc.UpdateContent(this);
			btn.Text = "Refreshed";			
		});		
		//load data
		this.SaveAndLoadLite();
		
		//dump this							
		this.dc.UpdateContent(this);
	}
	
	private void SaveAndLoadLite()
	{
		var queryFile = new FileInfo(Util.CurrentQueryPath);
		var file = Path.Combine(queryFile.Directory.FullName, "storage.litedb");
		using(var lite = new LiteDatabase(file))
		{			
			var coll = lite.GetCollection<AoCStorage>();
						
			//not yet existing
			if (this.Storage == null)
			{
				this.Storage = coll.Find(z => z.Year == this.Year && this.Day == z.Day).SingleOrDefault();
				
				//done
				if (this.Storage != null) return;
				
				//create new
				this.Storage = new AoCStorage(this.Year, this.Day);
				this.Storage.LoadData(this);
				coll.Upsert(this.Storage);
				
			}
			coll.Upsert(this.Storage);
		}
	}
	
	private void RefreshFromSite()
	{
		this.Storage.LoadData(this);
		SaveAndLoadLite();
	}

	internal string DownloadPage(string append)
	{
		//create webclient
		var client = new HttpClient();
		client.BaseAddress = urlAOC;
		
		//set cookie
		//client.DefaultRequestHeaders.Add("session", Util.GetPassword(this.Login.ToString()).Dump());
		client.DefaultRequestHeaders.Add("Cookie", $"session={Util.GetPassword(this.Login.ToString())}");
		
		//get result				
		var result = client.GetAsync($"/{this.Year}/day/{this.Day}{append}")
					.GetAwaiter().GetResult();
		
		//succes downloading?
		if (result.IsSuccessStatusCode == false)
			"SOMETHING WRONG WITH GETDATA".BigRedDump();
		
		return result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
	}

	public bool SubmitAnswer(string answer, Part part)
	{
		var dic = part switch
		{
			Part.A => this.Storage.dicAnswersA,
			Part.B => this.Storage.dicAnswersB,
			_ => throw new NotImplementedException()
		};
							
		var seen = dic.Where(z => z.answer == answer).SingleOrDefault();
		var correct = dic.Where(z => z.correct).SingleOrDefault();
		
		//we know the correct answer and its the same
		if(seen?.correct == true)
		{
			$"ALL GOOD answer ({answer})".BigRedDump("green");
			return true;
		}
		
		//wrong answer, but we do know the right one
		if(correct != null)
		{	
			$"WRONG ANSWER {answer} should be {(correct?.answer ?? "???")}".BigRedDump();			
			return false;
		}

		//do we need to wait?
		if (Storage.nextAllowedSubmit != null && Storage.nextAllowedSubmit > DateTime.Now)
		{
			$"NEED TO WAIT TILL {this.Storage.nextAllowedSubmit}".BigRedDump();
			return false;
		}

		var postResult = HttpPostAnswer(part, answer);
		
		//check waits
		this.Storage.CheckNeedToWait(postResult);
		if(Storage.nextAllowedSubmit != null && Storage.nextAllowedSubmit > DateTime.Now)
		{
			$"NEED TO WAIT TILL {this.Storage.nextAllowedSubmit}".BigRedDump();
			return false;
		}

		//check answer
		//right answer?
		var valid = postResult.Contains("That's the right answer!");
		
		//make sure its stored
		this.Storage.AddAnswer(answer, part, valid, postResult);
		this.SaveAndLoadLite();

		//dump this							
		this.dc.UpdateContent(this);

		return valid;
	}

	/// <summary>Get the answer</summary>
	public string HttpPostAnswer(Part part, string answer)
	{
		//is this answer not tried yet, and no valid answer present.
		if (string.IsNullOrWhiteSpace(answer) == false)
		{
			//create client
			var client = new HttpClient();			
			client.BaseAddress = urlAOC; // urlAOC;
			client.DefaultRequestHeaders.Add("Cookie", $"session={Util.GetPassword(this.Login.ToString())}");
			
			// Create the request message
			var request = new HttpRequestMessage(HttpMethod.Post, $"/2016/day/{this.Day}/answer");

			// Add the content-type header
			var dic = new Dictionary<string, string>(){
				{"level", ((int)part).ToString()},
				{ "answer" , answer}};

			var form = new FormUrlEncodedContent(dic);
			request.Content = form;			


			var result = client.SendAsync(request).Result;
			
			var content = result.Content.ReadAsStringAsync().Result;
			

			if (result.StatusCode != System.Net.HttpStatusCode.OK)
			{
				content.Dump();
				throw new Exception("Error post");
			}

			//print data
			Util.RawHtml(content).Dump("Result From AOC", 0);
			
			return content;
		}
		throw new NotImplementedException("huh?");
	}

	public override string ToString() => $"{Year} day {Day:D2}";
}

public record class AocSubmissions(string answer, bool correct, DateTime? when, string result);

public class AoCStorage
{		
	public ObjectId Id {get;set;}
	
	public int Day {get; set;}
	public int Year {get; set;}
	
	public DateTime? nextAllowedSubmit {get; set;}
	
	public string[] InputLines {get; set;}

	public Dictionary<int, string> CodeBlocks { get; private set; }

	[BsonIgnore]
	public string ValidAnswerA => dicAnswersA.FirstOrDefault(z => z.correct)?.answer;
	
	[BsonIgnore]
	public string ValidAnswerB => dicAnswersB.FirstOrDefault(z => z.correct)?.answer;


	public List<AocSubmissions> dicAnswersA {get; set;} = new ();
	public List<AocSubmissions> dicAnswersB {get; set;} = new ();	
	
	public AoCStorage(int year, int day) 
	{
		this.Day = day;
		this.Year = year;
	}
	
	public void GetLines(AdventOfCode aoc) => this.InputLines = aoc.DownloadPage("/input")?.TrimEnd().Split(new[] { '\n', '\r' }).ToArray();		
	
	public void LoadData(AdventOfCode aoc)
	{
		//get data
		var data = aoc.DownloadPage("");

		//check sign in 
		if (SignedIn(data) == false) { "NOT SIGNED IN".BigRedDump(); return; }
		
		//get lines
		GetLines(aoc);

		//prev solves
		CheckPreviousSolves(data);

		//Parse codeblocks
		CheckCodeBlocks(data);
	}

	public void CheckNeedToWait(string data)
	{
		var dicT = Enumerable.Range(1, 60).Select(x => (word: x.ToWords(), number: x))
		   .Concat(Enumerable.Range(1, 60).Select(x => (word: x.ToString(), number: x)))
		   .ToDictionary(x => x.word, x => x.number);
		dicT.Add("", 0);

		//Please wait one minute before trying again. (You guessed 1.)		
		//please wait  5 minutes before trying again.

		//You have 4m 31s left to wait.
		//You have    40s left to wait.

		Match match;
		//Too early
		if ((match = Regex.Match(data, @"(?<min>\w+) minutes? before trying again")).Success)
		{
			this.nextAllowedSubmit = DateTime.Now + TimeSpan.FromMinutes(dicT[match.Groups["min"].Value]);
		}
		else if ((match = Regex.Match(data, @"(?<min>\w*)m* (?<sec>\w+)s left to wait")).Success)
		{
			this.nextAllowedSubmit = DateTime.Now + TimeSpan.FromSeconds(int.Parse(match.Groups[1].Value) + 4);
		}
		else if (data.Contains("wait"))
		{
			(new[] { data }).Dump(depth: 0, description: "FOUND WAIT IN OUTPUT???");
			throw new Exception("NEED TO WAIT BUT NOT PARSED");
		}		
	}

	public bool SignedIn(string data)
	{
		return data.Contains("To play, please identify yourself via one of these services:") == false;
	}

	public void CheckCodeBlocks(string data)
	{
		var m = Regex.Matches(data, @"(((?<open><code>)[^<]*)+([^<]*(?<close-open></code>))+)+(?(open)(?!))");

		CodeBlocks = m.Select(x => System.Web.HttpUtility.HtmlDecode(x.Groups.Values.Last().Value))
						.Distinct()
						.Select((x, i) => (x, i))
						.ToDictionary(x => x.i, x => x.x.ToString());
	}

	public bool CheckPreviousSolves(string data)
	{
		var matches = Regex.Matches(data, @"Your puzzle answer was <code>([\w\d]+)", RegexOptions.None);

		int entries = dicAnswersA.Count + dicAnswersB.Count;
		//if (matches.Count == 0) 
		if (matches.Count >= 1)
		{
			var answerA = matches[0].Groups[1].Value;
			if (dicAnswersA.Any(a => a.answer == answerA) == false)			
				dicAnswersA.Add(new AocSubmissions(answerA, true, null, ""));
			
		}
		if (matches.Count >= 2)
		{
			var answerB = matches[1].Groups[1].Value;
			if (dicAnswersB.Any(a => a.answer == answerB) == false)
				dicAnswersB.Add(new AocSubmissions(answerB, true, null, ""));

		}
		
		//changes so signal save
		return entries !=  dicAnswersA.Count + dicAnswersB.Count;
	}

	internal void AddAnswer(string answer, Part part, bool valid, string postResult)
	{
		var target = part switch{
			Part.A => dicAnswersA,
			Part.B => dicAnswersB,
			_ => throw new NotImplementedException()
		};

		if (target.Any(z => z.answer == answer))
		{
			throw new Exception("Already saved");
		}
		
		target.Add(new AocSubmissions(answer, valid, DateTime.Now, postResult));				
	}
}

public static class  Exte
{
	public static string BigRedDump(this string s, string color = "red", int size = 3)
	{
		// You can dump any other HTML element as follows:
		var h1 = new LINQPad.Controls.Control("h" + size, s).Dump();

		// To apply styles:
		h1.Styles["color"] = color;

		return s;
	}
	
	public static void DumpMain(this string request)
	{		
		request = request.Substring(request.IndexOf("<main>") + 6);
		request = request.Substring(0, request.IndexOf("</main>"));

		var dc = new DumpContainer().Dump();
		dc.UpdateContent(new [] { Util.RawHtml(request)});				
	}

}