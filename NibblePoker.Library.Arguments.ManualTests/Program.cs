using NibblePoker.Library.Arguments;

// This file should only be used for manual test during development !


// ------------------------------------


// Test for #13 where tokens with value cause alignment issues.

Verb rootVerb = new Verb(null);

rootVerb.RegisterOption(new Option('a', null, "desc a"));
rootVerb.RegisterOption(new Option('b', null, "desc b", OptionFlags.HasValue));
//rootVerb.RegisterOption(new Option('c', "charlie", "desc c", OptionFlags.HasMultipleValue));
rootVerb.RegisterOption(new Option('d', "daniel", "desc d"));
rootVerb.RegisterOption(new Option('e', "emilie", "desc e", OptionFlags.HasValue));

// These 2 get pushed forward to the size of the token AND its value part.
rootVerb.RegisterOption(new Option(null, "fabrice", "desc f"));
rootVerb.RegisterOption(new Option(null, "george", "desc g", OptionFlags.HasValue));

Console.WriteLine(HelpText.GetFullHelpText(rootVerb, "test.exe"));

// Before:
// Options:
//   -a                             desc a
//   -b <VALUE>                     desc b
//   -d, --daniel                   desc d
//   -e, --emilie <EMILIE>          desc e
//               --fabrice          desc f
//               --george <GEORGE>  desc g

// After: Properly aligned


// ------------------------------------


/*
// Test for #12 where hidden verb affect the ones around it.

Verb rootVerb = new Verb(null);

rootVerb.RegisterOption(new Option('a', null, "desc a"));
rootVerb.RegisterOption(new Option('b', null, "desc b", OptionFlags.Hidden));
rootVerb.RegisterOption(new Option('c', null, "desc c"));

Console.WriteLine(HelpText.GetFullHelpText(rootVerb, "test.exe"));

// Before: The desc for `-b`, would appear for `-c`.
// After: No longer the case.
*/
