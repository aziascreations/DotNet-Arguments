using NUnit.Framework;

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
	public void TestNonNestedOption() {
		// Safety clear
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"-a", "--bravo"});
			Assert.That(returnedVerb, Is.EqualTo(_rootVerb));
		});

		Assert.Multiple(() => {
			Assert.That(_subVerb1.WasUsed, Is.False);
			Assert.That(_subSubVerb1.WasUsed, Is.False);
			Assert.That(_subVerb2.WasUsed, Is.False);
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_longFlagOption.WasUsed, Is.True);
		});
	}

	[Test]
	public void TestValidNestedOptions() {
		_rootVerb.Clear();
		Assert.DoesNotThrow(() => {
			Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"create", "-e"});
			Assert.That(returnedVerb, Is.EqualTo(_subVerb1));
		});
		Assert.Multiple(() => {
			Assert.That(_subVerb1.WasUsed, Is.True);
			Assert.That(_subSubVerb1.WasUsed, Is.False);
			Assert.That(_subVerb2.WasUsed, Is.False);
			Assert.That(_shortNestedFlagOption.WasUsed, Is.True);
		});
		
		_rootVerb.Clear();
		Assert.DoesNotThrow(() => {
			Verb returnedVerb = ArgumentsParser.ParseArguments(_rootVerb, new[]{"create", "new"});
			Assert.That(returnedVerb, Is.EqualTo(_subSubVerb1));
		});
		Assert.Multiple(() => {
			Assert.That(_subVerb1.WasUsed, Is.True);
			Assert.That(_subSubVerb1.WasUsed, Is.True);
			Assert.That(_subVerb2.WasUsed, Is.False);
		});
	}

	[Test]
	public void TestInvalidNestedOptions() {
		// Testing '-a create' which isn't allowed since the verb parsing flag is set in the parser
		// In this case, it attempts to parse the verb as a value for the default argument.
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.NoDefaultOptionFoundException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-a", "create"});
		});
		
		// Testing 'create' and 'delete' in the same process which shouldn't work since they both belong to the same parent verb.
		// Same reasoning for the exception here !
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.NoDefaultOptionFoundException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"create", "delete"});
		});
	}
}