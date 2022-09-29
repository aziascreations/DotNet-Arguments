namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Class <c>Option</c> models an option linked to one or more <c>Verb</c> that can be given in launch arguments to pass a valid
///  or toggle some actions.
/// </summary>
public class Option {
	public readonly char? Token;
	public readonly string? Name;
	public readonly string Description;
	public readonly OptionFlags Flags;
	
	public readonly List<string> Arguments;
	public int Occurrences;
	
	public Option(char? token, string? name, string description = "", OptionFlags flags = OptionFlags.None) {
		Token = token;
		Name = name;
		Description = description;
		Flags = flags;
		
		Arguments = new List<string>();
		Occurrences = 0;
		
		if(IsDefault() && !CanHaveValue()) {
			throw new Exceptions.InvalidFlagsException("A default options must have one or more value !");
		}
	}

	public bool HasToken() {
		return Token != null;
	}

	public bool HasName() {
		return Name != null;
	}

	public string GetFullName() {
		return (HasToken() ? Token : "") + (HasToken() && HasName() ? "|" : "") + (HasName() ? Name : "");
	}

	public bool IsDefault() {
		return Flags.HasFlag(OptionFlags.Default);
	}
	
	/// <summary>
	/// Checks if the option was given the "F:NibblePoker.Library.Arguments.OptionFlags.HasValue" flag in its
	///  constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool CanHaveValue() {
		return Flags.HasFlag(OptionFlags.HasValue);
	}
	
	/// <summary>
	/// Checks if the option was given the "F:NibblePoker.Library.Arguments.OptionFlags.Repeatable" flag in its
	///  constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsRepeatable() {
		return Flags.HasFlag(OptionFlags.Repeatable);
	}

	/// <summary>
	/// Checks if the option was given the "F:NibblePoker.Library.Arguments.OptionFlags.HasValue" and
	///  "F:NibblePoker.Library.Arguments.OptionFlags.Repeatable" flags in its constructor.
	/// </summary>
	/// <returns>True if the flags were given, false otherwise.</returns>
	public bool CanHaveMultipleValue() {
		return CanHaveValue() && IsRepeatable();
	}

	/// <summary>
	/// Checks if the option was given the "F:NibblePoker.Library.Arguments.OptionFlags.Hidden" flag in its constructor.
	/// </summary>
	/// <returns>True if the flag was given, false otherwise.</returns>
	public bool IsHidden() {
		return Flags.HasFlag(OptionFlags.Hidden);
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