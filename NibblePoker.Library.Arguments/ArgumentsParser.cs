namespace NibblePoker.Library.Arguments; 

public static class ArgumentsParser {
	public static Verb ParseArguments(Verb rootVerb, string[] arguments) {
		Verb currentVerb = rootVerb;

		bool hasFinishedParsingVerbs = false;
		bool hasReachedEndOfOptions = false;
		
		for(int iArg = 0; iArg < arguments.Length; iArg++) {
			currentVerb.WasUsed = true;
			Option? relevantOption = null;
		
			if(arguments[iArg].StartsWith("--")) {
				// Long option or end of parameters
				hasFinishedParsingVerbs = true;

				if(arguments[iArg].Length == 2) {
					// End of options symbol
					if(hasReachedEndOfOptions) {
						throw new Exceptions.InvalidArgumentException("Dual end of options given as '--' !");
					}
					hasReachedEndOfOptions = true;
				} else if(hasReachedEndOfOptions) {
					// Default option's value that starts with "--"
					relevantOption = currentVerb.GetRelevantDefaultOption();
					
					if(relevantOption == null) {
						throw new Exceptions.NoDefaultOptionFoundException(
							"No relevant option found in the '" + currentVerb.Name + "' verb !");
					}
					
					relevantOption.Occurrences++;
					relevantOption.Arguments.Add(arguments[iArg]);
				} else {
					// Generic option by '--<name>'
					relevantOption = currentVerb.GetOptionByName(arguments[iArg][2..]);
					if(relevantOption == null) {
						throw new Exceptions.UnknownOptionException(
							"Unable to find the '" + arguments[iArg][2..] + "' option !");
					}
					
					if(relevantOption.WasUsed() && !relevantOption.IsRepeatable()) {
						throw new Exceptions.RepeatedSingularOptionException(
							"The option '" + arguments[iArg][2..] + "' was used more than once !");
					}
					relevantOption.Occurrences++;

					if(!relevantOption.CanHaveValue()) {
						continue;
					}

					if(!relevantOption.CanHaveMultipleValue() && relevantOption.Arguments.Count >= 1) {
						throw new Exceptions.OptionValueOverflowException(
							"The option '" + arguments[iArg][2..] + "' can only have 1 argument !");
					}
						
					if(arguments.Length <= iArg + 1) {
						throw new Exceptions.NotEnoughArgumentsException(
							"Unable to get a value for '" + arguments[iArg][2..] + " !");
					}
					
					relevantOption.Arguments.Add(arguments[iArg + 1]);
					iArg++;
				}
			} else if(arguments[iArg].StartsWith("-")) {
				// Short option
				hasFinishedParsingVerbs = true;
				
				for(int iChar = 1; iChar < arguments[iArg].Length; iChar++) {
					relevantOption = currentVerb.GetOptionByToken(arguments[iArg][iChar]);
					
					if(relevantOption == null) {
						throw new Exceptions.UnknownOptionException(
							"Unable to find the '" + arguments[iArg][iChar] + "' short option !");
					}

					if(relevantOption.WasUsed() && !relevantOption.IsRepeatable()) {
						throw new Exceptions.RepeatedSingularOptionException(
							"The short option '" + arguments[iArg][iChar] + "' was used more than once !");
					}
					relevantOption.Occurrences++;

					if(!relevantOption.CanHaveValue()) {
						continue;
					}
					
					if(iChar + 1 < arguments[iArg].Length) {
						throw new Exceptions.OptionHasValueAndMoreShortsException("The short option '" +
							arguments[iArg][iChar] + " was given before the end of the argument");
					}
					
					if(!relevantOption.CanHaveMultipleValue() && relevantOption.Arguments.Count >= 1) {
						throw new Exceptions.OptionValueOverflowException(
							"The short option '" + arguments[iArg][iChar] + "' can only have 1 argument !");
					}
					
					if(arguments.Length <= iArg + 1) {
						throw new Exceptions.NotEnoughArgumentsException(
							"Unable to get a value for '" + arguments[iArg][iChar] + " !");
					}
					
					relevantOption.Arguments.Add(arguments[iArg + 1]);
					iArg++;
				}
			} else {
				// Verb or default argument
				Verb? desiredVerb = null;
				
				// TODO: Optimize this condition !
				if(hasFinishedParsingVerbs) {
					// Already finished parsing verbs.
					// We are sure we are parsing option's arguments.
					relevantOption = currentVerb.GetRelevantDefaultOption();
					
					if(relevantOption == null) {
						throw new Exceptions.NoDefaultOptionFoundException(
							"No relevant option found in the '" + currentVerb.Name + "' verb !");
					}
				} else {
					// We will have find out it it is a verb or an option's argument.
					// We should still be at the end of the arguments.
					
					// We check for the verb first.
					desiredVerb = currentVerb.GetSubVerbByName(arguments[iArg]);

					if(desiredVerb == null) {
						// We did not find a verb, we will now search for the default option

						relevantOption = currentVerb.GetRelevantDefaultOption();
						if(relevantOption == null) {
							throw new Exceptions.NoDefaultOptionFoundException(
								"No relevant verb or option found in the '" + currentVerb.Name + "' verb !");
						}
					}
				}
				
				// We now process and set the variables before finishing.
				if(desiredVerb != null) {
					desiredVerb.WasUsed = true;
					currentVerb = desiredVerb;
				} else if(relevantOption != null) {
					hasFinishedParsingVerbs = true;
					
					// Not checking for flags and max value count since "GetRelevantDefaultOption" takes care of it !
					
					relevantOption.Occurrences++;
					relevantOption.Arguments.Add(arguments[iArg]);
				}
			}
		}

		return currentVerb;
	}
}