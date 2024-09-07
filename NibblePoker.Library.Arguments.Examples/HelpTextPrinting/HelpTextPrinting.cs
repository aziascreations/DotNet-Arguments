using System;
using System.Linq;

#pragma warning disable IDE0090 // Use 'new(...)'
namespace NibblePoker.Library.Arguments.Examples.HelpTextPrinting {
    internal static class HelpTextPrinting {
        private static readonly Option HelpOption = new Option('h', "help", "Shows this help text.", OptionFlags.StopsParsing);

        // Used to demo regulqr options
        private static readonly Option DemoOption1 = new Option('a', "alpha", "Option with token and name.");
        private static readonly Option DemoOption2 = new Option('b', null, "Option with token only.");
        private static readonly Option DemoOption3 = new Option(null, "charlie", "Option with a name only.");
        private static readonly Option DemoOption4 = new Option('ß', "weiß", "Option with non-ascii token and name with non-ascii letter.");
        private static readonly Option DemoOption5 = new Option('e', "echo", String.Join(" ", Enumerable.Repeat("This is long description.", 20)));

        // Used to demo the hidden options
        private static readonly Option SecretDebugOption = new Option('d', "debug", "Secret debugging option.", OptionFlags.Hidden);

        // Verbs
        private static readonly Verb RootVerb = new Verb("root")
            .RegisterOptionRecursively(HelpOption)
            .RegisterOption(DemoOption1)
            .RegisterOption(DemoOption2)
            .RegisterOption(DemoOption3)
            .RegisterOption(DemoOption4)
            .RegisterOption(DemoOption5)
            .RegisterOption(SecretDebugOption);

        private static void Main(string[] args) {
            Verb relevantVerb;

            try {
                relevantVerb = ArgumentsParser.ParseArguments(RootVerb, args);
            } catch (ArgumentException err) {
                Console.Error.WriteLine("Failed to process launch arguments !");
                Console.Error.WriteLine(err.Message);
                return;
            }

            if(args.Count() == 0 || HelpOption.WasUsed()) {
                Console.WriteLine(HelpText.GetFullHelpText(RootVerb, "example.exe", (uint) Console.BufferWidth - 1, 1, 2, false));
                return;
            }

            Console.WriteLine("");

            Console.WriteLine("Goodbye :)");
        }
    }
}
