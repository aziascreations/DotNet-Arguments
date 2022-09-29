namespace NibblePoker.Library.Arguments; 

public static class Exceptions {
	// Option exceptions
	public class OptionException : Exception {
		protected OptionException(string message) : base(message) {}
	}

	public class InvalidFlagsException : OptionException {
		public InvalidFlagsException(string message) : base(message) {}
	}

	// Verb exceptions
	public class VerbException : Exception {
		protected VerbException(string message) : base(message) {}
	}

	public class InvalidVerbNameException : VerbException {
		public InvalidVerbNameException(string message) : base(message) {}
	}
	
	public class DuplicateOptionException : VerbException {
		public DuplicateOptionException(string message) : base(message) {}
	}
	
	public class DuplicateVerbException : VerbException {
		public DuplicateVerbException(string message) : base(message) {}
	}
	
	public class ExistingDefaultMultipleOptionException : VerbException {
		public ExistingDefaultMultipleOptionException(string message) : base(message) {}
	}
	
	// Parser exceptions
	public class ParserException : Exception {
		protected ParserException(string message) : base(message) {}
	}
	
	public class InvalidArgumentException : ParserException {
		public InvalidArgumentException(string message) : base(message) {}
	}
	
	public class UnknownOptionException : ParserException {
		public UnknownOptionException(string message) : base(message) {}
	}
	
	public class RepeatedSingularOptionException : ParserException {
		public RepeatedSingularOptionException(string message) : base(message) {}
	}
	
	public class OptionValueOverflowException : ParserException {
		public OptionValueOverflowException(string message) : base(message) {}
	}
	
	public class NotEnoughArgumentsException : ParserException {
		public NotEnoughArgumentsException(string message) : base(message) {}
	}
	
	public class NoDefaultOptionFoundException : ParserException {
		public NoDefaultOptionFoundException(string message) : base(message) {}
	}
	
	public class OptionHasNotArgumentsException : ParserException {
		public OptionHasNotArgumentsException(string message) : base(message) {}
	}
	
	public class OptionHasValueAndMoreShortsException : ParserException {
		public OptionHasValueAndMoreShortsException(string message) : base(message) {}
	}
}