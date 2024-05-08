using NUnit.Framework;

namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestParserWithValue {
	// Root verb
	private Verb _rootVerb = null!;
	
	// Used to test short options with value.
	private Option _shortFlagOption = null!;
	private Option _shortValueOption = null!;
	
	// Used for other tests
	private Option _singleValueOption = null!;
	private Option _multipleValueOption = null!;
	
	[SetUp]
	public void Setup() {
		// Definition
		_rootVerb = new Verb(null);
		
		_shortFlagOption = new Option('a', null);
		_shortValueOption = new Option('b', null, "", OptionFlags.HasValue);
		
		_singleValueOption = new Option('c', "charlie", "", OptionFlags.HasValue);
		_multipleValueOption = new Option('d', "delta", "", OptionFlags.HasMultipleValue);
		
		// Registration
		_rootVerb
			.RegisterOption(_shortFlagOption)
			.RegisterOption(_shortValueOption)
			.RegisterOption(_singleValueOption)
			.RegisterOption(_multipleValueOption);
		
		// Usage:
		// test.exe [-a] [-b <value>] [-c|--charlie <value>] [-d|--delta <values>]...
	}

	[Test]
	public void TestValidShortOptions() {
		// Testing '-b <value>' alone
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-b", "one"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortValueOption.WasUsed, Is.True);
			Assert.That(_shortValueOption.Occurrences, Is.EqualTo(1));
			Assert.That(_shortValueOption.Arguments[0], Is.EqualTo("one"));
		});
		
		// Testing '-ab <value>'.
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-ab", "two"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_shortValueOption.WasUsed, Is.True);
			Assert.That(_shortValueOption.Occurrences, Is.EqualTo(1));
			Assert.That(_shortValueOption.Arguments[0], Is.EqualTo("two"));
		});
		
		// Testing '-b <value> -a'.
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-b", "three", "-a"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_shortValueOption.WasUsed, Is.True);
			Assert.That(_shortValueOption.Occurrences, Is.EqualTo(1));
			Assert.That(_shortValueOption.Arguments[0], Is.EqualTo("three"));
		});
	}

	[Test]
	public void TestInvalidShortOptions() {
		// Testing '-b' alone
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.NotEnoughArgumentsException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-b"});
		});
		
		// Testing '-ba <value>'
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.OptionHasValueAndMoreShortsException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-ba", "one"});
		});
		
		// Testing '-b <value> -b <value>'
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.RepeatedSingularOptionException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[] { "-b", "two", "-b", "three" });
		});
	}
	
	[Test]
	public void TestShortMultipleOptions() {
		// Testing '-d <value> --delta <value> -d <value>'.
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-d", "one", "--delta", "two", "-d", "three"});
		});
		Assert.Multiple(() => {
			Assert.That(_multipleValueOption.WasUsed, Is.True);
			Assert.That(_multipleValueOption.Occurrences, Is.EqualTo(3));
			Assert.That(_multipleValueOption.Arguments[0], Is.EqualTo("one"));
			Assert.That(_multipleValueOption.Arguments[1], Is.EqualTo("two"));
			Assert.That(_multipleValueOption.Arguments[2], Is.EqualTo("three"));
		});
		
		// Testing '-d'.
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.NotEnoughArgumentsException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[] { "-d" });
		});
		
		// Testing '-dd <value>'.
		_rootVerb.Clear();
		
		Assert.Throws<Exceptions.OptionHasValueAndMoreShortsException>(delegate {
			ArgumentsParser.ParseArguments(_rootVerb, new[] { "-dd", "four" });
		});
	}
}