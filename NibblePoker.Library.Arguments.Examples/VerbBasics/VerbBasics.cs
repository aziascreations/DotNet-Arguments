using System;

namespace NibblePoker.Library.Arguments.Examples.VerbBasics {
    internal static class VerbBasics {
        // Declaring recursively registered options.
        private static readonly Option HelpOption = new Option('h', "help",
            "Shows this help text", OptionFlags.StopsParsing);
        
        // Declaring the root verb and its option.
        private static readonly Option RootOption = new Option('a', "alpha",
            "Root's option");
        private static readonly Verb RootVerb = new Verb(null);

        // Declaring sub-verb and its option.
        private static readonly Verb SubVerb = new Verb("sub", "Sub-verb");
        private static readonly Option SubOption = new Option('b', "bravo",
            "Sub-verb's options");


        private static void Main(string[] args) {
            // Preparing the verb and options.
            // This can be done when they're instantiated too.
            SubVerb.RegisterOption(SubOption);

            RootVerb.RegisterVerb(SubVerb);
            RootVerb.RegisterOption(RootOption);

            RootVerb.RegisterOptionRecursively(HelpOption);
            
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

            // Printing the help text for any verb with a single check
            // This is possible due to the fact the option is recursively registered, and we got "relevantVerb".
            if (HelpOption.WasUsed()) {
                Console.WriteLine(HelpText.GetFullHelpText(relevantVerb, "app.exe",
                    (uint) Console.WindowWidth - 1));
                return;
            }
            
            if (relevantVerb == RootVerb) {
                // Processing the "root-only" part
                if (RootOption.WasUsed()) {
                    Console.WriteLine("The root's option was used !");
                }

            } else if (relevantVerb == SubVerb) {
                // Processing the "sub-only" part
                if (SubOption.WasUsed()) {
                    Console.WriteLine("The sub-verb's option was used !");
                }
                
            }

            Console.WriteLine("");

            Console.WriteLine("Goodbye :)");
        }
    }
}

