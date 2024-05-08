# .NET - Launch Arguments Parser Library
[![Nuget.org latest version](https://img.shields.io/nuget/v/NibblePoker.Library.Arguments?label=Latest%20version)](https://www.nuget.org/packages/NibblePoker.Library.Arguments)
[![Nuget.org downloads count](https://img.shields.io/nuget/dt/NibblePoker.Library.Arguments?label=Downloads)](https://www.nuget.org/packages/NibblePoker.Library.Arguments)
[![Repository's License](https://img.shields.io/github/license/aziascreations/DotNet-Arguments)](https://github.com/aziascreations/DotNet-Arguments/blob/master/LICENSE)

A simple and 'to-the-point' library to parse launch arguments in .NET and .NET Core applications.

This library is an improved port of my [PB-Arguments](https://github.com/aziascreations/PB-Arguments) library that intended to achieve the same goals but was
missing support for some features.<br>
It is also has the exact same features as the port in [C99-Utility-Libraries](https://github.com/aziascreations/C99-Utility-Libraries).

## Features
* Easy to use, lightweight and 'to-the-point' philosophy
  * No unnecessary types, classes, procedures and whatnot
* Support for 'git-like' verbs
* Different behavior for options
  * Required options
  * Repeatable flag-like options
  * Multiple value
  * Multiple default option per verb with index-based ordering
  * Hidden in help text
  * Early parser exit
  * In-between verbs
* Configurable help text printer
* Easy exception filtering with inheritance
  * 1 common parent
  * 3 child for distinct parts of the library
  * 14 final errors thrown in specific places.

## Requirements
* .NET Framework 4.0 or newer
* .NET v6.0 or newer

## Documentation
Go to [aziascreations.github.io/DotNet-Arguments/](https://aziascreations.github.io/DotNet-Arguments/) for the HTML documentation.

## Building
Please refer to the [building.md](building.md) file for more information.

## Basic Example
The following example shows you how to declare 2 options and how to parse and use the launch arguments.

```csharp
// Preparing options and root verb.
Option OptionHelp    = new('h', "help",    "", OptionFlags.StopsParsing);
Option OptionVerbose = new('v', "verbose", "", OptionFlags.Repeatable);

Verb RootVerb = new Verb("").RegisterOption(OptionHelp).RegisterOption(OptionVerbose);

// Parsing lanch arguments
try {
    ArgumentsParser.ParseArguments(RootVerb, args);  // 'args' is gotten from Main().
} catch(ArgumentException) {
    Console.Error.Write("Failed to parse the launch arguments !");
    RootVerb.Clear();  // Ignoring the error and simulating no launch parameters.
}

// Using the results
if(OptionHelp.WasUsed()) {
    Console.WriteLine(HelpText.GetFullHelpText(RootVerb, "app.exe"));
}

if(OptionVerbose.WasUsed() && OptionVerbose.Occurrences >= 2) {
    // We count the number of occurences to enable more logging.
    Console.WriteLine("Activating super-verbose mode !");
}
```

## Other Examples
* [Regular Declaration](https://github.com/aziascreations/DotNet-Arguments/blob/master/NibblePoker.Library.Arguments.Examples/RegularDeclaration/RegularDeclaration.cs)
  * *Standard recommended method of declaring and using the options and verbs*
* [Loose Declaration](https://github.com/aziascreations/DotNet-Arguments/blob/master/NibblePoker.Library.Arguments.Examples/LooseDeclaration/LooseDeclaration.cs)
  * *Declaration, registration and parsing done in a single nested statement*

## License
The code in this repository is licensed under
[CC0 1.0 Universal (CC0 1.0) (Public Domain)](https://github.com/aziascreations/DotNet-Arguments/blob/master/LICENSE).

The [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css) repository is used as a
submodule for Doxygen and is licensed under the [MIT license](https://github.com/jothepro/doxygen-awesome-css/blob/main/LICENSE).
