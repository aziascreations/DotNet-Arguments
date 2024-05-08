using System;

namespace NibblePoker.Library.Arguments.Examples.RegularDeclaration {
    internal static class RegularDeclaration {
        // Declaring the options shared in the sub-verbs.
        private static readonly Option NameOption = new('n', "name",
            "Element's name", OptionFlags.Default | OptionFlags.HasValue);

        private static readonly Option OverwriteOption = new('O', "overwrite",
            "Erases previous elements");

        // Declaring sub-verbs and registering the options in one go.
        // Can also be done in `Main()`.
        private static readonly Verb CreateVerb = new Verb("create", "Create an element")
            .RegisterOption(NameOption)
            .RegisterOption(OverwriteOption);

        private static readonly Verb DeleteVerb = new Verb("delete", "Delete an element")
            .RegisterOption(NameOption);

        // Declaring the root verb's options
        private static readonly Option HelpOption = new('h', "help",
            "Shows this help text", OptionFlags.StopsParsing);

        private static readonly Option VersionOption = new('v', "version",
            "Shows the version information", OptionFlags.StopsParsing);

        // Declaring the root verb and registering the options in one go.
        // Can also be done in `Main()`.
        private static readonly Verb RootVerb = new Verb(null)
            .RegisterVerb(CreateVerb)
            .RegisterVerb(DeleteVerb)
            .RegisterOption(HelpOption)
            .RegisterOption(VersionOption);


        private static void Main(string[] args) {
            // Parsing the launch arguments.
            Verb relevantVerb;
            try {
                relevantVerb = ArgumentsParser.ParseArguments(RootVerb, args);
            } catch (ArgumentException err) {
                Console.Error.WriteLine("Failed to process launch arguments !");
                Console.Error.WriteLine(err.Message);
                return;
            }

            // Processing parsed arguments.
            Console.WriteLine("Parsed " + args.Length + " launch argument(s) !");
            Console.WriteLine("");

            if (relevantVerb == RootVerb) {
                Console.WriteLine("The 'root' verb was used.");

                if (HelpOption.WasUsed()) {
                    Console.WriteLine(HelpText.GetFullHelpText(RootVerb, "app.exe",
                        (uint) Console.WindowWidth - 1));
                    return;
                }

                if (VersionOption.WasUsed()) {
                    Console.WriteLine("RegularDeclaration Example v1.0.0");
                    return;
                }
            } else if (relevantVerb == CreateVerb) {
                Console.WriteLine("The 'create' verb was used.");

                if (OverwriteOption.WasUsed()) {
                    Console.WriteLine("> Overwrite mode enabled !");
                }

                foreach (string inputName in NameOption.Arguments) {
                    Console.WriteLine($"> Input: {inputName}");
                }
            } else if (relevantVerb == DeleteVerb) {
                Console.WriteLine("The 'delete' verb was used.");

                foreach (string inputName in NameOption.Arguments) {
                    Console.WriteLine($"> Input: {inputName}");
                }
            }

            Console.WriteLine("");

            Console.WriteLine("Goodbye :)");
        }
    }
}
