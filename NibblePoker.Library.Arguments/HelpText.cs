using System.Text;

namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Static class that contains helpers related to printing a help text.
/// </summary>
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
	/// <param name="verb">The <c>Verb</c> for which the help text should be rendered.</param>
	/// <param name="programName">Executable's name to use.</param>
	/// <param name="consoleWidth">The console width used to calculate properly formatted and spaced line returns.</param>
	/// <param name="addVerbs">Toggle to include or exclude the <c>Verb</c>s.</param>
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
	/// <param name="verb">The <c>Verb</c> for which the help text should be rendered.</param>
	/// <param name="consoleWidth">The console width used to calculate properly formatted and spaced line returns.</param>
	/// <param name="leftSpace">Amount of spaces for any non-heading text.</param>
	/// <param name="innerSpace">Amount of spaces between the <c>Option</c> and their description.</param>
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
	
	/// <summary>
	/// Retrieves the sub-<c>Verb</c> details section for the given <c>Verb</c>.
	/// </summary>
	/// <param name="verb">The <c>Verb</c> for which the help text should be rendered.</param>
	/// <param name="consoleWidth">The console width used to calculate properly formatted and spaced line returns.</param>
	/// <param name="leftSpace">Amount of spaces for any non-heading text.</param>
	/// <param name="innerSpace">Amount of spaces between the <c>Verb</c> and their description.</param>
	/// <returns></returns>
	public static string GetVerbsDetails(Verb verb, uint consoleWidth = 80, uint leftSpace = 2, uint innerSpace = 2) {
		string outputString = "";
		int maxSubVerbSize = 0;

		foreach(Verb subVerb in verb.Verbs) {
			if(subVerb.Name.Length > maxSubVerbSize) {
				maxSubVerbSize = subVerb.Name.Length;
			}
		}
		
		foreach(Verb subVerb in verb.Verbs) {
			outputString += new string(' ', (int) leftSpace) + subVerb.Name +
			                new string(' ', maxSubVerbSize - subVerb.Name.Length + (int) innerSpace) + 
			                SplitToMultiline(subVerb.Description,
				                (int) consoleWidth - maxSubVerbSize - (int) innerSpace - (int) leftSpace - 1,
				                new string(' ', (int) leftSpace + maxSubVerbSize + (int)innerSpace + 1)
				                ) + "\n";
		}
		
		return outputString.TrimEnd('\n');
	}
	
	/// <summary>
	/// Retrieves the complete help text with the usage and verb/options info and description.
	/// </summary>
	/// <param name="verb">The <c>Verb</c> for which the help text should be rendered, preferably the root <c>Verb</c>.</param>
	/// <param name="programName">Executable's name to use in the usage text.</param>
	/// <param name="consoleWidth">The console width used to calculate properly formatted and spaced line returns.</param>
	/// <param name="leftSpace">Amount of spaces for any non-heading text in the <c>Verb</c> and <c>Option</c> sections.</param>
	/// <param name="innerSpace">Amount of spaces between the <c>Verb</c> and <c>Option</c> and their description.</param>
	/// <param name="addVerbs">Toggle to include or exclude the <c>Verb</c> section.</param>
	/// <returns>The complete help text as a string.</returns>
	public static string GetFullHelpText(Verb verb, string programName, uint consoleWidth = 80, uint leftSpace = 2,
		uint innerSpace = 2, bool addVerbs = true) {
		if(addVerbs && verb.Verbs.Count == 0) {
			addVerbs = false;
		}
		return GetUsageString(verb, programName, consoleWidth) + "\n\n" +
		       (addVerbs ? "Actions:\n" + GetVerbsDetails(verb, consoleWidth, leftSpace, innerSpace) + "\n\n" : "") +
		       (verb.Options.Count > 0 ? "Options:\n" + GetOptionsDetails(verb, consoleWidth, leftSpace, innerSpace) : "");
	}
}