namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestParserNestedVerbs {
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
		_longFlagOption = new Option(null, "bravo");
		_mixedFlagOption1 = new Option('c', "charlie");
		_mixedFlagOption2 = new Option('d', "delta");
		
		_shortNestedFlagOption = new Option('e', "echo");
		_longNestedFlagOption = new Option('f', "foxtrot");
		
		_shortCountOption = new Option('g', null);
		_longCountOption = new Option(null, "hotel");
		_mixedCountOption = new Option('i', "india");
		
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
		//   create [ new | [-e|--echo] [-f|--foxtrot] ] |
		//   delete [-g] [--hotel] [-i|--india] |
		//   [-a] [--bravo] [-c|--charlie] [-d|--delta]
		// )
	}

	[Test]
	public void TestNothing() {
		
	}
}