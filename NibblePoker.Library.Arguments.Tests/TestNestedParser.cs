namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestNestedParser {
	// Default verb
	private Verb _rootVerb = null!;
	
	// Valid sub verbs
	private Verb _subVerb1 = null!;
	private Verb _subVerb2 = null!;
	private Verb _subSubVerb1 = null!;
	
	// Used to test that they are properly detected in a given verb.
	private Option _shortFlagOption = null!;
	private Option _longFlagOption = null!;
	private Option _mixedFlagOption1 = null!;
	private Option _mixedFlagOption2 = null!;
	
	// Used to test options in sub-verbs.
	private Option _shortNestedFlagOption = null!;
	private Option _longNestedFlagOption = null!;
	
	private Option _shortCountOption = null!;
	private Option _longCountOption = null!;
	private Option _mixedCountOption = null!;
	
	[SetUp]
	public void Setup() {
		// Definition
		_rootVerb = new Verb(null);
		
		_subVerb1 = new Verb("create");
		_subVerb2 = new Verb("delete");
		_subSubVerb1 = new Verb("new");

		_shortFlagOption = new Option('a', null);
		_longFlagOption = new Option(null, "byte");
		_mixedFlagOption1 = new Option('h', "help");
		_mixedFlagOption2 = new Option('v', "version");
		
		_shortNestedFlagOption = new Option('i', "integer");
		_longNestedFlagOption = new Option('J', "John");
		
		_shortCountOption = new Option('c', null);
		_longCountOption = new Option(null, "count");
		_mixedCountOption = new Option('C', "cumulate");
		
		// Registration
		_rootVerb
			.RegisterOption(_shortFlagOption)
			.RegisterOption(_longFlagOption)
			.RegisterOption(_mixedFlagOption1)
			.RegisterOption(_mixedFlagOption2)
			.RegisterVerb(_subVerb1
				.RegisterOption(_shortNestedFlagOption)
				.RegisterOption(_longNestedFlagOption)
				.RegisterVerb(_subSubVerb1)
			)
			.RegisterVerb(_subVerb2
				.RegisterOption(_shortCountOption)
				.RegisterOption(_longCountOption)
				.RegisterOption(_mixedCountOption));
		
		// Result
		// test.exe (
		//   create [ new | [-i|--integer] [-J|--John] ] |
		//   delete [-c] [--count] [-C|--cumulate] |
		//   [-a] [--byte] [-h|--help] [-v|--version]
		// )
	}

	[Test]
	public void TestNothing() {
		
	}
}