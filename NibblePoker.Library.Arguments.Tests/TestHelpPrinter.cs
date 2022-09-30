namespace NibblePoker.Library.Arguments.Tests; 

[TestFixture]
public class TestHelpPrinter {
	private Verb _rootVerb = null!;
	
	[SetUp]
	public void Setup() {
		_rootVerb = new Verb("root")
			.RegisterOption(new Option('a', "alpha", "Lorem Ipsum"))
			.RegisterOption(new Option('b', null, "Test 123"))
			.RegisterOption(new Option(null, "charlie", "Hello world"))
			.RegisterOption(new Option('d', "delta", "The quick brown fox jumps over the lazy dog"))
			.RegisterOption(new Option('e', "echo", "Lorem Ipsum", OptionFlags.HasValue))
			.RegisterOption(new Option('f', null, "Test 123", OptionFlags.HasMultipleValue))
			.RegisterOption(new Option(null, "golf", "Hello world", OptionFlags.HasValue))
			.RegisterVerb(new Verb("create", "Create something"))
			.RegisterVerb(new Verb("delete", "Delete something"))
			.RegisterVerb(new Verb("update", "Testing line return a b c d e f g h i j k l m n o p q r s t u v w x y z"));
	}

	[Test]
	public void TestUsageText() {
		Console.WriteLine("Testing usage text renderer:");
		Console.WriteLine(new string('-', 40));
		Console.WriteLine(HelpText.GetUsageString(_rootVerb, "test.exe", 40));
	}

	[Test]
	public void TestOptionsText() {
		Console.WriteLine("Testing options text renderer:");
		Console.WriteLine(new string('-', 60));
		Console.WriteLine(HelpText.GetOptionsDetails(_rootVerb, 60));
	}

	[Test]
	public void TestFullText() {
		Console.WriteLine("Testing full text renderer:");
		Console.WriteLine(new string('-', 60));
		Console.WriteLine(HelpText.GetFullHelpText(_rootVerb, "test.exe", 60));
	}
}