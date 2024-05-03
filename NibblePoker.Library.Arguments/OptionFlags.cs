namespace NibblePoker.Library.Arguments; 

/// <summary>
///   Binary enum that contains all the flags an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
///    can use.<br/>
///   These flags can toggle some special behaviour and conditions during the parsing process.
/// </summary>
[Flags]
public enum OptionFlags {
	/// <summary>
	///   Used by default to indicate that the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    has no special features and should act as a single-use toggle.
	/// </summary>
	None = 0b0000_0000,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    should be a default one, meaning that it can be used without using the short and long argument.<br/>
	///   Must be used alongside <see cref="OptionFlags.HasValue">OptionFlags.HasValue</see> !
	/// </summary>
	/// <seealso cref="NibblePoker.Library.Arguments.Verb.GetRelevantDefaultOption"/>
	Default = 0b0000_0001,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    can have at least one value.
	/// </summary>
	HasValue = 0b0000_0010,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    can be used multiple time with or without a value.
	/// </summary>
	Repeatable = 0b0000_0100,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    can have at one or more value(s).
	/// </summary>
	HasMultipleValue = HasValue | Repeatable,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    shouldn't be shown in the help text.
	/// </summary>
	/// <seealso cref="NibblePoker.Library.Arguments.HelpText.GetFullHelpText"/>
	Hidden = 0b0000_1000,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    must be used in the final <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.<br/>
	///   If used alongside <see cref="OptionFlags.AllowVerbsAfter">OptionFlags.AllowVerbsAfter</see>
	///    it must also have been used in any parent <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
	/// </summary>
	Required = 0b0001_0000,
	
	/// <summary>
	///   Used to indicate that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    must stop the parsing process as soon as it has been handled.<br/>
	///   This flag is typically reserved for options such as <c>--help</c> or <c>--version</c>.<br/>
	///   Ignores any checks related to <see cref="OptionFlags.Required">OptionFlags.Required</see> !
	/// </summary>
	StopsParsing = 0b0010_0000,
	
	/// <summary>
	///   Indicates that an <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    will not prevent further <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    from being parsed after it is encountered.<br/>
	///   Usage of this flag required some forethought if the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    has the same name as a sub-<see cref="NibblePoker.Library.Arguments.Verb">Verb</see> while using
	///    the <see cref="OptionFlags.Default">OptionFlags.Default</see> flag.<br/>
	///   In those cases, the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>
	///    will take precedence over the <see cref="NibblePoker.Library.Arguments.Option">Option</see>
	///    unless it is explicitly used with its token or name.<br/>
	///   It is recommended to only use it with the <see cref="OptionFlags.Required">OptionFlags.Required</see>
	///    flag to force the user into explicitly using the token or name.
	/// </summary>
	AllowVerbsAfter = 0b0100_0000,
	
	/// <summary>
	///   Used for tests, do not use in any program !
	/// </summary>
	All = None | Default | HasValue | Repeatable | HasMultipleValue | Hidden | AllowVerbsAfter,
}
