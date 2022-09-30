# .NET - Launch Arguments Parser Library
A simple and 'to-the-point' library to parse launch arguments in .NET and .NET Core applications.

This library is an improved port of my [PB-Arguments](https://github.com/aziascreations/PB-Arguments) library that intended to achieve the same goals but was missing support for some features.

## Features
* Easy to use, lightweight and 'to-the-point' philosophy
  * No unnecessary types, classes, procedures and whatnot
  * Loose declaration, registration and parsing can be done in a [**single** human-readable 'nested' statement](NibblePoker.Library.Arguments.Demo.LooseDeclaration/Program.cs).
* Support for 'git-like' verbs - **(Not tested yet)**
* Different behavior for options
  * Required options
  * Repeatable flag-like options
  * Multiple value
  * Multiple default option per verb with index-based ordering
  * Hidden in help text
  * Early parser exit
* <s>Configurable help text printer</s> (TODO)
* Easy exception filtering with inheritance
  * 1 common parent
  * 3 child for distinct parts of the library
  * 13 final errors thrown in specific places.

## Requirements
* Any OS
* Any CPU
* .NET v6.0
* C# 10.0

## Installation
TODO

## Documentation
TODO

## Examples
* [Loose Declaration](NibblePoker.Library.Arguments.Demo.LooseDeclaration/)
  * *Declaration, registration and parsing done in a single nested statement*
* ???

## License
[MIT License](LICENSE)
