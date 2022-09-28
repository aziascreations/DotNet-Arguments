namespace NibblePoker.Library.Arguments; 

public static class ArgumentsParser {
	public static Verb ParseArguments(Verb rootVerb, string[] arguments) {
		Verb currentVerb = rootVerb;

		bool hasFinishedParsingVerbs = false;
		bool hasReachedEndOfOptions = false;
		
		for(int i = 0; i<= arguments.Length; i++) {
			currentVerb.WasUsed = true;
		
			if(arguments[i].StartsWith("--")) {
				// Long option or end of parameters
				hasFinishedParsingVerbs = true;

				if(arguments[i].Length == 2) {
					// End of options symbol
					if(hasReachedEndOfOptions) {
						throw new Exceptions.InvalidArgumentException("Dual end of options given as '--' !");
					}
					hasReachedEndOfOptions = true;
				} else if(hasReachedEndOfOptions) {
					// Default option's value that starts with "--"
					
					// TODO
				} else {
					// Generic option by '--<name>'
					Option? relevantOption = currentVerb.GetOptionByName(arguments[i][2..]);
					if(relevantOption == null) {
						throw new Exceptions.UnknownOptionException("Unable to find the '" + arguments[i][2..] +
						                                            "' option !");
					}
					
					if(relevantOption.WasUsed() && !relevantOption.IsRepeatable()) {
						throw new Exceptions.RepeatedSingularOptionException("The option '" + arguments[i][2..] +
						                                                     "' was used more than once !");
					}
					relevantOption.Occurrences++;

					if(!relevantOption.CanHaveValue()) {
						continue;
					}

					if(!relevantOption.CanHaveMultipleValue() && relevantOption.Arguments.Count >= 1) {
						throw new Exceptions.OptionValueOverflowException("The option '" + arguments[i][2..] +
						                                                  "' can only have 1 argument !");
					}
						
					if(arguments.Length <= i + 1) {
						throw new Exceptions.NotEnoughArgumentsException("Unable to get a value for '" +
						                                                 arguments[i][2..] + " !");
					}
					
					relevantOption.Arguments.Add(arguments[i + 1]);
					i++;
				}
			} else if(arguments[i].StartsWith("-")) {
			
			} else {
				// Verb or default argument
			}
		}

		return currentVerb;
	}
}