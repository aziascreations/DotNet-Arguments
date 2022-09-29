﻿namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestParserWithNoValues {
	// Root verb
	private Verb _rootVerb = null!;
	
	// Used to test the detection between every type of option.
	private Option _shortFlagOption = null!;
	private Option _longFlagOption = null!;
	private Option _mixedFlagOption1 = null!;
	private Option _mixedFlagOption2 = null!;
	
	private Option _countingOption = null!;

	[SetUp]
	public void Setup() {
		// Definition
		_rootVerb = new Verb(null);

		_shortFlagOption = new Option('a', null);
		_longFlagOption = new Option(null, "bravo");
		_mixedFlagOption1 = new Option('c', "charlie");
		_mixedFlagOption2 = new Option('d', "delta");
		
		_countingOption = new Option('e', "echo", "", OptionFlags.Repeatable);
		
		// Registration
		_rootVerb
			.RegisterOption(_shortFlagOption)
			.RegisterOption(_longFlagOption)
			.RegisterOption(_mixedFlagOption1)
			.RegisterOption(_mixedFlagOption2)
			.RegisterOption(_countingOption);
		
		// Usage:
		// test.exe [-a] [--bravo] [-c|--charlie] [-d|--delta] [[-e|--echo] ...]
	}

	[Test]
	public void TestFullCorrectUsage() {
		// Safety clear
		_rootVerb.Clear();
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-a"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_longFlagOption.WasUsed, Is.False);
			Assert.That(_longFlagOption.Occurrences, Is.EqualTo(0));
			Assert.That(_mixedFlagOption1.WasUsed, Is.False);
			Assert.That(_mixedFlagOption1.Occurrences, Is.EqualTo(0));
			Assert.That(_mixedFlagOption2.WasUsed, Is.False);
			Assert.That(_mixedFlagOption2.Occurrences, Is.EqualTo(0));
			Assert.That(_countingOption.WasUsed, Is.False);
			Assert.That(_countingOption.Occurrences, Is.EqualTo(0));
		});
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"--bravo"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_longFlagOption.WasUsed, Is.True);
			Assert.That(_longFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption1.WasUsed, Is.False);
			Assert.That(_mixedFlagOption1.Occurrences, Is.EqualTo(0));
			Assert.That(_mixedFlagOption2.WasUsed, Is.False);
			Assert.That(_mixedFlagOption2.Occurrences, Is.EqualTo(0));
			Assert.That(_countingOption.WasUsed, Is.False);
			Assert.That(_countingOption.Occurrences, Is.EqualTo(0));
		});
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-c"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_longFlagOption.WasUsed, Is.True);
			Assert.That(_longFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption1.WasUsed, Is.True);
			Assert.That(_mixedFlagOption1.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption2.WasUsed, Is.False);
			Assert.That(_mixedFlagOption2.Occurrences, Is.EqualTo(0));
			Assert.That(_countingOption.WasUsed, Is.False);
			Assert.That(_countingOption.Occurrences, Is.EqualTo(0));
		});
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"--delta"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_longFlagOption.WasUsed, Is.True);
			Assert.That(_longFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption1.WasUsed, Is.True);
			Assert.That(_mixedFlagOption1.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption2.WasUsed, Is.True);
			Assert.That(_mixedFlagOption2.Occurrences, Is.EqualTo(1));
			Assert.That(_countingOption.WasUsed, Is.False);
			Assert.That(_countingOption.Occurrences, Is.EqualTo(0));
		});
		
		Assert.DoesNotThrow(() => {
			ArgumentsParser.ParseArguments(_rootVerb, new[]{"-e", "--echo"});
		});
		Assert.Multiple(() => {
			Assert.That(_shortFlagOption.WasUsed, Is.True);
			Assert.That(_shortFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_longFlagOption.WasUsed, Is.True);
			Assert.That(_longFlagOption.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption1.WasUsed, Is.True);
			Assert.That(_mixedFlagOption1.Occurrences, Is.EqualTo(1));
			Assert.That(_mixedFlagOption2.WasUsed, Is.True);
			Assert.That(_mixedFlagOption2.Occurrences, Is.EqualTo(1));
			Assert.That(_countingOption.WasUsed, Is.True);
			Assert.That(_countingOption.Occurrences, Is.EqualTo(2));
		});
	}
}