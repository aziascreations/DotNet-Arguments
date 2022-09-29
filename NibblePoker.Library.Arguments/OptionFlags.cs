namespace NibblePoker.Library.Arguments; 

/// <summary>
/// ???
/// </summary>
[Flags]
public enum OptionFlags {
	/// <summary>
	/// ???
	/// </summary>
	None = 0b0000_0000,
	
	/// <summary>
	/// ???
	/// </summary>
	Default = 0b0000_0001,
	
	/// <summary>
	/// Shit
	/// </summary>
	HasValue = 0b0000_0010,
	
	/// <summary>
	/// ???
	/// </summary>
	Repeatable = 0b0000_0100,
	
	/// <summary>
	/// ???
	/// </summary>
	HasMultipleValue = HasValue | Repeatable,
	
	/// <summary>
	/// ???
	/// </summary>
	Hidden = 0b0000_1000,
	
	//Required
	
	//StopsParsing
	
	//CatchAllFallbackForVerb
	
	/// <summary>
	/// ???
	/// </summary>
	All = None | Default | HasValue | Repeatable | HasMultipleValue | Hidden,
}
