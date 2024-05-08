using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NibblePoker.Library.Arguments; 

/// <summary>
///   Static class that contains helpers related to printing a help text.
/// </summary>
public static class HelpText {
	/// <summary>
	///   Splits a given text in a series of lines with a given line prefix that fit in a given width. 
	/// </summary>
	/// <param name="inputText">Text to split.</param>
	/// <param name="maxLineLength">Maximum line length.</param>
	/// <param name="sequentialPrefix">Prefix text to use for any split line.</param>
	/// <returns>The split text as a single string.</returns>
	public static string SplitToMultiline(string inputText, int maxLineLength, string sequentialPrefix) {        
		StringBuilder result = new StringBuilder();
		StringBuilder line = new StringBuilder();
		
		Stack<string> inversedStack = new Stack<string>(inputText.Split(' '));
		
		Stack<string> stack = new Stack<string>();
		while(inversedStack.Count != 0) {
			stack.Push(inversedStack.Pop());
		}
		
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
	
	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    and returns the usage part in POSIX format.
	/// </summary>
	/// <param name="option">The <see cref="NibblePoker.Library.Arguments.Option">Option</see> to process.</param>
	/// <returns>The formatted text as a string.</returns>
	public static string GetOptionUsagePart(Option option) {
		return (option.IsRequired() ? "<" : "[") + option.GetFullName(true) + 
		       (option.CanHaveValue() ? " " + (option.HasName() ? "<" + option.Name!.ToUpper() + ">" : "<VALUE>") : "") +
		       (option.IsRepeatable() || option.CanHaveMultipleValue() ? "..." : "") +
		       (option.IsRequired() ? ">" : "]");
	}
	
	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    and returns the details part.
	/// </summary>
	/// <param name="option">
	///   The <see cref="NibblePoker.Library.Arguments.Option">Option</see> to process.
	/// </param>
	/// <param name="shortOptionSpace">
	///   Amount of spaces used by short options in other lines, must include the inner comma and space.
	/// </param>
	/// <param name="addValueToShort">
	///   Add the value's name part to both the short version if both short and long options are available.
	/// </param>
	/// <returns>The formatted text as a string.</returns>
	public static string GetOptionDetailsPart(Option option, uint shortOptionSpace = 0, bool addValueToShort = false) {
		return (option.HasToken() ? "-" + option.Token + (
			       option.CanHaveValue() && (addValueToShort || !option.HasName()) ? 
				       (option.HasName() ? " <" + option.Name!.ToUpper() + ">" : " <VALUE>") +
				       (option.IsRepeatable() || option.CanHaveMultipleValue() ? "..." : "")
				       : ""
				) : new string(' ', (int) shortOptionSpace)) +
		       (option.HasToken() && option.HasName() ? ", " : "") +
		       (option.HasName() ? "--" + option.Name + (
			       option.CanHaveValue() ? 
				       (option.HasName() ? " <" + option.Name!.ToUpper() + ">" : " <VALUE>") +
				       (option.IsRepeatable() || option.CanHaveMultipleValue() ? "..." : "")
				       : ""
		       ) : "");
	}
	
	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    to get the associated usage text in POSIX format as a list of lines.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> for which the help text should be rendered.
	/// </param>
	/// <param name="programName">
	///   Executable's name to use.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="addVerbs">
	///   Toggle to include or exclude the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> from the output.
	/// </param>
	/// <returns>
	///   The formatted lines as a list.
	/// </returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
	public static List<string> GetUsageLines(Verb verb, string programName, uint consoleWidth = 80, bool addVerbs = true) {
		List<string> usageLines = new List<string>();
		List<string> usageParts = new List<string>();
		
		// Grabbing the verb usage
		if(addVerbs && verb.Verbs.Count > 0) {
			string verbs = "[";
			
			foreach(Verb subVerb in verb.Verbs) {
				verbs += subVerb.Name + "|";
			}
			
			usageParts.Add(verbs.TrimEnd('|')+"]");
		}
		
		// Getting the non-default options and then the default ones.
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
		
		// Composing the lines
		string currentLine = programName;
		int currentLineLength = currentLine.Length;
		
		foreach(string usagePart in usageParts) {
			if(currentLineLength + usagePart.Length >= consoleWidth) {
				usageLines.Add(currentLine);
				
				currentLineLength = programName.Length;
				currentLine = new string(' ', programName.Length);
			}
			
			currentLine += " " + usagePart;
			currentLineLength += usagePart.Length + 1;
		}
		
		usageLines.Add(currentLine);
		
		return usageLines;
	}
	
	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    to get the associated usage text in POSIX format.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> for which the help text should be rendered.
	/// </param>
	/// <param name="programName">
	///   Executable's name to use.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="addVerbs">
	///   Toggle to include or exclude the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> from the output.
	/// </param>
	/// <returns>
	///   The formatted lines as a string with line returns.
	/// </returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
	public static string GetUsageString(Verb verb, string programName, uint consoleWidth = 80, bool addVerbs = true) {
		return string.Join('\n', GetUsageLines(verb, programName, consoleWidth, addVerbs));
	}

	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    to get the associated <see cref="NibblePoker.Library.Arguments.Option">Options</see>'
	///    details text in POSIX format as a list.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> for which the help text should be rendered.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="leftSpace">
	///   Amount of spaces for any non-heading text.
	/// </param>
	/// <param name="innerSpace">
	///   Amount of spaces between the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    and their description.
	/// </param>
	/// <param name="addValueToShort">
	///   Add the value's name part to both the short version if both short and long options are available.
	/// </param>
	/// <returns>
	///   The <see cref="NibblePoker.Library.Arguments.Option">Options</see>' details as a list of strings.
	/// </returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
	public static List<string> GetOptionsDetailsLines(Verb verb, uint consoleWidth = 80, uint leftSpace = 2, uint innerSpace = 2, bool addValueToShort = false) {
		// Calculating the maximum token size for the spacing later on
		int maxTokenSize = 0;
		foreach(Option option in verb.Options) {
			if(!option.HasToken()) {
				continue;
			}
			
			// Calculated as: '-a' + ?('<VALUE>' + '...').Length + ', '
			int currentTokenSize = 2 + (
					option.CanHaveValue() && (addValueToShort || !option.HasName()) ? 
						(option.HasName() ? " <" + option.Name!.ToUpper() + ">" : " <VALUE>") +
						(option.IsRepeatable() || option.CanHaveMultipleValue() ? "..." : "")
						: ""
				).Length + (option.IsRepeatable() || option.CanHaveMultipleValue() ? 3 : 0) + 2;
			
			if(currentTokenSize > maxTokenSize) {
				maxTokenSize = currentTokenSize;
			}
		}
		
		// Getting the options details and calculating the max size for those.
		List<string> optionsDetailsText = new List<string>();
		int maxDetailsSize = 0;
		
		foreach(Option option in verb.Options) {
			string currentDetails = GetOptionDetailsPart(option, (uint)maxTokenSize, addValueToShort);
			int currentSize = (int) leftSpace + currentDetails.Length + (int) innerSpace;
			
			if(currentSize > maxDetailsSize) {
				maxDetailsSize = currentSize;
			}
			
			optionsDetailsText.Add(currentDetails);
		}
		
		// Making the lines.
		List<string> returnedLines = new List<string>();
		
		for(int iOption = 0; iOption < optionsDetailsText.Count; iOption++) {
			string currentOptionDetails = optionsDetailsText[iOption];
			
			returnedLines.AddRange((
				new string(' ', (int) leftSpace) + currentOptionDetails +
				new string(' ', maxDetailsSize - (int) leftSpace - currentOptionDetails.Length - (int) innerSpace) +
				new string(' ', (int) innerSpace) +
				SplitToMultiline(verb.Options[iOption].Description, (int)consoleWidth - maxDetailsSize, new string(' ', maxDetailsSize))
			).Split('\n').Select(a => a.TrimEnd('\r', '\n', ' ')));
		}
		
		return returnedLines;
	}
	
	/// <summary>
	///   Processes a given <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    to get the associated <see cref="NibblePoker.Library.Arguments.Option">Options</see>'
	///    details text in POSIX format.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> for which the help text should be rendered.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="leftSpace">
	///   Amount of spaces for any non-heading text.
	/// </param>
	/// <param name="innerSpace">
	///   Amount of spaces between the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    and their description.
	/// </param>
	/// <param name="addValueToShort">
	///   Toggle to add the value's name part to both the short version if both short and long
	///    <see cref="NibblePoker.Library.Arguments.Option">Options</see> are available.
	/// </param>
	/// <returns>
	///   The <see cref="NibblePoker.Library.Arguments.Option">Options</see>'
	///    details as a single string with line returns.
	/// </returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
	public static string GetOptionsDetails(Verb verb, uint consoleWidth = 80, uint leftSpace = 2, uint innerSpace = 2, bool addValueToShort = false) {
		return string.Join('\n', GetOptionsDetailsLines(verb, consoleWidth, leftSpace, innerSpace, addValueToShort));
	}
	
	/// <summary>
	///   Retrieves the sub-<see cref="NibblePoker.Library.Arguments.Verb">Verbs</see>'s
	///    details section for the given <see cref="NibblePoker.Library.Arguments.Verb">Verbs</see>.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verbs</see> for which the help text should be rendered.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="leftSpace">
	///   Amount of spaces for any non-heading text.
	/// </param>
	/// <param name="innerSpace">
	///   Amount of spaces between the <see cref="NibblePoker.Library.Arguments.Verb">Verbs</see>
	///    and their description.
	/// </param>
	/// <returns>
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>'
	///    sub-<see cref="NibblePoker.Library.Arguments.Verb">Verbs</see>'
	///    details as a single string with line returns.
	/// </returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
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
	///   Retrieves the complete help text with the usage and verb/options info and description.
	/// </summary>
	/// <param name="verb">
	///   The <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> for which the help text should be rendered.
	/// </param>
	/// <param name="programName">
	///   Executable's name to use in the usage text.
	/// </param>
	/// <param name="consoleWidth">
	///   The console width used to calculate properly formatted and spaced line returns.
	/// </param>
	/// <param name="leftSpace">
	///   Amount of spaces for any non-heading text in the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    and <see cref="NibblePoker.Library.Arguments.Option">Option</see> sections.
	/// </param>
	/// <param name="innerSpace">
	///   Amount of spaces between the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    and <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    and their description.
	/// </param>
	/// <param name="addVerbs">
	///   Toggle to include or exclude the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> section.
	/// </param>
	/// <returns>The complete help text as a string.</returns>
	/// <remarks>
	///   It is recommended to subtract 1 from the console's max width to prevent
	///    weird and unexpected line returns in some command prompts.
	/// </remarks>
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