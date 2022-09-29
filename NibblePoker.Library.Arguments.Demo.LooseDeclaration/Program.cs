namespace NibblePoker.Library.Arguments.Demo.LooseDeclaration; 

internal static class Program {
	private static void Main(string[] args) {
		// Declaring, registering and parsing the launch arguments in a loose way.
		// This approach is easier to declare, but a smidge harder to read and process.
		Verb relevantVerb;

		try {
			relevantVerb = ArgumentsParser.ParseArguments(
				new Verb("root", "...")
					.RegisterOption(new Option('h', "help", "Shows this help text"))
					.RegisterOption(new Option('v', "version", "Shows the program's version"))
					.RegisterVerb(new Verb("create", "Create an element")
						.RegisterOption(new Option('n', "name", "Element's name", OptionFlags.Default | OptionFlags.HasValue))
						.RegisterOption(new Option('O', "overwrite", "Erases previous elements")))
					.RegisterVerb(new Verb("delete", "Delete an element")
						.RegisterOption(new Option('n', "name", "Element's name", OptionFlags.Default | OptionFlags.HasValue))
					)
				, args
			);
		} catch(ArgumentException err) {
			Console.Error.WriteLine("Failed to process launch arguments !");
			Console.Error.WriteLine(err.Message);
			return;
		}
		
		// Processing example.
		// We can ignore some null reference warnings since the declaration and options flags insure us that what we
		//  want will be there.
		Console.WriteLine("Parsed " + args.Length + " launch argument(s) !");
		Console.WriteLine("");
		
		switch(relevantVerb.Name) {
			case "root":
				Console.WriteLine("We are in the 'root' verb !");
				if(relevantVerb.GetOptionByName("help")!.WasUsed()) {
					Console.WriteLine("> Showing help text...");
				}
				if(relevantVerb.GetOptionByName("version")!.WasUsed()) {
					Console.WriteLine("> Showing version text...");
				}
				break;
			case "create":
				Console.WriteLine("We are in the 'create' verb !");
				Console.WriteLine("> We should create the element: " + relevantVerb.GetOptionByName("name")!.Arguments[0]);
				Console.WriteLine("> We should " + (relevantVerb.GetOptionByToken('O')!.WasUsed() ? "" : "not ") +
				                  "overwrite the element !");
				break;
			case "delete":
				Console.WriteLine("We are in the 'delete' verb !");
				Console.WriteLine("> We should delete the element: " + relevantVerb.GetOptionByName("name")!.Arguments[0]);
				break;
		}
		Console.WriteLine("");
		
		Console.WriteLine("Goodbye :)");
	}
}