using NibblePoker.Library.Arguments;

// This file should only be used for manual test during development !


// Test for #12 where hidden verb affect the ones around it.

Verb rootVerb = new Verb(null);

rootVerb.RegisterOption(new Option('a', null, "desc a"));
rootVerb.RegisterOption(new Option('b', null, "desc b", OptionFlags.Hidden));
rootVerb.RegisterOption(new Option('c', null, "desc c"));

Console.WriteLine(HelpText.GetFullHelpText(rootVerb, "test.exe"));

// Before: The desc for `-b`, would appear for `-c`.
// After: No longer the case.
