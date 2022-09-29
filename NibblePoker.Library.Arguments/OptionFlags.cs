namespace NibblePoker.Library.Arguments; 

/// <summary>
/// ???
/// </summary>
[Flags]
public enum OptionFlags {
	/// <summary>
	/// Used by default to indicate that the <c>Option</c> has no special features.
	/// </summary>
	None = 0b0000_0000,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> should be a default one,
	/// meaning that it can be used without using the short and long argument.
	/// </summary>
	Default = 0b0000_0001,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> can have at least one value.
	/// </summary>
	HasValue = 0b0000_0010,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> can be used multiple time with or without a value.
	/// </summary>
	Repeatable = 0b0000_0100,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> can have at one or more value(s).
	/// </summary>
	HasMultipleValue = HasValue | Repeatable,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> shouldn't be shown in the help text.
	/// </summary>
	Hidden = 0b0000_1000,
	
	/// <summary>
	/// Used to indicate that an <c>Option</c> must be used in the final <c>Verb</c>.
	/// </summary>
	Required = 0b0001_0000,
	
	//StopsParsing
	
	//AllowMissingValue
	
	//CatchAllFallbackForVerb - What's the point ?
	
	/// <summary>
	/// Used for tests, do not use in any program !
	/// </summary>
	All = None | Default | HasValue | Repeatable | HasMultipleValue | Hidden,
}
