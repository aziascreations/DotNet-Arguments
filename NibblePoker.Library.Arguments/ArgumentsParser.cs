namespace NibblePoker.Library.Arguments; 

public static class ArgumentsParser {
	/// <summary>
	/// Parses the given arguments into the given root <c>Verb</c>.
	/// </summary>
	/// <param name="rootVerb"></param>
	/// <param name="arguments"></param>
	/// <returns>The last used <c>Verb</c> when parsing.</returns>
	/// <exception cref="ParserException">Extended by all the following exceptions.</exception>
	/// <exception cref="InvalidArgumentException">TODO</exception>
	/// <exception cref="NoDefaultOptionFoundException">TODO</exception>
	/// <exception cref="UnknownOptionException">TODO</exception>
	/// <exception cref="RepeatedSingularOptionException">TODO</exception>
	/// <exception cref="OptionValueOverflowException">TODO</exception>
	/// <exception cref="NotEnoughArgumentsException">TODO</exception>
	/// <exception cref="OptionHasValueAndMoreShortsException">TODO</exception>
	/// <exception cref="MissingRequiredOptionException">TODO</exception>
	public static Verb ParseArguments(Verb rootVerb, string[] arguments) {
		Verb currentVerb = rootVerb;

		bool hasFinishedParsingVerbs = false;
		bool hasReachedEndOfOptions = false;
		
		Option? relevantOption = null;
		
		// Parsing arguments
		for(int iArg = 0; iArg < arguments.Length; iArg++) {
			currentVerb.WasUsed = true;
			
			// Checking for the 'StopsParsing' flags and setting 'relevantOption' to null if not encountered.
			if(relevantOption != null) {
				if(relevantOption.ShouldStopParsing()) {
					return currentVerb;
				}
				relevantOption = null;
			}
			
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
						// This condition might be redundant ! -> See (used && !repeat) ~15 lines above.
						throw new Exceptions.OptionValueOverflowException(
							"The short option '" + arguments[iArg][iChar] + "' can only have 1 argument !");
					}
					
					if(arguments.Length <= iArg + 1) {
						throw new Exceptions.NotEnoughArgumentsException(
							"Unable to get a value for '" + arguments[iArg][iChar] + " !");
					}
					
					relevantOption.Arguments.Add(arguments[iArg + 1]);
					iArg++;
					break;
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
					// 'relevantOption' will always be null in this case, there is no need to reset it !
				} else if(relevantOption != null) {
					hasFinishedParsingVerbs = true;
					
					// Not checking for flags and max value count since "GetRelevantDefaultOption" takes care of it !
					
					relevantOption.Occurrences++;
					relevantOption.Arguments.Add(arguments[iArg]);
				}
			}
		}

		// Checking the "Required" flag.
		foreach(Option option in currentVerb.Options) {
			if(option.IsRequired() && !option.WasUsed()) {
				throw new Exceptions.MissingRequiredOptionException(
					"The required option '" + option.GetFullName() + "' wasn't given !");
			}
		}
		
		return currentVerb;
	}
}