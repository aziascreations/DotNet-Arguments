# .NET - Launch Arguments Parser Library
A simple and 'to-the-point' library to parse launch arguments in .NET and .NET Core applications.

This library is an improved port of my [PB-Arguments](https://github.com/aziascreations/PB-Arguments) library that intended to achieve the same goals but was missing support for some features.

## Features
* Easy to use and 'to-the-point' philosophy
  * No unnecessary types, classes and whatnot
  * Loose declaration, registration and parsing can be done in a [**single** human-readable 'nested' statement]().
* Support for 'git-like' verbs
* Different behavior for options
  * Required options (TODO)
  * Repeatable flag-like options
  * Multiple value
  * Multiple default option per verb with index-based ordering
  * Hidden in help text
  * Early parser exit (TODO)
* Human-readable declaration, registration and usage. *
* Easy exception filtering with inheritance
  * 1 common parent
  * 3 child for distinct parts of the library
  * 13 final errors thrown in specific places.

<sub>*: See the ['commandlineparser/commandline/' demo](https://github.com/commandlineparser/commandline/blob/f738eeff7c698bfb01d0e085e97dc65d07c8aaa9/demo/ReadText.Demo/Program.cs)
for a common example of unreadable hieroglyphs looking code...</sub>

## Requirements
* Any OS
* Any CPU
* .NET v6.0
* C# 10.0

## Documentation
TODo: Automate it !

## Examples
* [Loose Declaration](NibblePoker.Library.Arguments.Demo.LooseDeclaration/)
  * *Declaration, registration and arsing done in a single nested statement*
* ???

## License
[MIT License](LICENSE)
