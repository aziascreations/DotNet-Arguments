namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestHelpPrinter {
	private Verb _rootVerb = null!;

	private Verb _createVerb = null!;
	private Verb _deleteVerb = null!;
	private Verb _updateVerb = null!;
	
	private Option _mixedOption = null!;
	private Option _shortOption = null!;
	private Option _longOption = null!;
	private Option _mixedWithLongDescOption = null!;
	private Option _mixedValueOption = null!;
	private Option _shortMultipleValuesOption = null!;
	private Option _longValueOption = null!;
	private Option _defaultOption = null!;
	private Option _defaultRequiredOption = null!;
	
	[SetUp]
	public void Setup() {
		_rootVerb = new Verb("root");

		_createVerb = new Verb("create", "Create something");
		_deleteVerb = new Verb("delete", "Delete something");
		_updateVerb = new Verb("update", "Testing line return a b c d e f g h i j k l m n o p q r s t u v w x y z");
			
		_mixedOption = new Option('a', "alpha", "Lorem Ipsum");
		_shortOption = new Option('b', null, "Test 123");
		_longOption = new Option(null, "charlie", "Hello world");
		_mixedWithLongDescOption = new Option('d', "delta", "The quick brown fox jumps over the lazy dog");
		_mixedValueOption = new Option('e', "echo", "Lorem Ipsum", OptionFlags.HasValue);
		_shortMultipleValuesOption = new Option('f', null, "Test 123", OptionFlags.HasMultipleValue);
		_longValueOption = new Option(null, "golf", "Hello world", OptionFlags.HasValue);
		_defaultOption = new Option('h', "hotel", "Default option 1", OptionFlags.HasValue | OptionFlags.Default);
		_defaultRequiredOption = new Option('i', null, "Default option 2", OptionFlags.HasValue | OptionFlags.Default | OptionFlags.Required);
	}

	[Test]
	public void TestUsageParts() {
        Assert.Multiple(() => {
            Assert.That(HelpText.GetOptionUsagePart(_mixedOption), Is.EqualTo("[-a|--alpha]"));
            Assert.That(HelpText.GetOptionUsagePart(_shortOption), Is.EqualTo("[-b]"));
            Assert.That(HelpText.GetOptionUsagePart(_longOption), Is.EqualTo("[--charlie]"));
            Assert.That(HelpText.GetOptionUsagePart(_mixedWithLongDescOption), Is.EqualTo("[-d|--delta]"));
            Assert.That(HelpText.GetOptionUsagePart(_mixedValueOption), Is.EqualTo("[-e|--echo <ECHO>]"));
            Assert.That(HelpText.GetOptionUsagePart(_shortMultipleValuesOption), Is.EqualTo("[-f <VALUE>...]"));
            Assert.That(HelpText.GetOptionUsagePart(_longValueOption), Is.EqualTo("[--golf <GOLF>]"));
            Assert.That(HelpText.GetOptionUsagePart(_defaultOption), Is.EqualTo("[-h|--hotel <HOTEL>]"));
            Assert.That(HelpText.GetOptionUsagePart(_defaultRequiredOption), Is.EqualTo("<-i <VALUE>>"));
        });
    }

    [Test]
	public void TestUsage() {
        // Safety Clean
        _rootVerb.Clear();
		_rootVerb.Verbs.Clear();
		_rootVerb.Options.Clear();
		
		// Preparing the root verb without any sub-verbs or special options.
		_rootVerb.RegisterOption(_mixedOption).RegisterOption(_shortOption).RegisterOption(_longOption);
		
		// Testing basic formatting behaviour
		// > The max line length will be tested only once here.
		// > The later tests will only test some specific rendering conditions.
		// > Size:
		// >   7  - app.exe
		// >   12 - [-a|--alpha]
		// >   4  - [-b]
		// >   11 - [--charlie]
		// >   37 - Total (7 + 1 + 12 + 1 + 4 + 1 + 11)
        Assert.Multiple(() => {
	        Assert.That(HelpText.GetUsageLines(_rootVerb, "app.exe", 999, false), Has.Count.EqualTo(1));
	        Assert.That(HelpText.GetUsageLines(_rootVerb, "app.exe", 37, false), Has.Count.EqualTo(1));
	        Assert.That(HelpText.GetUsageLines(_rootVerb, "app.exe", 37 - 1, false), Has.Count.EqualTo(2));
	        Assert.That(HelpText.GetUsageLines(_rootVerb, "app.exe", 1, false), Has.Count.EqualTo(4));
        });
        
        List<string> testedLines = HelpText.GetUsageLines(_rootVerb, "app.exe", 37 - 1, false);
        Assert.Multiple(() => {
	        Assert.That(testedLines[0], Is.EqualTo("app.exe [-a|--alpha] [-b]"));
	        Assert.That(testedLines[1], Is.EqualTo("        [--charlie]"));
        });
        
        testedLines = HelpText.GetUsageLines(_rootVerb, "app.exe", 1, false);
        Assert.Multiple(() => {
	        Assert.That(testedLines[0], Is.EqualTo("app.exe"));
	        Assert.That(testedLines[1], Is.EqualTo("        [-a|--alpha]"));
	        Assert.That(testedLines[2], Is.EqualTo("        [-b]"));
	        Assert.That(testedLines[3], Is.EqualTo("        [--charlie]"));
        });
        
        Assert.That(HelpText.GetUsageString(_rootVerb, "app.exe", 37 - 1, false),
	        Is.EqualTo("app.exe [-a|--alpha] [-b]\n        [--charlie]")
        );
        
        // Adding a verb and testing with the "addVerbs" parameter set to true and false.
    }

    [Test]
	public void TestDetailsPartGetter() {
		// Safety Clean
		_rootVerb.Clear();
		_rootVerb.Verbs.Clear();
		_rootVerb.Options.Clear();
		
		// Testing the ?
		Assert.Multiple(() => {
			// With default optional arguments.
			Assert.That(HelpText.GetOptionDetailsPart(_mixedOption), Is.EqualTo("-a, --alpha"));
			Assert.That(HelpText.GetOptionDetailsPart(_shortOption), Is.EqualTo("-b"));
			Assert.That(HelpText.GetOptionDetailsPart(_longOption), Is.EqualTo("--charlie"));
			Assert.That(HelpText.GetOptionDetailsPart(_mixedWithLongDescOption), Is.EqualTo("-d, --delta"));
			Assert.That(HelpText.GetOptionDetailsPart(_mixedValueOption), Is.EqualTo("-e, --echo <ECHO>"));
			Assert.That(HelpText.GetOptionDetailsPart(_shortMultipleValuesOption), Is.EqualTo("-f <VALUE>..."));
			Assert.That(HelpText.GetOptionDetailsPart(_longValueOption), Is.EqualTo("--golf <GOLF>"));
			Assert.That(HelpText.GetOptionDetailsPart(_defaultOption), Is.EqualTo("-h, --hotel <HOTEL>"));
			Assert.That(HelpText.GetOptionDetailsPart(_defaultRequiredOption), Is.EqualTo("-i <VALUE>"));
			
			// Testing spacing
			Assert.That(HelpText.GetOptionDetailsPart(_mixedOption, 2), Is.EqualTo("-a, --alpha"));
			Assert.That(HelpText.GetOptionDetailsPart(_shortOption, 2), Is.EqualTo("-b"));
			Assert.That(HelpText.GetOptionDetailsPart(_longOption, 2), Is.EqualTo("  --charlie"));
			
			// Testing with value on tokens.
			Assert.That(HelpText.GetOptionDetailsPart(_mixedValueOption, 0, true), Is.EqualTo("-e <ECHO>, --echo <ECHO>"));
			Assert.That(HelpText.GetOptionDetailsPart(_defaultOption, 0, true), Is.EqualTo("-h <HOTEL>, --hotel <HOTEL>"));
			
			// Making sure the token value isn't added when no token is present.
			Assert.That(HelpText.GetOptionDetailsPart(_longValueOption, 0, true), Is.EqualTo("--golf <GOLF>"));
		});
	}

	[Test]
	public void TestDetailsLinesGetter() {
		// Safety Clean
		_rootVerb.Clear();
		_rootVerb.Verbs.Clear();
		_rootVerb.Options.Clear();
		
		// Preparing the root verb
		_rootVerb.RegisterOption(_mixedOption).RegisterOption(_mixedWithLongDescOption);
		
		// Sizes: 
		//  * _mixedOption  -> "-a, --alpha" => 11 + 11 for description => "Lorem Ipsum"
		//  * _mixedWith... -> "-d, --delta" => 11 + 43 for description => "The quick brown fox jumps over the lazy dog"
		
		// Testing non-line-returning default values.
		List<string> testedLines = HelpText.GetOptionsDetailsLines(_rootVerb, 999);

		Assert.Multiple(() => {
			Assert.That(testedLines, Has.Count.EqualTo(2));
			Assert.That(testedLines[0], Is.EqualTo("  -a, --alpha  Lorem Ipsum"));
			Assert.That(testedLines[1], Is.EqualTo("  -d, --delta  The quick brown fox jumps over the lazy dog"));
		});
		
		// Testing with no tokens.
		_rootVerb.Clear();
		_rootVerb.Verbs.Clear();
		_rootVerb.Options.Clear();
		
		_rootVerb.RegisterOption(_longOption).RegisterOption(_longValueOption);
		
		testedLines = HelpText.GetOptionsDetailsLines(_rootVerb, 999);

		Assert.Multiple(() => {
			Assert.That(testedLines, Has.Count.EqualTo(2));
			Assert.That(testedLines[0], Is.EqualTo("  --charlie      Hello world"));
			Assert.That(testedLines[1], Is.EqualTo("  --golf <GOLF>  Hello world"));
		});
		
		// Testing line returns and spacing on split description lines.
		_rootVerb.Clear();
		_rootVerb.Verbs.Clear();
		_rootVerb.Options.Clear();
		
		_rootVerb.RegisterOption(_mixedWithLongDescOption);
		
		// @999 => '  -d, --delta  The quick brown fox jumps over the lazy dog'
		//          ^1            ^15       ^20
		
		// @25 => [
		//     '  -d, --delta  The quick' <- 1 left at end
		//     '               brown fox' <- 1 left at end
		//     '               jumps over'
		//     '               the lazy'  <- 2 left at end
		//     '               dog'       <- 7 left at end
		// ]
		testedLines = HelpText.GetOptionsDetailsLines(_rootVerb, 25);
		
		Assert.Multiple(() => {
			Assert.That(testedLines, Has.Count.EqualTo(5));
			Assert.That(testedLines[0], Is.EqualTo("  -d, --delta  The quick"));
			Assert.That(testedLines[1], Is.EqualTo("               brown fox"));
			Assert.That(testedLines[2], Is.EqualTo("               jumps over"));
			Assert.That(testedLines[3], Is.EqualTo("               the lazy"));
			Assert.That(testedLines[4], Is.EqualTo("               dog"));
		});
		
		// @24 => [
		//     '  -d, --delta  The quick'
		//     '               brown fox'
		//     '               jumps'    <- 4 left at end
		//     '               over the' <- 1 left at end
		//     '               lazy dog' <- 1 left at end
		// ]
		testedLines = HelpText.GetOptionsDetailsLines(_rootVerb, 24);
		
		Assert.Multiple(() => {
			Assert.That(testedLines, Has.Count.EqualTo(5));
			Assert.That(testedLines[0], Is.EqualTo("  -d, --delta  The quick"));
			Assert.That(testedLines[1], Is.EqualTo("               brown fox"));
			Assert.That(testedLines[2], Is.EqualTo("               jumps"));
			Assert.That(testedLines[3], Is.EqualTo("               over the"));
			Assert.That(testedLines[4], Is.EqualTo("               lazy dog"));
		});
	}
}