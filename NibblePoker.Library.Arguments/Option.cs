namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Class <c>Option</c> models an option linked to one or more <c>Verb</c> that can be given in launch arguments to pass a valid
///  or toggle some actions.
/// </summary>
public class Option {
	/// <summary>
	/// Token character used when searching for the <c>Option</c> or rendering the help text.
	/// </summary>
	public readonly char? Token;
	
	/// <summary>
	/// Name string used when searching for the <c>Option</c> or rendering the help text.
	/// </summary>
	public readonly string? Name;
	
	/// <summary>
	/// Description that is shown in the help text.
	/// </summary>
	public readonly string Description;
	
	/// <summary>
	/// Binary flags used to toggle some special behaviour for the <c>Option</c> during the parsing process.
	/// See <see cref="OptionFlags">OptionFlags</see>.
	/// </summary>
	public readonly OptionFlags Flags;
	
	/// <summary>
	/// List of values given to the <c>Option</c> if it is allowed to have any.
	/// </summary>
	public readonly List<string> Arguments;
	
	/// <summary>
	/// Counter used to indicate how many times the <c>Option</c> was used during the parsing process.
	/// </summary>
	public int Occurrences;
	
	/// <summary>
	/// Common constructor for any <c>Option</c>.
	/// </summary>
	/// <param name="token">Token used when searching for it in a verb.</param>
	/// <param name="name">Name used when searching for it in a verb.</param>
	/// <param name="description">Description that is shown in the help text.</param>
	/// <param name="flags"> Binary flags used to toggle some special behaviour.</param>
	/// <exception cref="Exceptions.MissingOptionIdentifierException">
	/// Thrown is a null 'Token' and 'Name' was given.
	/// </exception>
	/// <exception cref="Exceptions.InvalidFlagsException">
	/// Thrown if some invalid <see cref="OptionFlags">OptionFlags</see> combination is given in 'flags'.
	/// </exception>
	public Option(char? token, string? name, string description = "", OptionFlags flags = OptionFlags.None) {
		Token = token;
		Name = name;
		Description = description;
		Flags = flags;
		
		Arguments = new List<string>();
		Occurrences = 0;
		
		if(!HasToken() && !HasName()) {
			throw new Exceptions.MissingOptionIdentifierException("The option doesn't have a token or name !");
		}
		
		if(IsDefault() && !CanHaveValue()) {
			throw new Exceptions.InvalidFlagsException("A default options must have one or more value !");
		}
	}
	
	/// <summary>
	/// Checks if the <c>Option</c> has a token.
	/// </summary>
	/// <returns>True if it has a token, false otherwise</returns>
	public bool HasToken() {
		return Token != null;
	}
	
	/// <summary>
	/// Checks if the <c>Option</c> has a name.
	/// </summary>
	/// <returns>True if it has a name, false otherwise</returns>
	public bool HasName() {
		return Name != null;
	}
	
	/// <summary>
	/// Retrieve a proper and safe to print name for the option.
	/// </summary>
	/// <param name="addTicks">Add the '-' and '--' to the output</param>
	/// <returns>The name in the 't', 'token' or 't|token' format.</returns>
	public string GetFullName(bool addTicks = false) {
		return (HasToken() ? (addTicks ? "-" : "") + Token : "") +
		       (HasToken() && HasName() ? "|" : "") +
		       (HasName() ? (addTicks ? "--" : "") + Name : "");
	}
	
	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see> flag in its
	/// constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsDefault() {
		return Flags.HasFlag(OptionFlags.Default);
	}
	
	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> flag in its
	///  constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool CanHaveValue() {
		return Flags.HasFlag(OptionFlags.HasValue);
	}
	
	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flag in its
	/// constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsRepeatable() {
		return Flags.HasFlag(OptionFlags.Repeatable);
	}

	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags in its
	/// constructor.
	/// </summary>
	/// <returns>True if the flags were given, false otherwise.</returns>
	public bool CanHaveMultipleValue() {
		return CanHaveValue() && IsRepeatable();
	}

	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Hidden">OptionFlags.Hidden</see> flag in its constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsHidden() {
		return Flags.HasFlag(OptionFlags.Hidden);
	}

	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see> flag in its
	/// constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsRequired() {
		return Flags.HasFlag(OptionFlags.Required);
	}

	/// <summary>
	/// Checks if the option was given the
	/// <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see> flag in its
	/// constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool ShouldStopParsing() {
		return Flags.HasFlag(OptionFlags.StopsParsing);
	}
	
	/// <summary>
	/// Checks if the option was used and received a value during the parsing process.
	/// </summary>
	/// <returns>True if it one or more value, false otherwise.</returns>
	public bool HasValue() {
		return Arguments.Count >= 1;
	}

	/// <summary>
	/// Checks if the option was used in the parsed launch arguments.
	/// </summary>
	/// <returns>True if the option was used at least once, false otherwise.</returns>
	public bool WasUsed() {
		return Occurrences >= 1;
	}

	/// <summary>
	/// Clears any field that may be modified once the launch arguments are parsed.
	/// </summary>
	public void Clear() {
		Arguments.Clear();
		Occurrences = 0;
	}
}