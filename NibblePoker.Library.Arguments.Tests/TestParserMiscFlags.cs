namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestParserMiscFlags {
	// Root verb
	private Verb _rootVerb = null!;
	
	// Used to the default options
	private Option _singleDefaultOption1 = null!;
	private Option _singleDefaultOption2 = null!;
	private Option _singleDefaultOption3 = null!;
	private Option _multipleDefaultOption1 = null!;
	private Option _multipleDefaultOption2 = null!;
	
	// Required option's flag
	private Option _requiredOption1 = null!;
	private Option _requiredOption2 = null!;
	
	[SetUp]
	public void Setup() {
		_rootVerb = new Verb(null);
		
		_singleDefaultOption1 = new Option('a', null, "", OptionFlags.HasValue | OptionFlags.Default);
		_singleDefaultOption2 = new Option('b', null, "", OptionFlags.HasValue | OptionFlags.Default);
		_singleDefaultOption3 = new Option('c', null, "", OptionFlags.HasValue | OptionFlags.Default);
		_multipleDefaultOption1 = new Option('d', null, "", OptionFlags.HasMultipleValue | OptionFlags.Default);
		_multipleDefaultOption2 = new Option('e', null, "", OptionFlags.HasMultipleValue | OptionFlags.Default);
		
		_requiredOption1 = new Option('f', null, "", OptionFlags.Required | OptionFlags.HasValue);
		_requiredOption2 = new Option('g', null, "", OptionFlags.Required);
	}
	
	[Test]
	public void TestDefaultOptionsRegistration() {
		Assert.DoesNotThrow(() => {
			_rootVerb.RegisterOption(_singleDefaultOption1);
		});
		
		Assert.DoesNotThrow(() => {
			_rootVerb.RegisterOption(_singleDefaultOption2);
		});
		
		Assert.DoesNotThrow(() => {
			_rootVerb.RegisterOption(_multipleDefaultOption1);
		});
		
		Assert.Throws<Exceptions.ExistingDefaultMultipleOptionException>(delegate {
			_rootVerb.RegisterOption(_singleDefaultOption3);
		});
		
		Assert.Throws<Exceptions.ExistingDefaultMultipleOptionException>(delegate {
			_rootVerb.RegisterOption(_multipleDefaultOption2);
		});
	}
	
	[Test]
	public void TestDefaultOptionsParsingWithoutVerbs() {
		Assert.DoesNotThrow(() => {
			_rootVerb
				.RegisterOption(_singleDefaultOption1)
				.RegisterOption(_singleDefaultOption2)
				.RegisterOption(_multipleDefaultOption1);
		});
		
		// Testing valid arguments with no option name and no registered verbs !
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"one", "two", "three", "four"});
		});
		Assert.Multiple(() => {
			Assert.That(_singleDefaultOption1.WasUsed, Is.True);
			Assert.That(_singleDefaultOption1.Occurrences, Is.EqualTo(1));
			Assert.That(_singleDefaultOption1.Arguments[0], Is.EqualTo("one"));
			Assert.That(_singleDefaultOption2.WasUsed, Is.True);
			Assert.That(_singleDefaultOption2.Occurrences, Is.EqualTo(1));
			Assert.That(_singleDefaultOption2.Arguments[0], Is.EqualTo("two"));
			Assert.That(_multipleDefaultOption1.WasUsed, Is.True);
			Assert.That(_multipleDefaultOption1.Occurrences, Is.EqualTo(2));
			Assert.That(_multipleDefaultOption1.Arguments[0], Is.EqualTo("three"));
			Assert.That(_multipleDefaultOption1.Arguments[1], Is.EqualTo("four"));
		});
		
		// TODO: Test the '--' argument and its effects.
	}


	[Test]
	public void TestRequiredOptions() {
		Assert.DoesNotThrow(() => {
			// test.exe <-f <value>> <-g>
			_rootVerb.RegisterOption(_requiredOption1).RegisterOption(_requiredOption2);
		});
		
		_rootVerb.Clear();
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-f", "one", "-g"});
		});
		Assert.Multiple(() => {
			Assert.That(_requiredOption1.WasUsed, Is.True);
			Assert.That(_requiredOption1.Occurrences, Is.EqualTo(1));
			Assert.That(_requiredOption1.Arguments[0], Is.EqualTo("one"));
			Assert.That(_requiredOption2.WasUsed, Is.True);
			Assert.That(_requiredOption2.Occurrences, Is.EqualTo(1));
		});
		
		_rootVerb.Clear();
		Assert.Throws<Exceptions.MissingRequiredOptionException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-f", "one"});
		});
		
		_rootVerb.Clear();
		Assert.Throws<Exceptions.MissingRequiredOptionException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-g"});
		});
	}
}