# .NET - Launch Arguments Parser Library
[![Nuget.org latest version](https://img.shields.io/nuget/v/NibblePoker.Library.Arguments?label=Latest%20version)](https://www.nuget.org/packages/NibblePoker.Library.Arguments)
[![Nuget.org downloads count](https://img.shields.io/nuget/dt/NibblePoker.Library.Arguments?label=Downloads)](https://www.nuget.org/packages/NibblePoker.Library.Arguments)

A simple and 'to-the-point' library to parse launch arguments in .NET and .NET Core applications.

This library is an improved port of my [PB-Arguments](https://github.com/aziascreations/PB-Arguments) library that intended to achieve the same goals but was missing support for some features.

## Features
* Easy to use, lightweight and 'to-the-point' philosophy
  * No unnecessary types, classes, procedures and whatnot
  * Loose declaration, registration and parsing can be done in a [**single** human-readable 'nested' statement](https://github.com/aziascreations/DotNet-Arguments/blob/master/NibblePoker.Library.Arguments.Demo.LooseDeclaration/Program.cs).
* Support for 'git-like' verbs
* Different behavior for options
  * Required options
  * Repeatable flag-like options
  * Multiple value
  * Multiple default option per verb with index-based ordering
  * Hidden in help text
  * Early parser exit
* Configurable help text printer
* Easy exception filtering with inheritance
  * 1 common parent
  * 3 child for distinct parts of the library
  * 14 final errors thrown in specific places.

## Requirements
* Any OS
* Any CPU
* .NET v6.0
* C# 10.0

## Building
See [building.md](building.md)

## Documentation
Go to [https://aziascreations.github.io/DotNet-Arguments/](https://aziascreations.github.io/DotNet-Arguments/) for the HTML documentation.

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
* [Loose Declaration](https://github.com/aziascreations/DotNet-Arguments/blob/master/NibblePoker.Library.Arguments.Demo.LooseDeclaration)
  * *Declaration, registration and parsing done in a single nested statement*

## License
[MIT License](https://github.com/aziascreations/DotNet-Arguments/blob/master/LICENSE)

Keep in mind, the [doxygen-awesome-css](https://github.com/jothepro/doxygen-awesome-css) submodule repository uses an
[MIT license](https://github.com/jothepro/doxygen-awesome-css/blob/main/LICENSE).
