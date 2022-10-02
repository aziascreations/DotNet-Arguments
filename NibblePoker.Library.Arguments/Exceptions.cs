namespace NibblePoker.Library.Arguments; 

/// <summary>
/// Static class that contains all exceptions thrown in the 'NibblePoker.Library.Arguments' library.
/// </summary>
public static class Exceptions {
	/// <summary>
	/// Common parent exception extended by all exceptions in this library.
	/// </summary>
	public class ArgumentsException : Exception {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected ArgumentsException(string message) : base(message) {}
	}
	
	/// <summary>
	/// Common parent exception extended by all exceptions thrown by <c>Option</c> class.
	/// </summary>
	public class OptionException : ArgumentsException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected OptionException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class MissingOptionIdentifierException : OptionException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public MissingOptionIdentifierException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class InvalidFlagsException : OptionException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidFlagsException(string message) : base(message) {}
	}
	
	/// <summary>
	/// Common parent exception extended by all exceptions thrown by <c>Verb</c> class.
	/// </summary>
	public class VerbException : ArgumentsException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected VerbException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class InvalidVerbNameException : VerbException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidVerbNameException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class DuplicateOptionException : VerbException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public DuplicateOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class DuplicateVerbException : VerbException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public DuplicateVerbException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class ExistingDefaultMultipleOptionException : VerbException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public ExistingDefaultMultipleOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	/// Common parent exception extended by all exceptions thrown by the parser.
	/// </summary>
	public class ParserException : ArgumentsException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		protected ParserException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class InvalidArgumentException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public InvalidArgumentException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class UnknownOptionException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public UnknownOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class RepeatedSingularOptionException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public RepeatedSingularOptionException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class OptionValueOverflowException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public OptionValueOverflowException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class NotEnoughArgumentsException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public NotEnoughArgumentsException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class NoDefaultOptionFoundException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public NoDefaultOptionFoundException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class OptionHasValueAndMoreShortsException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public OptionHasValueAndMoreShortsException(string message) : base(message) {}
	}
	
	/// <summary>
	/// TODO
	/// </summary>
	public class MissingRequiredOptionException : ParserException {
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="message">Exception's detailed error message.</param>
		public MissingRequiredOptionException(string message) : base(message) {}
	}
}