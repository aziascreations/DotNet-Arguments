namespace NibblePoker.Library.Arguments; 

/// <summary>
///   Static class that contains all exceptions thrown in the 'NibblePoker.Library.Arguments' library.
/// </summary>
public static class Exceptions {
	/// <summary>
	///   Common parent exception extended by all exceptions in this library.
	/// </summary>
	public class ArgumentsException : Exception {
		/// <summary>
		///   Common parent exception extended by all exceptions in this library.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected ArgumentsException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Common parent exception extended by all exceptions thrown by the
	///    <see cref="NibblePoker.Library.Arguments.Option">Option</see> class.
	/// </summary>
	public class OptionException : ArgumentsException {
		/// <summary>
		///   Common parent exception extended by all exceptions thrown by the
		///    <see cref="NibblePoker.Library.Arguments.Option">Option</see> class.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected OptionException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is
	///    instantiated without a token or a name.
	/// </summary>
	public class MissingOptionIdentifierException : OptionException {
		/// <summary>
		///   Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is
		///    instantiated without a token or a name.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public MissingOptionIdentifierException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is given an invalid
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags">OptionFlags</see> combination.
	/// </summary>
	public class InvalidFlagsException : OptionException {
		/// <summary>
		///   Thrown if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> is given an invalid
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags">OptionFlags</see> combination.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidFlagsException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Common parent exception extended by all exceptions thrown by the
	///    <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> class.
	/// </summary>
	public class VerbException : ArgumentsException {
		/// <summary>
		///   Common parent exception extended by all exceptions thrown by the
		///    <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> class.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected VerbException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown if a <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>,
	///    whose name is <c>null</c> or empty, is passed to
	///    <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see>.
	/// </summary>
	public class InvalidVerbNameException : VerbException {
		/// <summary>
		///   Thrown if a <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>,
		///    whose name is <c>null</c> or empty, is passed to
		///    <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see>.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidVerbNameException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see> if a given
	///    <see cref="NibblePoker.Library.Arguments.Option">Option</see> already has a duplicate registered in
	///    the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
	/// </summary>
	public class DuplicateOptionException : VerbException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see> if a given
		///    <see cref="NibblePoker.Library.Arguments.Option">Option</see> already has a duplicate registered in
		///    the <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public DuplicateOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see> if a given
	///    <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> already has a duplicate registered in
	///    the parent <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
	/// </summary>
	public class DuplicateVerbException : VerbException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterVerb">Verb.RegisterVerb</see> if a given
		///    <see cref="NibblePoker.Library.Arguments.Verb">Verb</see> already has a duplicate registered in
		///    the parent <see cref="NibblePoker.Library.Arguments.Verb">Verb</see>.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public DuplicateVerbException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see>
	///    if the given <see cref="NibblePoker.Library.Arguments.Option">Option</see> has the
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">Default</see> flag and is registered
	///    after one that also has <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>,
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags.
	/// </summary>
	public class ExistingDefaultMultipleOptionException : VerbException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.Verb.RegisterOption">Verb.RegisterOption</see>
		///    if the given <see cref="NibblePoker.Library.Arguments.Option">Option</see> has the
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">Default</see> flag and is registered
		///    after one that also has <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see>,
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags.HasValue">OptionFlags.HasValue</see> and
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Repeatable">OptionFlags.Repeatable</see> flags.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public ExistingDefaultMultipleOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Common parent exception extended by all exceptions thrown by the parser.
	/// </summary>
	public class ParserException : ArgumentsException {
		/// <summary>
		///   Common parent exception extended by all exceptions thrown by the parser.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected ParserException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if it is given a <c>--</c> token twice, or after reaching the end of options.
	/// </summary>
	public class InvalidArgumentException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if it is given a <c>--</c> token twice, or after reaching the end of options.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidArgumentException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
    ///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> couldn't be found while parsing.
	/// </summary>
	public class UnknownOptionException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> couldn't be found while parsing.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public UnknownOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only be used once was used twice.
	/// </summary>
	public class RepeatedSingularOptionException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only be used once was used twice.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public RepeatedSingularOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only hold one value was made to hold more.
	/// </summary>
	public class OptionValueOverflowException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> that could only hold one value was made to hold more.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public OptionValueOverflowException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> needs to have a value, but is the last argument.
	/// </summary>
	public class NotEnoughArgumentsException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if a given <see cref="NibblePoker.Library.Arguments.Option">Option</see> needs to have a value, but is the last argument.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public NotEnoughArgumentsException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if no appropriate <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see> flag could be found when needed.
	/// </summary>
	public class NoDefaultOptionFoundException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if no appropriate <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Default">OptionFlags.Default</see> flag could be found when needed.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public NoDefaultOptionFoundException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if a short <see cref="NibblePoker.Library.Arguments.Option">Option</see> with an expected value isn't given at
	///    the end of a short options block.
	/// </summary>
	public class OptionHasValueAndMoreShortsException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if a short <see cref="NibblePoker.Library.Arguments.Option">Option</see> with an expected value isn't given at
		///    the end of a short options block.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public OptionHasValueAndMoreShortsException(string message) : base(message) {}
	}
	
	/// <summary>
	///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
	///    if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
	///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see> flag wasn't used
	///    after all arguments were parsed.
	/// </summary>
	public class MissingRequiredOptionException : ParserException {
		/// <summary>
		///   Thrown by <see cref="NibblePoker.Library.Arguments.ArgumentsParser.ParseArguments">ArgumentsParser.ParseArguments</see>
		///    if an <see cref="NibblePoker.Library.Arguments.Option">Option</see> with the
		///    <see cref="NibblePoker.Library.Arguments.OptionFlags.Required">OptionFlags.Required</see> flag wasn't used
		///    after all arguments were parsed.
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public MissingRequiredOptionException(string message) : base(message) {}
	}
}