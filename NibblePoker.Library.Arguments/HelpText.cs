using System.Text;

namespace NibblePoker.Library.Arguments; 

public static class HelpText {
	private static string SplitToMultiline(string inputText, int maxLineLength, string sequentialPrefix) {        
		StringBuilder result = new StringBuilder();
		StringBuilder line = new StringBuilder();
		
		Stack<string> stack = new Stack<string>(inputText.Split(' '));
		
		while(stack.Count > 0) {
			string word = stack.Pop();
			if(word.Length > maxLineLength) {
				string head = word[..maxLineLength];
				string tail = word[maxLineLength..];

				word = head;
				stack.Push(tail);
			}
			
			if(line.Length + word.Length > maxLineLength) {
				result.AppendLine(line.ToString());
				result.Append(sequentialPrefix);
				line.Clear();
			}
			
			line.Append(word + " ");
		}
		
		result.Append(line);
		return result.ToString().TrimEnd();
	}
	private static string GetOptionUsagePart(Option option) {
		return (option.IsRequired() ? "<" : "[") + option.GetFullName(true) + 
		       (option.CanHaveValue() ? " " + (option.HasName() ? "<" + option.Name!.ToUpper() + ">" : "<VALUE>") : "") +
		       (option.IsRepeatable() || option.CanHaveMultipleValue() ? "..." : "") +
		       (option.IsRequired() ? ">" : "]");
	}
	
	/// <summary>
	/// Processes a given <c>Verb</c> to get the associated usage text in POSIX format.
	/// </summary>
	/// <param name="verb"><c>Verb</c> to process</param>
	/// <param name="programName"></param>
	/// <param name="consoleWidth"></param>
	/// <param name="addVerbs"></param>
	/// <returns></returns>
	public static string GetUsageString(Verb verb, string programName, uint consoleWidth = 80, bool addVerbs = true) {
		List<string> usageParts = new List<string>();

		if(addVerbs) {
			string verbs = "[";
			
			foreach(Verb subVerb in verb.Verbs) {
				verbs += subVerb.Name + "|";
			}
			
			usageParts.Add(verbs.TrimEnd('|')+"]");
		}
		
		foreach(Option option in verb.Options) {
			if(!option.IsDefault()) {
				usageParts.Add(GetOptionUsagePart(option));
			}
		}
		
		foreach(Option option in verb.Options) {
			if(option.IsDefault()) {
				usageParts.Add(GetOptionUsagePart(option));
			}
		}

		string outputString = programName + " ";
		int currentLineLength = outputString.Length;

		foreach(string usagePart in usageParts) {
			if(currentLineLength + usagePart.Length > consoleWidth - 1) {
				currentLineLength = programName.Length + 1;
				outputString += "\n" + new string(' ', programName.Length + 1);
			}
			outputString += usagePart + " ";
			currentLineLength += usagePart.Length + 1;
		}
		
		return outputString;
	}

	/// <summary>
	/// Processes a given <c>Verb</c> to get the associated options' details text in POSIX format.
	/// </summary>
	/// <param name="verb"><c>Verb</c> to process</param>
	/// <param name="consoleWidth"></param>
	/// <param name="leftSpace"></param>
	/// <param name="innerSpace"></param>
	/// <returns></returns>
	public static string GetOptionsDetails(Verb verb, uint consoleWidth = 80, uint leftSpace = 2, uint innerSpace = 2) {
		string outputString = "";
		int maxOptionSize = 0;

		foreach(Option option in verb.Options) {
			int optionValueSize = option.CanHaveValue() ? 3 + (option.HasName() ? option.Name!.ToUpper().Length : 5) : 0;
			int optionSize = (option.HasToken() ? 2 + optionValueSize: 0) +
			                 (option.HasToken() && option.HasName() ? 2 : 0) +
			                 (option.HasName() ? option.Name!.Length + 2 + optionValueSize: 0);
			if(optionSize > maxOptionSize) {
				maxOptionSize = optionSize;
			}
		}
		
		foreach(Option option in verb.Options) {
			string optionValueText =
				option.CanHaveValue() ? " " + (option.HasName() ? "<" + option.Name!.ToUpper() + ">" : "<VALUE>") : "";
			string optionText = (option.HasToken() ? "-" + option.Token + optionValueText : "") +
			                    (option.HasToken() && option.HasName() ? ", " : "") +
			                    (option.HasName() ? "--" + option.Name! + optionValueText : "");
			
			optionText = new string(' ', (int)leftSpace) + optionText +
			             new string(' ', maxOptionSize - optionText.Length + (int) innerSpace);
			
			optionText += SplitToMultiline(
				option.Description, 
				(int) consoleWidth - optionText.Length - 1, 
				new string(' ', optionText.Length + 1));
			
			outputString += optionText + "\n";
		}
		
		return outputString.TrimEnd('\n');
	}
}