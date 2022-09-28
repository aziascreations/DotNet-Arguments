namespace NibblePoker.Library.Arguments; 

public class Option {
	public readonly char? Token;
	public readonly string? Name;
	public readonly string Description;
	public readonly OptionFlags Flags;
	
	public List<string> Arguments;
	public int Occurrences;
	
	public Option(char? token, string? name, string description = "", OptionFlags flags = OptionFlags.None) {
		Token = token;
		Name = name;
		Description = description;
		Flags = flags;
		
		Arguments = new List<string>();
		Occurrences = 0;
	}

	public bool HasToken() {
		return Token != null;
	}

	public bool HasName() {
		return Name != null;
	}

	public bool IsDefault() {
		return Flags.HasFlag(OptionFlags.Default);
	}

	public bool CanHaveValue() {
		return Flags.HasFlag(OptionFlags.HasValue);
	}

	public bool IsRepeatable() {
		return Flags.HasFlag(OptionFlags.Repeatable);
	}

	public bool CanHaveMultipleValue() {
		return CanHaveValue() && IsRepeatable();
	}

	public bool IsHidden() {
		return Flags.HasFlag(OptionFlags.Hidden);
	}

	public bool WasUsed() {
		return Occurrences >= 1;
	}

	public void Clear() {
		Arguments.Clear();
		Occurrences = 0;
	}
}