namespace NibblePoker.Library.Arguments; 

public static class Exceptions {
	// Verb exceptions
	public class VerbException : Exception {
		protected VerbException(string message) : base(message) {}
	}
	
	public class InvalidVerbNameException : VerbException {
		public InvalidVerbNameException(string message) : base(message) {}
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
}