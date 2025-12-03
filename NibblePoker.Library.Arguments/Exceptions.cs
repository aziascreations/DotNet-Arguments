using System;

// ReSharper disable ArrangeNamespaceBody
namespace NibblePoker.Library.Arguments;

/// <summary>
///     Static class that contains all exceptions thrown in the 'NibblePoker.Library.Arguments' library.
/// </summary>
public static class Exceptions {
    /// <summary>
    ///     Common parent exception extended by all exceptions in this library.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class ArgumentsException(string message) : Exception(message);

    /// <summary>
    ///     Common parent exception extended by all exceptions thrown by the
    ///     <see cref="NibblePoker.Library.Arguments.Option">Option</see> class.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class OptionException(string message) : ArgumentsException(message);

    /// <summary>
    ///     Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is
    ///     instantiated without a token or a name.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class MissingOptionIdentifierException(string message) : OptionException(message);

    /// <summary>
    ///     Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is given an invalid
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags">OptionFlags</see> combination.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class InvalidFlagsException(string message) : OptionException(message);

    /// <summary>
    ///     Common parent exception extended by all exceptions thrown by the
    ///     <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> class.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class VerbException(string message) : ArgumentsException(message);

    /// <summary>
    ///     Thrown if a <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>,
    ///     whose name is <c>null</c> or empty, is passed to
    ///     <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see>.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class InvalidVerbNameException(string message) : VerbException(message);

    /// <summary>
    ///     Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see> if a given
    ///     <see cref="NibblePoker.Library.Arguments.Option">Option</see> already has a duplicate registered in
    ///     the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class DuplicateOptionException(string message) : VerbException(message);

    /// <summary>
    ///     Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see> if a given
    ///     <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> already has a duplicate registered in
    ///     the parent <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class DuplicateVerbException(string message) : VerbException(message);

    /// <summary>
    ///     Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see>
    ///     if the given <see cref="NibblePoker.Library.Arguments.Option">Option</see> has the
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">Default</see> flag and is registered
    ///     after one that also has <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>,
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class ExistingDefaultMultipleOptionException(string message) : VerbException(message);

    /// <summary>
    ///     Common parent exception extended by all exceptions thrown by the parser.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class ParserException(string message) : ArgumentsException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if it is given a <c>--</c> token twice, or after reaching the end of options.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class InvalidArgumentException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> couldn't be found while parsing.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class UnknownOptionException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only be used once was used
    ///     twice.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class RepeatedSingularOptionException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only hold one value was made to
    ///     hold more.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class OptionValueOverflowException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> needs to have a value, but is the last
    ///     argument.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class NotEnoughArgumentsException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if no appropriate <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see> flag could be found when
    ///     needed.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class NoDefaultOptionFoundException(string message) : ParserException(message);

    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if a short <see cref="NibblePoker.Library.Arguments.Option">Option</see> with an expected value isn't given at
    ///     the end of a short options block.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class OptionHasValueAndMoreShortsException(string message) : ParserException(message);
    
    /// <summary>
    ///     Thrown by
    ///     <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///     if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
    ///     <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see> flag wasn't used
    ///     after all arguments were parsed.
    /// </summary>
    /// <param name="message">Exception's detailed error message.</param>
    public class MissingRequiredOptionException(string message) : ParserException(message);
}
