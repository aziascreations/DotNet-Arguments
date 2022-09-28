namespace NibblePoker.Library.Arguments; 

[Flags]
public enum OptionFlags {
	None = 0b0000_0000,
	Default = 0b0000_0001,
	HasValue = 0b0000_0010,
	Repeatable = 0b0000_0100,
	HasMultipleValue = HasValue | Repeatable,
	Hidden = 0b0000_1000,
	All = None | Default | HasValue | Repeatable | HasMultipleValue | Hidden,
}
